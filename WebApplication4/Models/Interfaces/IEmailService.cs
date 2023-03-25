namespace WebApplication4.Models.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string message);
    }
}
