namespace contact.Models.Requests
{
    public class CreateContactRequest
    {
        public int number { get; set; }
        public byte photo { get; set; }
        public int userId { get; set; }
    }
}
