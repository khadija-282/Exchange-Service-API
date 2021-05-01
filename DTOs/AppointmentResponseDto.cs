using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeWebApi.DTOs
{
    public class AppointmentResponseDto:BaseResponseDto
    {
        public List<AppointmentDto> Appointments { get; set; }
    }
}
