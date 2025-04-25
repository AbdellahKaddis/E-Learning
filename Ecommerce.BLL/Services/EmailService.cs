using Ecommerce.Models.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Globalization;
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


        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml = false)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["Smtp:Username"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            email.Body = new TextPart(isBodyHtml ? "html" : "plain") { Text = body };

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true; // Ignore certs in dev

            await smtp.ConnectAsync(
                _configuration["Smtp:Host"],
                int.Parse(_configuration["Smtp:Port"]),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _configuration["Smtp:Username"],
                _configuration["Smtp:Password"]);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(string toEmail, string FullName, bool isBodyHtml = false)
        {


            var schoolName = "BitBot";
            var subject = $"Welcome to {schoolName}!";
            var logoUrl = "https://i.imgur.com/luvAL8B.png";



            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Registration Confirmation</title>
</head>
<body style=""margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; color: #1f2937; background-color: #f4f4f4;"">
  <table role=""presentation"" style=""width: 100%; border-collapse: collapse; background-color: #f4f4f4; padding: 0; margin: 0;"">
    <tr>
      <td align=""center"" style=""padding: 40px 10px;"">
        <table role=""presentation"" style=""width: 100%; max-width: 600px; border-collapse: collapse; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.1);"">
          <!-- Header -->
          <tr>
            <td style=""text-align: center; padding: 30px 20px; background: linear-gradient(135deg, #3b82f6, #1e3a8a);"">
              <img src='{logoUrl}' alt=""Company Logo"" style=""max-width: 180px; height: auto; display: block; margin: 0 auto;"">
              <h1 style=""color: #ffffff; font-size: 24px; font-weight: 600; margin: 20px 0 10px;"">Welcome to {schoolName}</h1>
              <p style=""color: #dbeafe; font-size: 16px; margin: 0;"">Streamline your workflows, achieve your goals</p>
            </td>
          </tr>
          <!-- Content -->
          <tr>
            <td style=""padding: 40px 20px;"">
              <p style=""color: #4b5563; font-size: 16px; line-height: 1.6; margin: 0 0 20px;"">
                Thank you for registering with {schoolName}! We're excited to have you on board. Our platform is designed to help you streamline your workflows and achieve your goals efficiently.
              </p>
              <p style=""color: #4b5563; font-size: 16px; line-height: 1.6; margin: 0 0 30px;"">
                Get started by exploring our features or contacting our support team if you have any questions.
              </p>
              <div style=""text-align: center; margin: 40px 0;"">
                <a href=""#"" style=""display: inline-block; background-color: #3b82f6; color: #ffffff; padding: 12px 24px; border-radius: 8px; text-decoration: none; font-size: 16px; font-weight: 500; transition: background-color 0.2s;"" onmouseover=""this.style.backgroundColor='#2563eb'"" onmouseout=""this.style.backgroundColor='#3b82f6'"">
                  Explore Features
                </a>
              </div>
              <!-- Support Section -->
              <div style=""background: #f8fafc; padding: 25px; border-radius: 8px; border: 1px solid #e5e7eb;"">
                <h3 style=""color: #1f2937; font-size: 18px; font-weight: 600; margin: 0 0 15px;"">Need Assistance?</h3>
                <p style=""color: #4b5563; font-size: 14px; line-height: 1.6; margin: 0 0 15px;"">
                  Our support team is ready to help you succeed:
                </p>
                <ul style=""padding-left: 20px; margin: 0; color: #4b5563; font-size: 14px;"">
                  <li style=""margin-bottom: 8px;"">Email: <a href=""mailto:support@bitbot.com"" style=""color: #3b82f6; text-decoration: none;"">support@bitbot.com</a></li>
                  <li style=""margin-bottom: 8px;"">Knowledge Base: <a href=""#"" style=""color: #3b82f6; text-decoration: none;"">Help Center</a></li>
                  <li>Live Chat: Available in your dashboard</li>
                </ul>
              </div>
            </td>
          </tr>
          <!-- Footer -->
          <tr>
            <td style=""background-color: #f8fafc; padding: 20px; text-align: center; border-top: 1px solid #e5e7eb;"">
              <p style=""color: #6b7280; font-size: 14px; margin: 0 0 10px;"">© {DateTime.Now.Year} {schoolName}. All rights reserved.</p>
              <p style=""color: #6b7280; font-size: 14px; margin: 10px 0 0;"">
                <a href=""#"" style=""color: #3b82f6; text-decoration: none; margin: 0 10px;"">Privacy Policy</a> |
                <a href=""#"" style=""color: #3b82f6; text-decoration: none; margin: 0 10px;"">Terms of Service</a>
              </p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>
";
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["Smtp:Username"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            email.Body = new TextPart(isBodyHtml ? "html" : "plain") { Text = body };

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true; // Ignore certs in dev

            await smtp.ConnectAsync(
                _configuration["Smtp:Host"],
                int.Parse(_configuration["Smtp:Port"]),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _configuration["Smtp:Username"],
                _configuration["Smtp:Password"]);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        //email to parent send schedule pdf file
        public async Task SendEmailAsync(string toEmail, string fullName, byte[] pdfBytes,string className,int NumberOfSessions, bool isBodyHtml = true)
        {
            string subject = "Your Weekly Schedule";
            string logoUrl = "https://i.imgur.com/luvAL8B.png";
            string schoolName = "BitBot";
            DateTime generationDate = DateTime.Now;
            int year = ISOWeek.GetYear(generationDate);
            int week = ISOWeek.GetWeekOfYear(generationDate);
            DateTime startOfWeek = ISOWeek.ToDateTime(year, week, DayOfWeek.Monday);
            DateTime endOfWeek = startOfWeek.AddDays(6);
            // Format dates with month names
            var culture = new CultureInfo("en-US");
            var formattedStart = startOfWeek.ToString("MMMM dd", culture);
            var formattedEnd = endOfWeek.ToString("MMMM dd, yyyy", culture);

            // Create filename with class name
            var pdfFileName = $"{schoolName}_Schedule_{formattedStart}-{formattedEnd}_{className}.pdf"
                .Replace(" ", "_")  // Remove spaces for file safety
                .Replace(",", "");  // Remove commas
            var body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Weekly Schedule</title>
</head>
<body style=""margin: 0; padding: 0; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; color: #1f2937; background-color: #f4f4f4;"">
  <table role=""presentation"" style=""width: 100%; border-collapse: collapse; background-color: #f4f4f4; padding: 0; margin: 0;"">
    <tr>
      <td align=""center"" style=""padding: 40px 10px;"">
        <table role=""presentation"" style=""width: 100%; max-width: 600px; border-collapse: collapse; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.1);"">
          <!-- Header -->
          <tr>
            <td style=""text-align: center; padding: 30px 20px; background: linear-gradient(135deg, #3b82f6, #1e3a8a);"">
              <img src='{logoUrl}' alt=""School Logo"" style=""max-width: 180px; height: auto; display: block; margin: 0 auto;"">
              <h1 style=""color: #ffffff; font-size: 24px; font-weight: 600; margin: 20px 0 10px;"">Hello, {fullName}!</h1>
              <p style=""color: #dbeafe; font-size: 16px; margin: 0;"">Your children weekly schedule is ready</p>
            </td>
          </tr>
          <!-- Content -->
          <tr>
            <td style=""padding: 40px 20px;"">
              <p style=""color: #4b5563; font-size: 16px; line-height: 1.6; margin: 0 0 20px;"">
                Attached you'll find your children schedule for the week of {startOfWeek:MMMM dd} - {endOfWeek:MMMM dd, yyyy}.
              </p>
              <div style=""background: #f8fafc; padding: 20px; border-radius: 8px; margin: 25px 0; border: 1px solid #e5e7eb;"">
                <p style=""color: #1f2937; font-size: 16px; margin: 0 0 15px; font-weight: 500;"">Schedule Details:</p>
                <ul style=""padding-left: 20px; color: #4b5563; font-size: 14px; line-height: 1.6; margin: 0;"">
                  <li style=""margin-bottom: 10px;"">Class: {className}</li>
                  <li style=""margin-bottom: 10px;"">Week: {startOfWeek:MMMM dd} - {endOfWeek:MMMM dd, yyyy}</li>
     <li>Total Sessions: {NumberOfSessions}</li>
                </ul>
              </div>
              <p style=""color: #4b5563; font-size: 16px; line-height: 1.6; margin: 20px 0 30px;"">
                Please review the attached PDF for your complete schedule details.
              </p>
              <!-- Footer Notes -->
              <div style=""margin-top: 30px; padding-top: 20px; border-top: 1px solid #e5e7eb;"">
                <h3 style=""color: #1f2937; font-size: 18px; font-weight: 600; margin: 0 0 15px;"">Need Help?</h3>
                <ul style=""padding-left: 20px; color: #4b5563; font-size: 14px; line-height: 1.6; margin: 0;"">
                  <li style=""margin-bottom: 10px;"">Contact your academic advisor for schedule changes</li>
                  <li>Email <a href=""mailto:bitbot@bitbot.com"" style=""color: #3b82f6; text-decoration: none;"">bitbot@bitbot.com</a> for technical support</li>
                </ul>
              </div>
            </td>
          </tr>
          <!-- Footer -->
          <tr>
            <td style=""background-color: #f8fafc; padding: 20px; text-align: center; border-top: 1px solid #e5e7eb;"">
              <p style=""color: #6b7280; font-size: 14px; margin: 0 0 10px;"">© {DateTime.Now.Year} {schoolName}. All rights reserved.</p>
              <p style=""color: #6b7280; font-size: 14px; margin: 0;"">Technopark</p>
              <p style=""color: #6b7280; font-size: 14px; margin: 10px 0 0;"">
                <a href=""#"" style=""color: #3b82f6; text-decoration: none; margin: 0 10px;"">Privacy Policy</a> |
                <a href=""#"" style=""color: #3b82f6; text-decoration: none; margin: 0 10px;"">Unsubscribe</a>
              </p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["Smtp:Username"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = isBodyHtml ? body : null,
                TextBody = !isBodyHtml ? body : null
            };

            builder.Attachments.Add(pdfFileName, pdfBytes, ContentType.Parse("application/pdf"));
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await smtp.ConnectAsync(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
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
