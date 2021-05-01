using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeWebApi.DTOs
{
    public class BaseResponseDto
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
