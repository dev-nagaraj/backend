using Application.Interfaces;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SalesService(BackendProContext backendProContext) : ISalesService
    {
        public async Task<bool> RefreshSalesData()
        {
            try
            {
                backendProContext.Database.ExecuteSqlRaw("CALL refresh_sales_data();");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<RevenueDetail> GetTotalRevenue(RevenueType revenueType, DateTime from, DateTime to)
        {
            var result =  backendProContext.Database.SqlQuery<RevenueDetail>($"select * from  GetTotalRevenue({revenueType}, {from},{to})");

            return result.FirstOrDefault();
        }

    }
}
