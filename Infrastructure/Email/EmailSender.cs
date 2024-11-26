using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Email
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string userEmail, string emailSubject, string msg)
        {
            if (string.IsNullOrWhiteSpace(userEmail) || !IsValidEmail(userEmail))
            {
                // Handle the case where the email address is invalid.
                throw new ArgumentException("Invalid email address.");
            }

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(
                    _config["Smtp:Username"],
                    _config["Smtp:Password"]
                ),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Smtp:Username"]),
                Subject = emailSubject,
                Body = msg,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(userEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                // The email address is not in a valid format.
                return false;
            }
        }
    }
}
