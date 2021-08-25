using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Packing.Mvc.Servicios.Interfaces;

namespace Packing.Mvc.Servicios
{
    public class EnviadorCorreos : IEnviadorCorreos
    {
        private readonly IConfiguration _config;
        private readonly SmtpClient _clientMail;

        public EnviadorCorreos(IConfiguration config)
        {
            try
            {
                _config = config;
                _clientMail = new SmtpClient(_config.GetValue<string>("EmailSender:Host"))
                {
                    Credentials = new NetworkCredential(_config.GetValue<string>("EmailSender:Username"),
                        _config.GetValue<string>("EmailSender:Password")),
                    EnableSsl = _config.GetValue<bool>("EmailSender:EnableSsl"),
                    Port = _config.GetValue<int>("EmailSender:Port"),
                    UseDefaultCredentials = false,
                    Timeout = 20000
                };
            }
            catch (Exception errorException)
            {
                Console.WriteLine(errorException.Message);
            }
        }

        /// <summary>
        /// Envia el email
        /// </summary>
        /// <param name="email">Destinatario</param>
        /// <param name="subject">Asunto</param>
        /// <param name="htmlMessage">Correo con formato html</param>
        public void EnviarEmail(string email, string subject, string htmlMessage)
        {
            try
            {
                using var mail = new MailMessage();
                mail.From = new MailAddress(_config.GetValue<string>("EmailSender:From"), _config.GetValue<string>("DisplayFrom"));
                mail.To.Add(new MailAddress(email));
                mail.Subject = subject;
                mail.Body = htmlMessage;
                mail.IsBodyHtml = true;
                _clientMail.Send(mail);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}
