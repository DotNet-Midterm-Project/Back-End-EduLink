using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace EduLink.Repositories.Services
{
public class GoogleCalendarService
{
    private static string[] Scopes = { "https://www.googleapis.com/auth/calendar" };
    private static string ApplicationName = "EDULINK";

        public static CalendarService GetCalendarService()
        {
            UserCredential credential;

            try
            {
                string credentialsPath = "credentials.json";

                // تأكيد قراءة الملف
                if (!File.Exists(credentialsPath))
                {
                    throw new FileNotFoundException($"The credentials file was not found at {credentialsPath}");
                }

                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    Console.WriteLine("Starting authorization process...");

                    // Print a message before authorizing
                    Console.WriteLine("Please visit the following URL to authorize:");
                    Console.WriteLine("https://accounts.google.com/o/oauth2/auth");

                    // Use the GoogleWebAuthorizationBroker to authorize
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;

                    Console.WriteLine("Authorization completed successfully.");
                }

                // Create and return the CalendarService
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                return service;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

    }

}

