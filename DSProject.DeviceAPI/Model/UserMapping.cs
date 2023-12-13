using System.ComponentModel.DataAnnotations;

namespace DSProject.DeviceAPI.Model
{
    public class UserMapping
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
