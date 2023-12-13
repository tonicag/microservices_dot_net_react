using DSProject.MonitoringAPI.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq;

namespace DSProject.MonitoringAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MeasurementsController(AppDbContext appDbContext)
        {
            this._db = appDbContext;
        }
        [HttpDelete]
        public async Task<ActionResult> deleteAllMeasurements([FromQuery] Guid device_id)
        {
            foreach (var m in _db.Measurements.Where(m => m.Device.device_id == device_id))
            {
                _db.Remove(m);
            }
            _db.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> getMeasurements([FromBody] DateDto date)
        {
            var dayMeasurements = _db.Measurements.Where(d => d.Device.device_id == date.Device_id && d.Timestamp.Year == date.Date.Year && d.Timestamp.Month == date.Date.Month && d.Timestamp.Day == date.Date.Day)
                .OrderBy(m => m.Timestamp);
            List<HourlyEntity> result = new List<HourlyEntity>();
            for (int i = 0; i <= 24; i++)
            {
                double sum = 0;
                var hourResult = dayMeasurements.Where(d => d.Timestamp.Hour == i);
                var first = hourResult.FirstOrDefault();
                foreach (var h in hourResult)
                {
                    if (first != null)
                    {
                        sum = sum + h.Value - first.Value;
                    }
                }
                result.Add(new HourlyEntity
                {
                    Value = sum,
                    Id = i
                });
            }
            return Ok(result);
        }
    }
}
