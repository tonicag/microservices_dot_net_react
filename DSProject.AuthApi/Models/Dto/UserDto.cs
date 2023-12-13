namespace UsersAPI.Models.Dto
{
    public class UserDto
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
    }
}
