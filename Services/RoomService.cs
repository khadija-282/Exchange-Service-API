using ExchangeWebApi.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace ExchangeWebAPI.Services
{
    public class RoomService
    {
   
        public RoomListResponseDto RoomList(IConfiguration configuration, RoomListRequestDto request)
        {
            RoomListResponseDto response = new RoomListResponseDto();
            List<RoomDto> roomlist = new List<RoomDto>();
            string secretkey ="";
            try
            {
                //AuthenticationService authenticate= new AuthenticationService();
                //authenticate.AuthenticateUser();

                //Todo: verify SecretKey before the response
                secretkey = configuration.GetSection("SecretKey").Get<string>();
                if (string.Compare(secretkey, request.SecretKey, false) != 0)
                {
                    throw new Exception("Client Secret Key did not match!");
                }
                roomlist = configuration.GetSection("RoomList:Rooms").Get<List<RoomDto>>();

                response.IsSuccessful = true;
                response.Rooms = roomlist;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }

}
