namespace contact.Models.Requests
{
    public class CreateContactRequest
    {
        public int number { get; set; }
        public IFormFile photo { get; set; }
    }
}
