using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Models
{
    public class EmailSender
    {
        public EmailSender(MailAddress recipient, string subject, string body)
        {
            this.recipient = recipient;
            this.subject = subject;
            this.body = body;
        }

        string sender = "pizzeriaditalia.official@gmail.com";
        MailAddress recipient;
        string subject;
        string body;

        public static string GenerateCode()
        {
            Random random = new Random();

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task SendMailAsync()
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(sender),
                Subject = subject,
                Body = body
            };

            mailMessage.To.Add(recipient);

            SmtpClient SmtpServer = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(sender, "P1zz3r1@")
            };
            
            await SmtpServer.SendMailAsync(mailMessage);
           
        }
    }
}
