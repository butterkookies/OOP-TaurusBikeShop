using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(int userId, CheckoutViewModel checkout);
        Task<IEnumerable<OrderViewModel>> GetOrderHistoryAsync(int userId);
        Task<OrderViewModel?> GetOrderDetailsAsync(int orderId, int userId);
    }
}
