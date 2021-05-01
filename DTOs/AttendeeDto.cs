using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeWebApi.DTOs
{
    public class AttendeeDto
    {
        public AttendeeDto(string email,string name)
        {
            AttendeeEmail = email;
            AttendeeName = name;
        }
        public  string AttendeeEmail { get; set; }
        public string AttendeeName { get; set; }
    }
}
