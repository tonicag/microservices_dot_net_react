using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSProject.DeviceAPI.Model
{
    [Keyless]
    public class DeviceMapping
    {
        [ForeignKey("Id")]
        public string DeviceId { get; set; }
        public string UserId { get; set; }
    }
}
