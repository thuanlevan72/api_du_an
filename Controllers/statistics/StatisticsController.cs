using FOLYFOOD.Services.statistics;
using FOLYFOOD.Services.typeProduct;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FOLYFOOD.Controllers.statistics
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private protected StatisticsService statisticsService;

        public StatisticsController()
        {
            statisticsService = new StatisticsService();
        }
        // GET: api/<StatisticsController>
        [HttpGet("TopSellingProducts")]
        public  IActionResult GetTopSellingProducts(DateTime? startDate = null, DateTime? endDate = null)
        {
            return Ok(statisticsService.GetTopSellingProducts(startDate,endDate));
        }
        [HttpGet("CalculateMonthlyRevenue")]
        public IActionResult GetCalculateMonthlyRevenue(DateTime? startDate = null, DateTime? endDate = null)
        {
            return Ok(statisticsService.CalculateMonthlyRevenueAndOrderCount(startDate, endDate));
        }
        [HttpGet("CalculateOrderStatusData")]
        public IActionResult GetCalculateOrderStatusData(DateTime? startDate = null, DateTime? endDate = null)
        {
            return Ok(statisticsService.CalculateOrderStatusData());
        }
    }
}
