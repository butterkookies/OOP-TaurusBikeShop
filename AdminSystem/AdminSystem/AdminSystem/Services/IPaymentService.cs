using System.Collections.Generic;
using AdminSystem.Models;

namespace AdminSystem.Services
{
    public interface IPaymentService
    {
        IEnumerable<Payment> GetPendingVerification();
        Payment GetPaymentById(int paymentId);
        void ApprovePayment(int paymentId, string notes = null);
        void RejectPayment(int paymentId, string reason);
        // NOTE: RefundPayment intentionally does not exist.
        // Taurus Bike Shop does not offer refunds.
    }
}
