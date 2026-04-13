using System.ComponentModel.DataAnnotations;

namespace contact.Models.Entities
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
    }
}
