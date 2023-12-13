using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSProject.DeviceAPI.Model
{
    public class Device
    {
        [Key]
        public Guid Id { get; set; }  
        public string Description { get; set; } 
        public string Address { get; set; }
        public double MaximumHourlyEnergyConsumption { get; set; }
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public UserMapping? User { get; set; }
    }
}
