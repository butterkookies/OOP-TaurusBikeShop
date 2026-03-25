// WebApplication/BusinessLogic/Services/PaymentService.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Utilities;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IPaymentService"/> — GCash and BankTransfer
/// proof submission with GCS upload, status transitions, and notification queuing.
/// Flowchart: Part 5 — Payment Processing.
/// </summary>
public sealed class PaymentService : IPaymentService
{
    private readonly AppDbContext          _context;
    private readonly PaymentRepository     _paymentRepo;
    private readonly OrderRepository       _orderRepo;
    private readonly FileUploadHelper?     _fileUpload;
    private readonly INotificationService  _notifications;
    private readonly IConfiguration        _config;
    private readonly ILogger<PaymentService> _logger;

    private const int BankTransferVerificationHours = 24;

    /// <inheritdoc/>
    public PaymentService(
        AppDbContext         context,
        PaymentRepository    paymentRepo,
        OrderRepository      orderRepo,
        FileUploadHelper?    fileUpload,
        INotificationService notifications,
        IConfiguration       config,
        ILogger<PaymentService> logger)
    {
        _context       = context       ?? throw new ArgumentNullException(nameof(context));
        _paymentRepo   = paymentRepo   ?? throw new ArgumentNullException(nameof(paymentRepo));
        _orderRepo     = orderRepo     ?? throw new ArgumentNullException(nameof(orderRepo));
        _fileUpload    = fileUpload; // null when GCS credentials are not configured locally
        _notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));
        _config        = config        ?? throw new ArgumentNullException(nameof(config));
        _logger        = logger        ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> SubmitGCashPaymentAsync(
        int orderId,
        int userId,
        string gcashRefNumber,
        Microsoft.AspNetCore.Http.IFormFile screenshot,
        CancellationToken cancellationToken = default)
    {
        // Guard: order exists and belongs to this user
        ServiceResult? guardResult = await ValidateOrderForPaymentAsync(
            orderId, userId, cancellationToken);
        if (guardResult != null) return guardResult;

        // Guard: no existing payment for this order
        bool alreadyPaid = await _context.Payments
            .AnyAsync(p => p.OrderId == orderId
                        && p.PaymentStatus != PaymentStatuses.Failed,
                cancellationToken);
        if (alreadyPaid)
            return ServiceResult.Fail("A payment has already been submitted for this order.");

        if (string.IsNullOrWhiteSpace(gcashRefNumber))
            return ServiceResult.Fail("GCash reference number is required.");

        try
        {
            // Upload screenshot to GCS
            if (_fileUpload is null)
                return ServiceResult.Fail("File uploads are unavailable — Google Cloud Storage credentials are not configured.");

            string folder = GetGCSFolder("payment-proofs", orderId);
            UploadResult upload = await _fileUpload.UploadPaymentProofAsync(
                screenshot, folder, cancellationToken);

            // Build amount from order
            decimal amount = await GetOrderGrandTotalAsync(orderId, cancellationToken);

            // Create Payment + GCashPayment subtype in one transaction
            Payment payment = new()
            {
                OrderId       = orderId,
                PaymentMethod = PaymentMethods.GCash,
                PaymentStage  = PaymentStages.Upfront,
                PaymentStatus = PaymentStatuses.VerificationPending,
                Amount        = amount,
                CreatedAt     = DateTime.UtcNow
            };

            GCashPayment gcash = new()
            {
                GcashTransactionId = gcashRefNumber.Trim(),
                ScreenshotUrl      = upload.ImageUrl,
                StorageBucket      = upload.StorageBucket,
                StoragePath        = upload.StoragePath,
                SubmittedAt        = DateTime.UtcNow
            };

            await _paymentRepo.CreateWithSubtypeAsync(payment, gcash, cancellationToken);

            // Transition order to PendingVerification
            await _orderRepo.UpdateStatusAsync(
                orderId, OrderStatuses.PendingVerification, cancellationToken);

            // SystemLog
            await WriteSystemLogAsync(userId,
                SystemLogEvents.PaymentProcessed,
                $"GCash payment submitted for Order #{orderId}. Ref: {gcashRefNumber}",
                cancellationToken);

            // Queue notification
            await QueuePaymentNotificationAsync(
                userId, orderId, NotifTypes.PaymentReceived,
                "GCash Payment Received",
                "We received your GCash payment proof. Our team will verify it shortly.",
                cancellationToken);

            return ServiceResult.Ok();
        }
        catch (InvalidOperationException ex)
        {
            // File validation failures from FileUploadHelper
            return ServiceResult.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "SubmitGCashPaymentAsync failed for order {OrderId}.", orderId);
            return ServiceResult.Fail("Unable to submit payment. Please try again.");
        }
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> SubmitBankTransferPaymentAsync(
        int orderId,
        int userId,
        string bpiReferenceNumber,
        Microsoft.AspNetCore.Http.IFormFile proofFile,
        CancellationToken cancellationToken = default)
    {
        ServiceResult? guardResult = await ValidateOrderForPaymentAsync(
            orderId, userId, cancellationToken);
        if (guardResult != null) return guardResult;

        bool alreadyPaid = await _context.Payments
            .AnyAsync(p => p.OrderId == orderId
                        && p.PaymentStatus != PaymentStatuses.Failed,
                cancellationToken);
        if (alreadyPaid)
            return ServiceResult.Fail("A payment has already been submitted for this order.");

        if (string.IsNullOrWhiteSpace(bpiReferenceNumber))
            return ServiceResult.Fail("BPI reference number is required.");

        try
        {
            if (_fileUpload is null)
                return ServiceResult.Fail("File uploads are unavailable — Google Cloud Storage credentials are not configured.");

            string folder = GetGCSFolder("payment-proofs", orderId);
            UploadResult upload = await _fileUpload.UploadPaymentProofAsync(
                proofFile, folder, cancellationToken);

            decimal amount = await GetOrderGrandTotalAsync(orderId, cancellationToken);
            DateTime deadline = DateTime.UtcNow.AddHours(BankTransferVerificationHours);

            Payment payment = new()
            {
                OrderId       = orderId,
                PaymentMethod = PaymentMethods.BankTransfer,
                PaymentStage  = PaymentStages.Upfront,
                PaymentStatus = PaymentStatuses.VerificationPending,
                Amount        = amount,
                CreatedAt     = DateTime.UtcNow
            };

            BankTransferPayment bankTransfer = new()
            {
                BpiReferenceNumber   = bpiReferenceNumber.Trim(),
                ProofUrl             = upload.ImageUrl,
                ProofStorageBucket   = upload.StorageBucket,
                ProofStoragePath     = upload.StoragePath,
                VerificationDeadline = deadline,
                SubmittedAt          = DateTime.UtcNow
            };

            await _paymentRepo.CreateWithSubtypeAsync(payment, bankTransfer, cancellationToken);

            await _orderRepo.UpdateStatusAsync(
                orderId, OrderStatuses.PendingVerification, cancellationToken);

            await WriteSystemLogAsync(userId,
                SystemLogEvents.PaymentProcessed,
                $"Bank transfer submitted for Order #{orderId}. Ref: {bpiReferenceNumber}. Deadline: {deadline:u}",
                cancellationToken);

            await QueuePaymentNotificationAsync(
                userId, orderId, NotifTypes.PaymentReceived,
                "Bank Transfer Proof Received",
                $"We received your bank transfer proof. Our team will verify it within 24 hours (by {deadline:MMMM d, yyyy h:mm tt} UTC).",
                cancellationToken);

            return ServiceResult.Ok();
        }
        catch (InvalidOperationException ex)
        {
            return ServiceResult.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "SubmitBankTransferPaymentAsync failed for order {OrderId}.", orderId);
            return ServiceResult.Fail("Unable to submit payment. Please try again.");
        }
    }

    /// <inheritdoc/>
    public async Task<PaymentDetailDto?> GetPaymentDetailAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        Order? order = await _orderRepo.GetOrderWithDetailsAsync(orderId, cancellationToken);
        if (order is null || order.UserId != userId)
            return null;

        // Compute grand total from items
        decimal subTotal = order.Items.Sum(i => i.UnitPrice * i.Quantity);
        decimal grandTotal = subTotal - order.DiscountAmount + order.ShippingFee;

        Payment? existingPayment = order.Payments
            .FirstOrDefault(p => p.PaymentStatus != PaymentStatuses.Failed);

        string? proofUrl        = null;
        string? paymentStatus   = existingPayment?.PaymentStatus;
        DateTime? verDeadline   = null;

        if (existingPayment?.GCashPayment != null)
            proofUrl = existingPayment.GCashPayment.ScreenshotUrl;
        else if (existingPayment?.BankTransferPayment != null)
        {
            proofUrl    = existingPayment.BankTransferPayment.ProofUrl;
            verDeadline = existingPayment.BankTransferPayment.VerificationDeadline;
        }

        return new PaymentDetailDto(
            OrderId:              order.OrderId,
            OrderNumber:          order.OrderNumber,
            OrderStatus:          order.OrderStatus,
            GrandTotal:           grandTotal,
            PaymentMethod:        existingPayment?.PaymentMethod ?? string.Empty,
            PaymentStatus:        paymentStatus,
            ProofImageUrl:        proofUrl,
            HasExistingPayment:   existingPayment != null,
            VerificationDeadline: verDeadline);
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    /// <summary>
    /// Returns null when the order is valid for payment submission,
    /// or a <see cref="ServiceResult"/> failure if validation fails.
    /// </summary>
    private async Task<ServiceResult?> ValidateOrderForPaymentAsync(
        int orderId, int userId, CancellationToken ct)
    {
        Order? order = await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderId == orderId, ct);

        if (order is null || order.UserId != userId)
            return ServiceResult.Fail("Order not found.");

        if (order.OrderStatus == OrderStatuses.Cancelled)
            return ServiceResult.Fail("This order has been cancelled.");

        if (order.OrderStatus == OrderStatuses.Delivered)
            return ServiceResult.Fail("This order has already been delivered.");

        return null;
    }

    private async Task<decimal> GetOrderGrandTotalAsync(int orderId, CancellationToken ct)
    {
        Order? order = await _context.Orders
            .AsNoTracking()
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderId == orderId, ct);

        if (order is null) return 0m;
        decimal sub = order.Items.Sum(i => i.UnitPrice * i.Quantity);
        return sub - order.DiscountAmount + order.ShippingFee;
    }

    private string GetGCSFolder(string prefix, int orderId)
    {
        string bucket = _config.GetValue<string>(
            "GoogleCloudStorage:Folders:PaymentProofs") ?? "payment-proofs";
        return $"{bucket}/order-{orderId}";
    }

    private async Task WriteSystemLogAsync(
        int userId, string eventType, string details, CancellationToken ct)
    {
        SystemLog log = new()
        {
            UserId    = userId,
            EventType = eventType,
            EventDescription = details,
            CreatedAt = DateTime.UtcNow
        };
        await _context.SystemLogs.AddAsync(log, ct);
        await _context.SaveChangesAsync(ct);
    }

    private async Task QueuePaymentNotificationAsync(
        int userId, int orderId,
        string notifType, string subject, string body,
        CancellationToken ct)
    {
        User? user = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserId == userId, ct);
        if (user?.Email is null) return;

        await _notifications.QueueAsync(
            channel:   NotifChannels.Email,
            notifType: notifType,
            recipient: user.Email,
            subject:   subject,
            body:      body,
            userId:    userId,
            orderId:   orderId,
            cancellationToken: ct);
    }
}