using contact.Data;
using contact.Models.Entities;
using contact.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace contact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        AppDbContext db;

        public UserController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult getAll()
        {
            var query = db.user.ToList();
            return Ok(new {
                message = "Get data success", 
                data = query
            });
        }

        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            var query = db.user.FirstOrDefault(x => x.id == id);

            if (query == null)
            {
                return BadRequest(new { message = "Get data failed" });
            }

            return Ok(new
            {
                message = "Get data success",
                data = query
            });
        }

        [HttpPost("create")]
        public IActionResult create(CreateUserRequest dto)
        {
            var query = new User();
            query.username = dto.username;
            query.password = dto.password;
            query.name = dto.name;

            db.user.Add(query);
            db.SaveChanges();

            return StatusCode(201, new
            {
                message = "Create data succes",
                data = query
            });
        }

        [HttpPut("update/{id}")]
        public IActionResult update(int id, CreateUserRequest dto)
        {
            var query = db.user.FirstOrDefault(x => x.id == id);

            if (query == null)
            {
                return BadRequest(new { message = "User not found" });
            }

            query.username = dto.username;
            query.password = dto.password;
            query.name = dto.name;

            db.SaveChanges();

            return StatusCode(200, new
            {
                message = "Update data succes",
                data = query
            });
        }

        public string generateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("12345678912345678912345678912345678")
                    );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id", user.id.ToString()),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpDelete("delete/{id}")]
        public IActionResult delete(int id)
        {
            var query = db.user.FirstOrDefault(x => x.id == id);

            if (query == null)
            {
                return BadRequest(new { message = "User not found" });
            }

            db.user.Remove(query);
            db.SaveChanges();

            return StatusCode(200, new
            {
                message = "Delete data succes",
                data = query
            });
        }

        [HttpPost("login")]
        public IActionResult login(LoginRequest dto)
        {
            var query = db.user.FirstOrDefault(x => x.username == dto.username && x.password == dto.password);

            if (query == null)
            {
                return StatusCode(201, new
                {
                    message = "Your data is not valid",
                });
            }

            var token = generateToken(query);

            return StatusCode(201, new
            {
                message = "Login success",
                data = query,
                token = token,
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult me()
        {
            var id = User.FindFirst("id")?.Value;

            if (id == null)
            {
                return StatusCode(404, new
                {
                    message = "User not found",
                });
            }

            return Ok(new { data = db.user.FirstOrDefault(x => x.id == Convert.ToInt32(id)) });
        }

    }
}
