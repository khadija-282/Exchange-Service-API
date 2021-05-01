using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeWebApi.DTOs
{
    public class AppointmentRequestDto:BaseRequestDto
    {
        public int RoomId { get; set; }
 
    }
}
