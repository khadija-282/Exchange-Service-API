using ExchangeAPI.Model;
using Microsoft.Graph;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExchangeWebAPI.Services
{
    public class AuthenticationService
    {
        public string AccessToken = null;
        public void AuthenticateUser()
        {

            GraphServiceClient graphClient = new GraphServiceClient(
                        "https://graph.microsoft.com/beta",
                        new DelegateAuthenticationProvider(
                            async (requestMessage) =>
                            {
                                getAccessTokenUsingPasswordGrant();
                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", AccessToken);
                            }));
            var pictureStream = graphClient.Me.FindMeetingTimes();

        }

        public void getAccessTokenUsingPasswordGrant()
        {

            string ClientId = "2abbccd9-2f63-4688-b9d8-a5f18193c9ed";
            string UserName = "khadija.mehmood@csn.edu.pk";
            string Password = "khadija";
            string ContentType = "application/x-www-form-urlencoded";
            string GrantType = "password";
            string TokenEndpoint = "https://login.microsoftonline.com/common/oauth2/authorize";
            string ResourceId = "https://graph.microsoft.com/";
            JObject jResult = null;
            String urlParameters = String.Format(
                    "grant_type={0}&resource={1}&client_id={2}&username={3}&password={4}",
                    GrantType,
                    ResourceId,
                    ClientId,
                    UserName,
                    Password
            );

            HttpClient client = new HttpClient();
            var createBody = new StringContent(urlParameters, System.Text.Encoding.UTF8, ContentType);
            Task<HttpResponseMessage> requestTask = client.PostAsync(TokenEndpoint, createBody);
            requestTask.Wait();
            HttpResponseMessage response = requestTask.Result;

            if (response.IsSuccessStatusCode)
            {
                Task<string> responseTask = response.Content.ReadAsStringAsync();
                responseTask.Wait();
                string responseContent = responseTask.Result;
                jResult = JObject.Parse(responseContent);
            }
            AccessToken = (string)jResult["access_token"];

            if (!String.IsNullOrEmpty(AccessToken))
            {
                //Set AuthenticationHelper values so that the regular MSAL auth flow won't be triggered.
                AuthenticationHelper.TokenForUser = AccessToken;
                AuthenticationHelper.Expiration = DateTimeOffset.UtcNow.AddHours(5);
            }


        }
    }
}
