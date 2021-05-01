using ExchangeWebApi.DTOs;
using ExchangeWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace ExAPI.Controllers
{
   
    public class RoomController : Controller
    {
        private readonly IConfiguration configuration;
      
        private RoomService service = new RoomService();
        public RoomController(IConfiguration config)
        {
            this.configuration = config;
        }

        [HttpPost]
        [Route("api/Rooms")]
        public ActionResult GetRoomsList([FromBody]RoomListRequestDto request)
        {
            var roomlist = service.RoomList(configuration , request);
            return Ok(roomlist);
        }
    }
}