using Application.Interfaces;

namespace Application.Command
{
    public class SalesCommand (ISalesService salesService)
    {

        public async Task RefreshSalesData()
        {
            try
            {
                await salesService.RefreshSalesData();
                Console.WriteLine("Starting sales data refresh...");

            }
            catch (Exception ex)
            {
                throw;

            }
        }

    }
}
