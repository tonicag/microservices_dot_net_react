using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSProject.MonitoringAPI.Model.Dto
{
    public class DateDto
    {
        public Guid Device_id { get; set; }
        public DateTime Date { get; set; }
    }
}
