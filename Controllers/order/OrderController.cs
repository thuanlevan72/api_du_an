using FOLYFOOD.Dto;
using FOLYFOOD.Dto.oderDto;
using FOLYFOOD.Entitys;
using FOLYFOOD.Hellers.Pagination;
using FOLYFOOD.Services.Contact;
using FOLYFOOD.Services.order;
using FOLYFOOD.Services.typeProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FOLYFOOD.Controllers.order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private (string, string) GetTokenInfo()
        {
            // Lấy token từ HttpContext
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Giải mã JWT và lấy thông tin accountId từ payload
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            string accountId = jwtToken.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;

            // Lấy role của người dùng từ HttpContext
            string role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            return (accountId, role);
        }
        private protected OrderServicer orderServicer;

        public OrderController()
        {
            orderServicer = new OrderServicer();
        }
        // GET: api/<OrderController>
        [HttpGet, Authorize(Roles = "admin")]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10)
        {
            var data = await orderServicer.GetAllOrders();
            var res = PaginationHelper.GetPagedData(data, page, pageSize);
            RetunObject<PagedResult<Order>> test = new RetunObject<PagedResult<Order>>()
            {
                data = res,
                mess = res.Data.Count() > 0 ? "đã lấy được dữ liệu" : "không có data",
                statusCode = 200,
            };
            return Ok(test);
        }
        [HttpGet("getwaitingorder"), Authorize(Roles = "admin")]
        public async Task<IActionResult> getWaitingOrder(int page = 1, int pageSize = 10)
        {
            var data = await orderServicer.getWaitingOrder();
            var res = PaginationHelper.GetPagedData(data, page, pageSize);
            RetunObject<PagedResult<Order>> test = new RetunObject<PagedResult<Order>>()
            {
                data = res,
                mess = res.Data.Count() > 0 ? "đã lấy được dữ liệu" : "không có data",
                statusCode = 200,
            };
            return Ok(test);
        }
        [HttpGet("getcomplete"), Authorize(Roles = "admin")]
        public async Task<IActionResult> getOrderComplete(int page = 1, int pageSize = 10)
        {
            var data = await orderServicer.getOrderComplete();
            var res = PaginationHelper.GetPagedData(data, page, pageSize);
            RetunObject<PagedResult<Order>> test = new RetunObject<PagedResult<Order>>()
            {
                data = res,
                mess = res.Data.Count() > 0 ? "đã lấy được dữ liệu" : "không có data",
                statusCode = 200,
            };
            return Ok(test);
        }
        [HttpGet("getordersAreBeingDelivered"), Authorize(Roles = "admin")]
        public async Task<IActionResult> getOrdersAreBeingDelivered(int page = 1, int pageSize = 10)
        {
            var data = await orderServicer.getOrdersAreBeingDelivered();
            var res = PaginationHelper.GetPagedData(data, page, pageSize);
            RetunObject<PagedResult<Order>> test = new RetunObject<PagedResult<Order>>()
            {
                data = res,
                mess = res.Data.Count() > 0 ? "đã lấy được dữ liệu" : "không có data",
                statusCode = 200,
            };
            return Ok(test);
        }
        [HttpGet("orderIsCanceled"), Authorize(Roles = "admin")]
        public async Task<IActionResult> orderIsCanceled(int page = 1, int pageSize = 10)
        {
            var data = await orderServicer.orderIsCanceled();
            var res = PaginationHelper.GetPagedData(data, page, pageSize);
            RetunObject<PagedResult<Order>> test = new RetunObject<PagedResult<Order>>()
            {
                data = res,
                mess = res.Data.Count() > 0 ? "đã lấy được dữ liệu" : "không có data",
                statusCode = 200,
            };
            return Ok(test);
        }
        [HttpGet("getDetailForEmail/{email}"), Authorize(Roles = "client, admin")]
        public async Task<IActionResult> GetOrderEmail(string email)
        {
            (string accountId, string role) = GetTokenInfo();
            var data = await orderServicer.GetOrderForEmail(email, accountId, role);
            return Ok(data);
        }
        [HttpGet("getDetail/{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> getDetail(int id)
        {
            var data = await orderServicer.getDetail(id);
            if (data.Count() <= 0)
            {
                return NotFound("không tồn tại sản phẩm trong hóa đơn");
            }
            return Ok(data);
        }
        // GET api/<OrderController>/5
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var data = await orderServicer.getOrderForCodeOrder(code);
            if (data.errorOccurred)
            {
                return NotFound(data);
            }
            return Ok(data);
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequest value)
        {
            var res = await orderServicer.CreateNewOrder(value);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        // PUT api/<OrderController>/5
        [HttpPost("updateStatusOrder")]
        public async Task<IActionResult> Put(int id, int idStatus)
        {
            var res = await orderServicer.updateStatusOrder(id,idStatus);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        [HttpGet("IsUserPurchasedProduct/{id}/{idProduct}")]
        public async Task<IActionResult> IsUserPurchasedProduct(int id, int idProduct)
        {
            bool isPurchased = await orderServicer.IsUserPurchasedProduct(id, idProduct);
            return Ok(new
            {
                isPurchased
            });
        }
        [HttpPost("cancelOrder/{code}"), Authorize(Roles = "client, admin")]
        public async Task<IActionResult> cancelOrder(string code)
        {
            (string accountId, string role) = GetTokenInfo();
            var res = await orderServicer.cancelOrder(code, accountId, role);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        // DELETE api/<OrderController>/5
        [HttpGet("deleteOrder/{id}"), Authorize(Roles = "admin")]
        public void Delete(int id)
        {
        }
    }
}
