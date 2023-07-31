using FOLYFOOD.Dto;
using FOLYFOOD.Dto.NewsDto;
using FOLYFOOD.Entitys;
using FOLYFOOD.Services.Info;
using FOLYFOOD.Services.News;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FOLYFOOD.Controllers.news
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private protected NewsServicer newsServicer;

        public NewsController()
        {
            newsServicer = new NewsServicer();
        }
        // GET: api/<NewsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<NewsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<NewsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] NewsRequest value)
        {
            RetunObject<News> res = await newsServicer.CrateNews(value);
            if (res.errorOccurred)
            {
                return NotFound(res);
            }
            return Ok(res);
        }

        // PUT api/<NewsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<NewsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
