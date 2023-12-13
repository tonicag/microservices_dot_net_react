namespace DSProject.MonitoringAPI.Model
{
    public class Device
    {
        public Guid id { get; set; }
        public Guid device_id { get; set; }
        public double MaximumHourlyConsumption { get; set; }
        public double LastMeasurement { get; set; } = 0;
        public double LastHourComsumption { get; set; } = 0;
        public Guid? userId { get; set; }
    }
}
