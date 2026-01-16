using Application.Interfaces;
using Domain;
using Domain.Enums;

namespace Application.Query
{
    public class SalesQuery(ISalesService salesService)
    {
        public async Task<RevenueDetail> Revenue( RevenueType revenueType, DateTime from, DateTime to) {            
            try
            {
                return await salesService.GetTotalRevenue(revenueType,from,to);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
