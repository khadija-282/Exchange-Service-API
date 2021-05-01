
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeWebApi.DTOs
{
    public class AppointmentDto
    {
        public ItemId Id { get; set; }
        public string Subject { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Duration { get; set; }
        public string Location { get; set; }
        public List<AttendeeDto> Attendee { get; set; }

        public AppointmentDto(ItemId id, string subject, string start, string end , int duration, string location )
        {
            Id = id;
            Subject = subject;
            Start = start;
            End = end;
            Duration = duration;
            Location = location;
        }
    }
}
