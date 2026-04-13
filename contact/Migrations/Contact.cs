using contact.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace contact.Migrations
{
    public class Contact
    {
        [Key]
        public int id { get; set; }
        public int number { get; set; }
        public byte[] photo { get; set; }
        public bool isActive { get; set; }

        public int userId { get; set; }

        [ForeignKey(nameof(userId))]
        public User? user { get; set; }
    }
}
