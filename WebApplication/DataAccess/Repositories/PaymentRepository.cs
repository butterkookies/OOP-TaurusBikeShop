// WebApplication/DataAccess/Repositories/PaymentRepository.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="Payment"/>, <see cref="GCashPayment"/>,
/// and <see cref="BankTransferPayment"/> entities.
/// Handles payment creation with subtype rows, status queries,
/// and pending verification retrieval.
/// </summary>
public sealed class PaymentRepository : Repository<Payment>
{
    /// <inheritdoc/>
    public PaymentRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns a payment with its method-specific subtype data loaded.
    /// Both <c>GCashPayment</c> and <c>BankTransferPayment</c> are included —
    /// only one will be non-null depending on the payment method.
    /// </summary>
    /// <param name="paymentId">The payment ID to load.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The payment with subtype data included, or <c>null</c>.</returns>
    public async Task<Payment?> GetWithSubtypeAsync(
        int paymentId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Payments
            .AsNoTracking()
            .Include(p => p.GCashPayment)
            .Include(p => p.BankTransferPayment)
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId, cancellationToken);
    }

    /// <summary>
    /// Inserts a <see cref="Payment"/> row and its corresponding subtype row
    /// (<see cref="GCashPayment"/> or <see cref="BankTransferPayment"/>)
    /// within a single database transaction.
    /// For Cash payments (POS only), pass <c>null</c> for <paramref name="subtype"/>
    /// — no subtype row is created.
    /// </summary>
    /// <param name="payment">The base payment record to insert.</param>
    /// <param name="subtype">
    /// The method-specific subtype entity to insert alongside the payment.
    /// Pass <c>null</c> for Cash payments — no subtype row is needed.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The inserted <see cref="Payment"/> with its generated PaymentId.</returns>
    public async Task<Payment> CreateWithSubtypeAsync(
        Payment payment,
        object? subtype,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(payment);

        // SqlServerRetryingExecutionStrategy (EnableRetryOnFailure) forbids
        // manually-opened transactions unless wrapped in ExecuteAsync.
        Microsoft.EntityFrameworkCore.Storage.IExecutionStrategy strategy =
            Context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using IDbContextTransaction transaction =
                await Context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await Context.Payments.AddAsync(payment, cancellationToken);
                await Context.SaveChangesAsync(cancellationToken);

                if (subtype is GCashPayment gcash)
                {
                    gcash.PaymentId = payment.PaymentId;
                    await Context.GCashPayments.AddAsync(gcash, cancellationToken);
                    await Context.SaveChangesAsync(cancellationToken);
                }
                else if (subtype is BankTransferPayment bankTransfer)
                {
                    bankTransfer.PaymentId = payment.PaymentId;
                    await Context.BankTransferPayments.AddAsync(bankTransfer, cancellationToken);
                    await Context.SaveChangesAsync(cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });

        return payment;
    }

    /// <summary>
    /// Returns all bank transfer payments currently awaiting admin verification,
    /// ordered oldest first (to prioritise payments approaching their
    /// 24-hour verification deadline).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// Payments with <c>PaymentStatus = VerificationPending</c>,
    /// with <c>BankTransferPayment</c> subtype included, ordered by <c>CreatedAt</c>.
    /// </returns>
    public async Task<IReadOnlyList<Payment>> GetPendingBankTransfersAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Payments
            .AsNoTracking()
            .Include(p => p.BankTransferPayment)
            .Where(p => p.PaymentStatus == PaymentStatuses.VerificationPending
                     && p.PaymentMethod == PaymentMethods.BankTransfer)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns all bank transfer payments whose verification deadline has passed
    /// and are still in <c>VerificationPending</c> status.
    /// Used by <c>PaymentTimeoutJob</c> to auto-hold timed-out orders.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// Payments with an exceeded <c>VerificationDeadline</c> and
    /// <c>PaymentStatus = VerificationPending</c>.
    /// </returns>
    public async Task<IReadOnlyList<Payment>> GetTimedOutBankTransfersAsync(
        CancellationToken cancellationToken = default)
    {
        DateTime now = DateTime.UtcNow;

        return await Context.Payments
            .AsNoTracking()
            .Include(p => p.BankTransferPayment)
            .Where(p => p.PaymentStatus == PaymentStatuses.VerificationPending
                     && p.PaymentMethod == PaymentMethods.BankTransfer
                     && p.BankTransferPayment != null
                     && p.BankTransferPayment.VerificationDeadline < now)
            .ToListAsync(cancellationToken);
    }
}