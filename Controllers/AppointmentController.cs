using ExchangeWebApi.DTOs;
using ExchangeWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeWebApi.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IConfiguration configuration;
        private AppointmentService service = new AppointmentService();
        public AppointmentController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpPost]
        [Route("api/Appointment")]
        public ActionResult GetAppointment([FromBody]AppointmentRequestDto request)
        {
            var appointment = service.LoadAppointments(configuration,  request);
            return Ok(appointment);
        }
        
    }
}