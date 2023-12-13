namespace DSProject.AuthAPI.Models.Dto
{
    public class UpdateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string? Role { get; set; }
        public string Id { get; set; }
    }
}
