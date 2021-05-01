using ExchangeWebApi.DTOs;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ExchangeWebAPI.Services
{
    public class AppointmentService
    {
        private string password;
        private string username;
        private ExchangeService Service
        {
            get
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010);
                service.Credentials = new WebCredentials(username, password, "csn");
                service.Url = new Uri("https://outlook.office365.com/ews/exchange.asmx");
                return service;
            }
        }
        private CalendarFolder FindDefaultCalendarFolder()
        {
            return CalendarFolder.Bind(Service, WellKnownFolderName.Calendar, new PropertySet());
        }
        private CalendarFolder FindNamedCalendarFolder(string name)
        {
            Microsoft.Exchange.WebServices.Data.FolderView view = new Microsoft.Exchange.WebServices.Data.FolderView(100);
            view.PropertySet = new PropertySet(BasePropertySet.IdOnly);
            view.PropertySet.Add(FolderSchema.DisplayName);
            view.Traversal = FolderTraversal.Deep;

            SearchFilter sfSearchFilter = new SearchFilter.IsEqualTo(FolderSchema.FolderClass, "IPF.Appointment");

            FindFoldersResults findFolderResults = Service.FindFolders(WellKnownFolderName.Root, sfSearchFilter, view);
            return findFolderResults.Where(f => f.DisplayName == name).Cast<CalendarFolder>().FirstOrDefault();
        }
        public AppointmentResponseDto LoadAppointments(IConfiguration configuration, AppointmentRequestDto request)
        {
            AppointmentResponseDto response = new AppointmentResponseDto();
            List<AppointmentDto> appts = new List<AppointmentDto>();
            string secretkey = "";
            try
            {
                //Todo: verify SecretKey before the response
                secretkey = configuration.GetSection("SecretKey").Get<string>();
                if (string.Compare(secretkey, request.SecretKey, false) != 0)
                {
                    throw new Exception("Client Secret Key did not match!");
                }
               
                password = configuration.GetSection("Password").Get<string>();
                username = "";
                var roomlist = configuration.GetSection("RoomList:Rooms").Get<List<RoomDto>>().Where(r => r.RoomId == request.RoomId).ToList();
                if (roomlist.Count > 0)
                    username = roomlist[0].RoomEmail;
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Parse(configuration.GetSection("ToTime").Get<string>());
                //startDate = DateTime.Parse("2018-07-01 08:00:00");

                CalendarFolder calendar = FindNamedCalendarFolder("Calendar");  // or FindDefaultCalendarFolder()
                CalendarView cView = new CalendarView(startDate, endDate, 50);
                cView.PropertySet = new PropertySet(AppointmentSchema.Id);
                FindItemsResults<Appointment> appointments = calendar.FindAppointments(cView);

                Service.LoadPropertiesForItems(appointments, new PropertySet(AppointmentSchema.Body,
                        AppointmentSchema.RequiredAttendees, AppointmentSchema.Duration,
                        AppointmentSchema.Location, AppointmentSchema.Subject, AppointmentSchema.Organizer,
                        AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.OptionalAttendees, AppointmentSchema.IsMeeting
                        , AppointmentSchema.LastModifiedName, AppointmentSchema.LastModifiedTime));
                foreach (Appointment apt in appointments)
                {
                    //apt.Body.Text = "The meeting has been updated";
                    //apt.Update(ConflictResolutionMode.AutoResolve, SendInvitationsOrCancellationsMode.SendToAllAndSaveCopy);

                    AppointmentDto dto = new AppointmentDto(apt.Id, apt.Subject, apt.Start.ToString(),
                        apt.End.ToString(), apt.Duration.Minutes, apt.Location);
                    dto.Attendee = new List<AttendeeDto>();
                    foreach (Attendee attendee in apt.RequiredAttendees)
                    {
                        dto.Attendee.Add(new AttendeeDto(attendee.Address, attendee.Name));
                    }
                    appts.Add(dto);
                }
                appts.OrderBy(a => a.Start);
                response.IsSuccessful = true;
                response.Appointments = appts;
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
