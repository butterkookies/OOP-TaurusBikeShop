using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task SubmitGCashAsync(int orderId, string refNumber, string? proofUrl)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            var order = await _context.Orders.FindAsync(orderId);

            if (payment == null)
            {
                payment = new Payment
                {
                    OrderId   = orderId,
                    Amount    = order?.TotalAmount ?? 0,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Payments.Add(payment);
            }

            payment.PaymentMethod  = "GCash";
            payment.ReferenceNumber = refNumber;
            payment.ProofImageUrl  = proofUrl;
            payment.PaymentStatus  = "PendingVerification";
            payment.PaymentDate    = DateTime.UtcNow;

            if (order != null)
                order.OrderStatus = "PendingVerification";

            await _context.SaveChangesAsync();
        }

        public async Task SubmitBankTransferAsync(int orderId, string refNumber, string? proofUrl)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            var order = await _context.Orders.FindAsync(orderId);

            if (payment == null)
            {
                payment = new Payment
                {
                    OrderId   = orderId,
                    Amount    = order?.TotalAmount ?? 0,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Payments.Add(payment);
            }

            payment.PaymentMethod  = "BankTransfer";
            payment.ReferenceNumber = refNumber;
            payment.ProofImageUrl  = proofUrl;
            payment.PaymentStatus  = "PendingVerification";
            payment.PaymentDate    = DateTime.UtcNow;

            if (order != null)
                order.OrderStatus = "PendingVerification";

            await _context.SaveChangesAsync();
        }

        public async Task ApproveAsync(int paymentId)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null) return;

            payment.PaymentStatus = "Completed";
            payment.VerifiedAt    = DateTime.UtcNow;

            if (payment.Order != null)
                payment.Order.OrderStatus = "Processing";

            await _context.SaveChangesAsync();
        }

        public async Task RejectAsync(int paymentId, string reason)
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

            if (payment == null) return;

            payment.PaymentStatus    = "VerificationRejected";
            payment.RejectionReason  = reason;

            if (payment.Order != null)
                payment.Order.OrderStatus = "PaymentRejected";

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o!.User)
                .Where(p => p.PaymentStatus == "PendingVerification")
                .ToListAsync();
        }
    }
}
