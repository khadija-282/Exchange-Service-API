using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.Graph;
using Microsoft.Identity.Client;
 

namespace ExchangeAPI.Model

{
    public class AuthenticationHelper
    {

        static string clientId = "2abbccd9-2f63-4688-b9d8-a5f18193c9ed";
        public static string[] Scopes = { "Mail.Send", "Files.ReadWrite" };

        public static PublicClientApplication IdentityClientApp = new PublicClientApplication(clientId);
        public static string TokenForUser = null;
        public static DateTimeOffset Expiration;

        public static GraphServiceClient graphClient = null;
        public static GraphServiceClient GetAuthenticatedClient()
        {
            if (graphClient == null)
            {
                // Create Microsoft Graph client.
                try
                {
                    graphClient = new GraphServiceClient(
                        "https://graph.microsoft.com/beta",
                        new DelegateAuthenticationProvider(
                            async (requestMessage) =>
                            {
                                var token = await GetTokenForUserAsync();
                                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                                // This header has been added to identify our sample in the Microsoft Graph service.  If extracting this code for your project please remove.
                                //requestMessage.Headers.Add("SampleID", "uwp-csharp-connect-sample");

                            }));
                    return graphClient;
                }

                catch (Exception ex)
                {
                    string m = ex.Message;
                }
            }

            return graphClient;
        }
        public static async Task<string> GetTokenForUserAsync()
        {
            Microsoft.Identity.Client.AuthenticationResult authResult;
            try
            {
                authResult = await IdentityClientApp.AcquireTokenSilentAsync(Scopes, IdentityClientApp.Users.First());
                TokenForUser = authResult.AccessToken;
            }

            catch (Exception)
            {
                if (TokenForUser == null || Expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
                {
                    authResult = await IdentityClientApp.AcquireTokenAsync(Scopes);

                    TokenForUser = authResult.AccessToken;
                    Expiration = authResult.ExpiresOn;
                }
            }

            return TokenForUser;
        }
    }
}
