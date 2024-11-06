using ChatBot.API.Areas.Dashboard.Data;
using ChatBot.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioScanAPI.Areas.Dashboard.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        [HttpGet("echocardiograms")]
        public async Task<IActionResult> EchocardiogramsByMonth()
        {
            var servicio = new DDashboard();

            var month = DateTime.Now.Month;

            var number = await servicio.EchocardiogramsByMonth(month);

            return Ok(number);
        }

        [HttpGet("patients")]
        public async Task<IActionResult> PatientsByMonth()
        {
            var servicio = new DDashboard();

            var month = DateTime.Now.Month;

            var number = await servicio.PatientsByMonth(month);

            return Ok(number);
        }

        [HttpGet("screenings")]
        public async Task<IActionResult> ScreeningByTrimester()
        {
            var servicio = new DDashboard();

            var year = DateTime.Now.Year;

            var trimesters = await servicio.ScreeningByTrimester(year);

            return Ok(trimesters);
        }
    }
}
