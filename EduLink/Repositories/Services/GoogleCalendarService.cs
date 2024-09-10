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
        // Define the required scopes for Google Calendar API access
        private static string[] Scopes = { "https://www.googleapis.com/auth/calendar" };

        // The name of the application for identification with Google API
        private static string ApplicationName = "EDULINK";

        // Method to get the authenticated Google Calendar service
        public static CalendarService GetCalendarService()
        {
            UserCredential credential; // Holds the user credential after authorization

            try
            {
                string credentialsPath = "credentials.json"; // Path to the credentials file

                // Check if the credentials file exists
                if (!File.Exists(credentialsPath))
                {
                    // Throw an exception if the credentials file is not found
                    throw new FileNotFoundException($"The credentials file was not found at {credentialsPath}");
                }

                // Open the credentials file and initiate authorization
                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json"; // Path to store the token after successful authorization

                    // Use GoogleWebAuthorizationBroker to authorize and get the user credentials
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets, // Load the client secrets from the credentials file
                        Scopes, // Specify the scopes required
                        "user", // The user identifier for storing tokens
                        CancellationToken.None, // No cancellation token
                        new FileDataStore(credPath, true) // Store the token in the specified path
                    ).Result;

                }

                // Create and return a new CalendarService with the obtained credentials
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential, // Pass the credentials for authentication
                    ApplicationName = ApplicationName, // Set the application name for API identification
                });

                return service; // Return the CalendarService instance
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the process and rethrow the exception
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }

}

