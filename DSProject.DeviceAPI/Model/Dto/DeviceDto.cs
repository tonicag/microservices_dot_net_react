namespace DSProject.DeviceAPI.Models.Dto
{
    public class DeviceDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double MaximumHourlyEnergyConsumption { get; set; }
        public Guid? UserId { get; set; }
    }
}
