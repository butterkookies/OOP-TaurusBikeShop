using System.Collections.Generic;
using System.Threading.Tasks;
using AdminSystem.Models;

namespace AdminSystem.Services
{
    public interface IPaymentService
    {
        IEnumerable<Payment>  GetPendingVerification();
        Task<List<Payment>>   GetPendingVerificationAsync();
        Payment GetPaymentById(int paymentId);
        void ApprovePayment(int paymentId, string notes = null);
        void RejectPayment(int paymentId, string reason);
        // NOTE: RefundPayment intentionally does not exist.
        // Taurus Bike Shop does not offer refunds.
    }
}
