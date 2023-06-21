using FOLYFOOD.Dto;
using FOLYFOOD.Dto.oderDto;
using FOLYFOOD.Entitys;
using FOLYFOOD.Hellers.Pagination;
using FOLYFOOD.Services.Contact;
using FOLYFOOD.Services.order;
using FOLYFOOD.Services.typeProduct;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FOLYFOOD.Controllers.order
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private protected OrderServicer orderServicer;

        public OrderController()
        {
            orderServicer = new OrderServicer();
        }
        // GET: api/<OrderController>
        [HttpGet]
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
        [HttpGet("getDetailForEmail/{email}")]
        public async Task<IActionResult> GetOrderEmail(string email)
        {
             var data = await orderServicer.GetOrderForEmail(email);
            return Ok(data);
        }
        [HttpGet("getDetail/{id}")]
        public async Task<IActionResult> getDetail(int id)
        {
            var data = await orderServicer.getDetail(id);
            if (data.errorOccurred)
            {
                return NotFound(data);
            }
            return Ok(data);
        }
        // GET api/<OrderController>/5
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code, int page = 1, int pageSize = 10)
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
        [HttpPut]
        public async Task<IActionResult> Put(int id, int idStatus)
        {
            var res = await orderServicer.updateStatusOrder(id,idStatus);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
