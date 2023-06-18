using FOLYFOOD.Dto;
using FOLYFOOD.Dto.ProductDto;
using FOLYFOOD.Entitys;
using FOLYFOOD.Hellers.Pagination;
using FOLYFOOD.IService.IContact;
using FOLYFOOD.Services.Contact;
using FOLYFOOD.Services.product;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FOLYFOOD.Controllers.product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private protected ProductService productService;

        public ProductController()
        {
            productService = new ProductService();
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int pageSize = 10, String? search = "", Double? priceFrom = null, Double? priceTo = null)
        {
            search = search ?? "";
            priceFrom = priceFrom == 0 ? Double.MinValue : priceFrom;
            priceTo = priceTo == 0 ? double.MaxValue : priceTo;
            if(priceFrom > priceTo)
            {
                double permutation = priceTo.Value;
                priceTo = priceFrom;
                priceFrom = permutation;
            }

            IQueryable<Product> data = await productService.getProducts(search, priceFrom, priceTo);
            var res = PaginationHelper.GetPagedData<Product>(data, page, pageSize);
            RetunObject<PagedResult<Product>> dataProduct = new RetunObject<PagedResult<Product>>()
            {
                data = res,
                mess = res.Data.Count() > 0 ? "đã lấy được dữ liệu" : "không có data",
                statusCode = 200,
            };
            return Ok(dataProduct);
        }
        [HttpGet("getlimitproductseal")]
        public async Task<IActionResult> GetLimitProductSeal()
        {
            return Ok(await productService.GetLimitProductSeal());
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            RetunObject<Product> res = await productService.getDetailproduct(id);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        [HttpGet("increaseviews/{id}")]
        public async Task<IActionResult> IncreaseViews(int id)
        {
            RetunObject<Product> res = await productService.increaseViews(id);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ProductDto value)
        {
            RetunObject<Product> res = await productService.CreateProduct(value);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
           
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] ProductDto value)
        {

            RetunObject<Product> res = await productService.updateProduct(id,value);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        [HttpPut("update_status/{id}")]
        public async Task<IActionResult> PutStatus(int id)
        {
            RetunObject<Product> res = await productService.updateStatus(id);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
        }
        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
