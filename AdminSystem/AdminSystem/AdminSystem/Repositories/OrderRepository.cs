using System.Collections.Generic;
using System.Linq;
using AdminSystem.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdminSystem.Repositories
{
    public class OrderRepository : Repository<Order>, IRepository<Order>
    {
        public Order GetById(int id)
        {
            using (SqlConnection conn = GetConnection())
            {
                Order order = conn.QueryFirstOrDefault<Order>(
                    @"SELECT o.*, u.FirstName+' '+u.LastName AS CustomerName,
                             u.Email AS CustomerEmail
                      FROM [Order] o
                      INNER JOIN [User] u ON o.UserId=u.UserId
                      WHERE o.OrderId=@Id", new { Id = id });
                if (order == null) return null;
                order.Items = conn.Query<OrderItem>(
                    @"SELECT oi.*, p.Name AS ProductName, pv.VariantName
                      FROM OrderItem oi
                      INNER JOIN Product p ON oi.ProductId=p.ProductId
                      LEFT JOIN ProductVariant pv ON oi.ProductVariantId=pv.ProductVariantId
                      WHERE oi.OrderId=@Id", new { Id = id }).ToList();
                return order;
            }
        }

        public IEnumerable<Order> GetAll()
            => Query(
                @"SELECT o.*, u.FirstName+' '+u.LastName AS CustomerName
                  FROM [Order] o
                  INNER JOIN [User] u ON o.UserId=u.UserId
                  ORDER BY o.OrderDate DESC");

        public IEnumerable<Order> GetByStatus(string status)
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<Order>(
                    @"SELECT o.*, u.FirstName+' '+u.LastName AS CustomerName
                      FROM [Order] o INNER JOIN [User] u ON o.UserId=u.UserId
                      WHERE o.OrderStatus=@Status ORDER BY o.OrderDate DESC",
                    new { Status = status });
        }

        public IEnumerable<Order> GetActiveOrders()
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<Order>(
                    @"SELECT o.*, u.FirstName+' '+u.LastName AS CustomerName
                      FROM [Order] o INNER JOIN [User] u ON o.UserId=u.UserId
                      WHERE o.OrderStatus NOT IN (@S1, @S2)
                      ORDER BY o.OrderDate DESC",
                    new { S1 = OrderStatuses.Delivered, S2 = OrderStatuses.Cancelled });
        }

        public int Insert(Order entity)
        {
            throw new System.NotSupportedException(
                "Orders are created by WebApplication only.");
        }

        public void Update(Order entity)
            => Execute(
                "UPDATE [Order] SET OrderStatus=@OrderStatus, UpdatedAt=GETUTCDATE() WHERE OrderId=@OrderId",
                entity);

        public void Delete(int id)
        {
            throw new System.NotSupportedException("Orders cannot be deleted.");
        }

        public void UpdateStatus(int orderId, string newStatus)
            => Execute(
                "UPDATE [Order] SET OrderStatus=@Status, UpdatedAt=GETUTCDATE() WHERE OrderId=@OrderId",
                new { Status = newStatus, OrderId = orderId });
    }
}
