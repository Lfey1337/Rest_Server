namespace Rest_Server.DTOs
{
    public class UpdateUser
    {
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
    }
}
