using CloudinaryDotNet.Actions;
using FOLYFOOD.Dto;
using FOLYFOOD.Dto.UserDto;
using FOLYFOOD.Entitys;
using FOLYFOOD.Hellers.imageChecks;
using FOLYFOOD.Hellers.Mail;
using FOLYFOOD.Hellers.Pagination;
using FOLYFOOD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FOLYFOOD.Controllers.user
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        
        public UserController()
        {
            userService = new UserService();
        }
        

        [HttpGet, Authorize(Roles = "staff, admin")]
        public async Task<IActionResult> Get(int? roleId, int page = 1, int pageSize = 10,string? searchName = "")
        {
            searchName = searchName ?? "";
            var accounts = await userService.getListUser(searchName, roleId);
            var res = PaginationHelper.GetPagedData(accounts, page, pageSize);
            RetunObject<PagedResult<Account>> test  =new RetunObject<PagedResult<Account>>()
            {
                data = res,
                mess = res.Data.Count() > 0 ? "đã lấy được dữ liệu" : "không có data",
                statusCode = 200,
            };
            return Ok(test);
        }
        [HttpGet("/sendMail"), Authorize(Roles = "staff, admin")]
        public async Task<IActionResult> sendMail(string mailTo, string Subject)
        {
          string str = SendMail.send(mailTo, Template1.temlapteHtmlMail(), Subject);
           return Ok(str);
        }
        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await userService.getOneUser(id);
            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost, Authorize(Roles = "staff, admin")]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPost("updateUser/{id}"), Authorize(Roles = "client, admin")]
        public async Task<IActionResult> Put(int id, [FromForm] UserUpdateClient value)
        {
            // Lấy token từ HttpContext
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Giải mã JWT và lấy thông tin accountId từ payload
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            string accountId = jwtToken.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;

            // Lấy role của người dùng từ HttpContext
            string role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            RetunObject<Account> data = await userService.updateOneAccount(value, id, accountId, role);
            return Ok(data);
        }
        [HttpPost("update_avatar/{id}"), Authorize(Roles = "client, admin")]
        public async Task<IActionResult> PutImageAvatar(int id, IFormFile Avatar)
        {
            // Lấy token từ HttpContext
            string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            // Giải mã JWT và lấy thông tin accountId từ payload
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            string accountId = jwtToken.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value;

            // Lấy role của người dùng từ HttpContext
            string role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            RetunObject<string> link = await userService.updateImageAvatar(Avatar, id, accountId, role);
            return Ok(link);
        }

        // DELETE api/<UserController>/5
        [HttpGet("delete/{id}"), Authorize(Roles = "staff, admin")]
        public async Task<IActionResult> Delete(int id)
        {
            RetunObject<Account> data = await userService.DeleteUser(id);
            if (data.errorOccurred)
            {
                return NotFound(data);
            }
            return Ok(data);
        }
    }
}
