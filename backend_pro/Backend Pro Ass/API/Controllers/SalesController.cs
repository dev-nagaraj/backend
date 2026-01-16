using Application.Command;
using Application.Interfaces;
using Application.Query;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class SalesController(ISalesService salesService) : ControllerBase
    {

        [HttpPost(Name = "refresh")]
        public ActionResult Refresh()
        {
            try
            {
                using var _ = new SalesCommand(salesService).RefreshSalesData();

                return Ok("Refreshed successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while refreshing sales data.");
            }
        }

        [HttpGet(Name = "refresh/{revenueType}")]
        public RevenueDetail Revenue( RevenueType revenueType, DateTime from , DateTime to)
        {
            var result = new RevenueDetail();
            try
            {
                var obj = new SalesQuery(salesService);
                result = obj.Revenue(revenueType, from, to).Result;
            }
            catch (Exception ex)
            {
                return new RevenueDetail { Amount = 0,};
            }
            return result
            ;
        }
    }
}
