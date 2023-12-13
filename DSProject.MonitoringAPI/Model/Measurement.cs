namespace DSProject.MonitoringAPI.Model
{
    public class Measurement
    {
        public Guid id { get; set; }
        public double Value { get; set; }
        public Device Device { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
