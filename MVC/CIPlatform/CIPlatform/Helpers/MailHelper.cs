﻿
using System.Net;

using System.Net.Mail;

namespace CIPlatform.Helpers
{
    public class MailHelper
    {
        private IConfiguration configuration;

        public MailHelper(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public bool Send(string to, string content)
        {
            try
            {
                var host = configuration["Gmail:Host"];
                var port = int.Parse(configuration["Gmail:Port"]);
                var username = configuration["Gmail:Username"];
                var password = configuration["Gmail:Password"];
                var enable = bool.Parse(configuration["Gmail:SMTP:starttls:enable"]);

                var smtpClient = new System.Net.Mail.SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };

                var mailMessage = new MailMessage("patelmanthan2000@gmail.com", to);
                mailMessage.Subject = "Forget Password";
                mailMessage.Body = content;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Send(string to, string content, string tital)
        {
            try
            {
                var host = configuration["Gmail:Host"];
                var port = int.Parse(configuration["Gmail:Port"]);
                var username = configuration["Gmail:Username"];
                var password = configuration["Gmail:Password"];
                var enable = bool.Parse(configuration["Gmail:SMTP:starttls:enable"]);

                var smtpClient = new System.Net.Mail.SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enable,
                    Credentials = new NetworkCredential(username, password)
                };

                var mailMessage = new MailMessage("patelmanthan2000@gmail.com", to);
                mailMessage.Subject = tital;
                mailMessage.Body = content;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
