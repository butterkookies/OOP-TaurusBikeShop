using System;
using System.Collections.Generic;
using AdminSystem.Models;

namespace AdminSystem.Services
{
    public interface IReportService
    {
        decimal GetTotalSalesToday();
        decimal GetTotalSalesForPeriod(DateTime from, DateTime to);
        int GetOrderCountByStatus(string status);
        IEnumerable<Order> GetTopOrdersByValue(int top);
        IEnumerable<Product> GetLowStockProducts();
        IEnumerable<InventoryLog> GetInventoryMovements(DateTime from, DateTime to);
    }
}
