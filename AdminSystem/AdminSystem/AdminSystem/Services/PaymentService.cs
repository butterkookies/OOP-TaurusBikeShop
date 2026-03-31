using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminSystem.Models;
using AdminSystem.Repositories;

namespace AdminSystem.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentRepository _paymentRepo;

        public PaymentService(PaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        public IEnumerable<Payment> GetPendingVerification()
            => _paymentRepo.GetPendingVerification();

        public Task<List<Payment>> GetPendingVerificationAsync()
            => Task.Run(() => _paymentRepo.GetPendingVerification().ToList());

        public Payment GetPaymentById(int paymentId)
            => _paymentRepo.GetById(paymentId);

        public void ApprovePayment(int paymentId, string notes = null)
        {
            Payment payment = _paymentRepo.GetById(paymentId);
            if (payment == null)
                throw new InvalidOperationException(
                    "Payment #" + paymentId + " not found.");

            if (payment.PaymentStatus != PaymentStatuses.VerificationPending)
                throw new InvalidOperationException(
                    "Payment is not in VerificationPending status.");

            int userId = App.CurrentUser != null ? App.CurrentUser.UserId : 0;
            _paymentRepo.ApprovePayment(paymentId, userId, notes);
        }

        public void RejectPayment(int paymentId, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                throw new ArgumentException(
                    "Rejection reason is required.");

            Payment payment = _paymentRepo.GetById(paymentId);
            if (payment == null)
                throw new InvalidOperationException(
                    "Payment #" + paymentId + " not found.");

            if (payment.PaymentStatus != PaymentStatuses.VerificationPending)
                throw new InvalidOperationException(
                    "Payment is not in VerificationPending status.");

            int userId = App.CurrentUser != null ? App.CurrentUser.UserId : 0;
            _paymentRepo.RejectPayment(paymentId, userId, reason);
        }
    }
}
