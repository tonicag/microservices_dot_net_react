using System.Text.Json.Serialization;

namespace DSProject.MonitoringAPI.Model.Dto
{
    public class MeasurementDto
    {
        public long Timestamp { get; set; }
        public Guid Device_id { get; set; }
        public double Value { get; set; }
    }
}
