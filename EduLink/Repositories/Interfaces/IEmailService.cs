namespace EduLink.Repositories.Interfaces
{
    public interface IEmailService
    {
        //Maybe after that I add the description for the email
        Task SendEmailAsync(string toEmail, string subject, string message);

    }
}
