using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Models
{
    /// <summary>
    /// Klasa odpowiadająca za wysyłanie emaili.
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recipient">Adres email odbiorcy.</param>
        /// <param name="subject">Temat emaila.</param>
        /// <param name="body">Treść emaila.</param>
        public EmailSender(MailAddress recipient, string subject, string body)
        {
            this.recipient = recipient;
            this.subject = subject;
            this.body = body;
        }

        /// <summary>
        /// Adres mailowy z którego będą wysyłane emiale do użytkownika. Domyślnie: pizzeriaditalia.official@gmail.com
        /// </summary>
        public string sender = "pizzeriaditalia.official@gmail.com";
        MailAddress recipient;
        string subject;
        string body;

        /// <summary>
        /// Funkcja generująca 5 znakowy kod weryfikujący.
        /// </summary>
        /// <returns>Kod w postaci string.</returns>
        public static string GenerateCode()
        {
            Random random = new Random();

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Asynchroniczna funkcja wysyłająca emaila.
        /// </summary>
        /// <returns>Nowe zadanie.</returns>
        public async Task SendMailAsync()
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(sender),
                Subject = subject,
                Body = body
            };

            mailMessage.To.Add(recipient);

            // Serwer smtp gmaila.
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
