using MailKit.Net.Smtp;
using MailKit.Security;
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
            string subject = "Welcome to BitBot Enterprise!";
            string logoUrl = "https://i.imgur.com/luvAL8B.png";

            string body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Welcome Email</title>
            </head>
            <body style='margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; background-color: #f4f4f4;'>
                <table role='presentation' width='100%' cellspacing='0' cellpadding='0' style='max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 8px rgba(0,0,0,0.1);'>
                    <!-- Header -->
                    <tr>
                        <td style='background-color: #007BFF; padding: 20px; text-align: center;'>
                            <img src='{logoUrl}' alt='BitBot Enterprise Logo' style='max-width: 150px; height: auto; display: block; margin: 0 auto;'>
                        </td>
                    </tr>
                    <!-- Content -->
                    <tr>
                        <td style='padding: 40px 30px; text-align: left;'>
                            <h1 style='color: #333333; font-size: 24px; margin: 0 0 20px; font-weight: normal;'>Welcome, {FullName}!</h1>
                            <p style='color: #555555; font-size: 16px; line-height: 1.6; margin: 0 0 20px;'>Thank you for registering with BitBot Enterprise! We're excited to have you on board. Our platform is designed to help you streamline your workflows and achieve your goals efficiently.</p>
                            <p style='color: #555555; font-size: 16px; line-height: 1.6; margin: 0 0 20px;'>Get started by exploring our features or contacting our support team if you have any questions.</p>
                            <a href='https://yourdomain.com/get-started' style='display: inline-block; padding: 12px 24px; background-color: #007BFF; color: #ffffff; text-decoration: none; border-radius: 4px; font-size: 16px; font-weight: bold;'>Get Started Now</a>
                        </td>
                    </tr>
                    <!-- Footer -->
                    <tr>
                        <td style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                            <p style='color: #777777; font-size: 14px; margin: 0 0 10px;'>BitBot Enterprise &copy; {DateTime.Now.Year}. All rights reserved.</p>
                            <p style='color: #777777; font-size: 14px; margin: 0;'>
                                <a href='https://yourdomain.com/privacy' style='color: #007BFF; text-decoration: none;'>Privacy Policy</a> | 
                                <a href='https://yourdomain.com/contact' style='color: #007BFF; text-decoration: none;'>Contact Us</a>
                            </p>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";
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


        public async Task SendCourseNotificationAsync(string toEmail, string userName, string courseName, string courseDescription, bool isBodyHtml = true)
        {
            string subject = $"New Course Available: {courseName}";
            string logoUrl = "https://i.imgur.com/luvAL8B.png";

            string body = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>New Course Notification</title>
    </head>
    <body style='margin: 0; padding: 0; font-family: Arial, Helvetica, sans-serif; background-color: #f4f4f4;'>
        <table role='presentation' width='100%' cellspacing='0' cellpadding='0' style='max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 8px rgba(0,0,0,0.1);'>
            <!-- Header -->
            <tr>
                <td style='background-color: #007BFF; padding: 20px; text-align: center;'>
                    <img src='{logoUrl}' alt='BitBot Enterprise Logo' style='max-width: 150px; height: auto; display: block; margin: 0 auto;'>
                </td>
            </tr>
            <tr>
                <td style='padding: 40px 30px; text-align: left;'>
                    <h1 style='color: #333333; font-size: 24px; margin: 0 0 20px; font-weight: normal;'>Hello, {userName}!</h1>
                    <p style='color: #555555; font-size: 16px; line-height: 1.6; margin: 0 0 20px;'>We're excited to announce a new course has been added to our platform!</p>
                    <h2 style='color: #007BFF; font-size: 20px; margin: 0 0 15px;'>{courseName}</h2>
                    <p style='color: #555555; font-size: 16px; line-height: 1.6; margin: 0 0 20px;'>{courseDescription}</p>
                    <a href='https://yourdomain.com/courses' style='display: inline-block; padding: 12px 24px; background-color: #007BFF; color: #ffffff; text-decoration: none; border-radius: 4px; font-size: 16px; font-weight: bold;'>Explore the Course</a>
                </td>
            </tr>
            <tr>
                <td style='background-color: #f8f9fa; padding: 20px; text-align: center;'>
                    <p style='color: #777777; font-size: 14px; margin: 0 0 10px;'>BitBot Enterprise &copy; {DateTime.Now.Year}. All rights reserved.</p>
                    <p style='color: #777777; font-size: 14px; margin: 0;'>
                        <a href='https://yourdomain.com/privacy' style='color: #007BFF; text-decoration: none;'>Privacy Policy</a> | 
                        <a href='https://yourdomain.com/unsubscribe' style='color: #007BFF; text-decoration: none;'>Unsubscribe</a>
                    </p>
                </td>
            </tr>
        </table>
    </body>
    </html>";

            await SendEmailAsync(toEmail, subject, body, isBodyHtml);
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
