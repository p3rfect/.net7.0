using MailKit.Net.Smtp;
using MimeKit;
using WebApplication4.Models.Interfaces;

namespace WebApplication4.Models.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string email, string subject, string message)
        {
            using var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("bsuir", "adokuchaeva11@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.elasticemail.com", 2525, false);
            await client.AuthenticateAsync("adokuchaeva11@gmail.com", "473F8437B761DE35285FEC569FBDA229BE41");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
