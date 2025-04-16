using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string body, bool isBodyHtml = false)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["Smtp:Username"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            // Set body as HTML if isBodyHtml is true, otherwise plain text
            email.Body = new TextPart(isBodyHtml ? "html" : "plain") { Text = body };

            using var smtp = new SmtpClient();
            //  Add this line to ignore certificate validation for local not production
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            smtp.Connect(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Basic email pattern
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

    }
}
