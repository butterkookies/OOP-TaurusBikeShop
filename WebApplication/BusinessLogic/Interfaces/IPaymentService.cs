using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Interfaces;

public interface IPaymentService
{
    Task<Payment?> GetByOrderIdAsync(int orderId);
    Task SubmitGCashAsync(int orderId, string refNumber, string? proofUrl);
    Task SubmitBankTransferAsync(int orderId, string refNumber, string? proofUrl);
    Task ApproveAsync(int paymentId);
    Task RejectAsync(int paymentId, string reason);
    Task<IEnumerable<Payment>> GetPendingPaymentsAsync();
}
