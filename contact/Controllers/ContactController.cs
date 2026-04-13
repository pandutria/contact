using contact.Data;
using contact.Migrations;
using contact.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace contact.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        AppDbContext db;

        public ContactController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult getAllContact()
        {
            var query = db.contact.ToList();

            return Ok(new
            {
                message = "Get data success",
                data = query
            });
        }

        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            var query = db.contact.FirstOrDefault(x => x.id == id);

            return Ok(new
            {
                message = "Get data success",
                data = query
            });
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult create(CreateContactRequest dto)
        {
            var userId = User.FindFirst("id")?.Value;

            if (userId == null)
            {
                return StatusCode(401, new { message = "User must be login first" });
            }

            var query = new Contact();
            query.number = dto.number;
            query.userId = Convert.ToInt32(userId);
            query.isActive = true;

        }
    }
}
