using contact.Data;
using contact.Migrations;
using contact.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var query = db.contact.Include(x => x.user).ToList();

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
        public IActionResult create([FromForm] CreateContactRequest dto)
        {
            var userId = User.FindFirst("id")?.Value;

            if (userId == null)
            {
                return StatusCode(401, new { message = "User must be login first" });
            }

            byte[] photoByte = null;

            if (dto.photo != null)
            {
                var stream = new MemoryStream();
                dto.photo.CopyTo(stream);
                photoByte = stream.ToArray();
            }

            var query = new Contact();
            query.number = dto.number;
            query.userId = Convert.ToInt32(userId);
            query.isActive = true;
            query.photo = photoByte;    

            db.contact.Add(query);
            db.SaveChanges();

            return StatusCode(201, new
            {
                message = "Create data success",
                data = query
            });
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public IActionResult update(int id, [FromForm] CreateContactRequest dto)
        {
            var userId = User.FindFirst("id")?.Value;

            if (userId == null)
            {
                return StatusCode(401, new { message = "User must be login first" });
            }

            byte[] photoByte = null;

            if (dto.photo != null)
            {
                var stream = new MemoryStream();
                dto.photo.CopyTo(stream);
                photoByte = stream.ToArray();
            }

            var query = db.contact.FirstOrDefault(x => x.id == id);
            query.number = dto.number;
            query.userId = Convert.ToInt32(userId);
            query.isActive = true;
            query.photo = photoByte;

            db.SaveChanges();

            return StatusCode(201, new
            {
                message = "Update data success",
                data = query
            });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult delete(int id)
        {
            var query = db.contact.FirstOrDefault(x => x.id == id);

            db.contact.Remove(query);
            db.SaveChanges();

            return StatusCode(200, new
            {
                message = "Delete data success",
                data = query
            });
        }
    }
}
