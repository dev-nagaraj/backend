using Domain;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface ISalesService
    {
        Task<bool> RefreshSalesData();
        Task<RevenueDetail> GetTotalRevenue(RevenueType revenueType, DateTime from, DateTime to);    
    }
}
