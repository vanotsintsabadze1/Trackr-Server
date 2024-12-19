    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;
    using System.Net;
    using System.Net.Mail;

    namespace Trackr.Application.Services;

    public class EmailService : IEmailSender
    {
        private IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage message = new();
            string? fromEmail = _configuration.GetSection("Smtp").GetValue<string?>("FromMail");
            string? pw = _configuration.GetSection("Smtp").GetValue<string?>("FromPassword");
            message.From = new MailAddress(fromEmail!);
            message.To.Add(new MailAddress(email));
            message.Body = htmlMessage;
            message.Subject = subject;
            message.IsBodyHtml = true;

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, pw),
                EnableSsl = true,
            };

            await smtp.SendMailAsync(message);
        }
    }
