using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeWebApi.DTOs
{
    public class RoomListResponseDto:BaseResponseDto
    {
        public List<RoomDto> Rooms { get; set; }
    }
}
