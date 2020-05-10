using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System.Threading.Tasks;

namespace VG_AspNetCore_Web.Services
{
    public class EmailService : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpEmail = _configuration["SMTPEmail"];
            var smtpPassword = _configuration["SMTPPassword"];
            var smtpProvider = _configuration["SMTPProvider"];

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("VG_AspNetCore_Web Administration", smtpEmail));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpProvider, 25, false);
                await client.AuthenticateAsync(smtpEmail, smtpPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}


