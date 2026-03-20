using System.Collections.Generic;
using AdminSystem.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdminSystem.Repositories
{
    public class PaymentRepository : Repository<Payment>, IRepository<Payment>
    {
        public Payment GetById(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                Payment p = conn.QueryFirstOrDefault<Payment>(
                    "SELECT * FROM Payment WHERE PaymentId=@Id", new { Id = id });
                if (p == null) return null;
                if (p.PaymentMethod == PaymentMethods.GCash)
                    p.GCash = conn.QueryFirstOrDefault<GCashPayment>(
                        "SELECT * FROM GCashPayment WHERE PaymentId=@Id", new { Id = id });
                if (p.PaymentMethod == PaymentMethods.BankTransfer)
                    p.BankTransfer = conn.QueryFirstOrDefault<BankTransferPayment>(
                        "SELECT * FROM BankTransferPayment WHERE PaymentId=@Id", new { Id = id });
                return p;
            }
        }

        public IEnumerable<Payment> GetAll()
            => Query("SELECT * FROM Payment ORDER BY CreatedAt DESC");

        public IEnumerable<Payment> GetPendingVerification()
            => Query(
                "SELECT * FROM Payment WHERE PaymentStatus=@Status ORDER BY CreatedAt ASC",
                new { Status = PaymentStatuses.VerificationPending });

        public int Insert(Payment entity)
        {
            throw new System.NotSupportedException(
                "Payments are submitted by customers via WebApplication only.");
        }

        public void Update(Payment entity)
            => Execute(
                "UPDATE Payment SET PaymentStatus=@PaymentStatus WHERE PaymentId=@PaymentId",
                entity);

        public void Delete(int id)
        {
            throw new System.NotSupportedException("Payments cannot be deleted.");
        }

        public void ApprovePayment(int paymentId, int verifiedByUserId, string notes = null)
        {
            ExecuteTransaction((conn, tx) =>
            {
                conn.Execute(
                    "UPDATE Payment SET PaymentStatus=@Status WHERE PaymentId=@Id",
                    new { Status = PaymentStatuses.Completed, Id = paymentId }, tx);
                conn.Execute(
                    @"UPDATE BankTransferPayment
                      SET VerifiedByUserId=@UserId, VerifiedAt=GETUTCDATE(), VerificationNotes=@Notes
                      WHERE PaymentId=@Id",
                    new { UserId = verifiedByUserId, Notes = notes, Id = paymentId }, tx);
                conn.Execute(
                    @"UPDATE [Order] SET OrderStatus=@OrderStatus, UpdatedAt=GETUTCDATE()
                      WHERE OrderId=(SELECT OrderId FROM Payment WHERE PaymentId=@Id)",
                    new { OrderStatus = OrderStatuses.Processing, Id = paymentId }, tx);
            });
        }

        public void RejectPayment(int paymentId, int verifiedByUserId, string reason)
        {
            ExecuteTransaction((conn, tx) =>
            {
                conn.Execute(
                    "UPDATE Payment SET PaymentStatus=@Status WHERE PaymentId=@Id",
                    new { Status = PaymentStatuses.VerificationRejected, Id = paymentId }, tx);
                conn.Execute(
                    @"UPDATE BankTransferPayment
                      SET VerifiedByUserId=@UserId, VerifiedAt=GETUTCDATE(), VerificationNotes=@Notes
                      WHERE PaymentId=@Id",
                    new { UserId = verifiedByUserId, Notes = reason, Id = paymentId }, tx);
                conn.Execute(
                    @"UPDATE [Order] SET OrderStatus=@OrderStatus, UpdatedAt=GETUTCDATE()
                      WHERE OrderId=(SELECT OrderId FROM Payment WHERE PaymentId=@Id)",
                    new { OrderStatus = OrderStatuses.OnHold, Id = paymentId }, tx);
            });
        }
    }
}
