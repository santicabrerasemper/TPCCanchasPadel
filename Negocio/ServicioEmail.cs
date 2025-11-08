using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Negocio
{
    public class ServicioEmail
    {
        private const bool MODO_DEMO = true;

        private const string SmtpHost = "sandbox.smtp.mailtrap.io";
        private const int SmtpPort = 2525;
        private const string SmtpUser = "usuario";
        private const string SmtpPass = "contra";
        private const bool SmtpSsl = true;

        private const string FromEmail = "no-responder@dondejuego.com";
        private const string FromName = "Sitio DondeJuego";
        private const string ToEmail = "corporativo@dondejuego.com";

        public void EnviarContacto(string nombre, string emailCliente, string telefonoCompleto, string mensaje)
        {
            if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre requerido");
            if (string.IsNullOrWhiteSpace(emailCliente)) throw new ArgumentException("Email requerido");
            if (string.IsNullOrWhiteSpace(mensaje)) throw new ArgumentException("Mensaje requerido");

            if (MODO_DEMO) return; 

            var cuerpo = new StringBuilder()
                .AppendLine($"Nombre: {nombre}")
                .AppendLine($"Email: {emailCliente}")
                .AppendLine($"Teléfono: {telefonoCompleto}")
                .AppendLine()
                .AppendLine("Mensaje:")
                .AppendLine(mensaje)
                .ToString();

            using (var msg = new MailMessage())
            {
                msg.From = new MailAddress(FromEmail, FromName);
                msg.To.Add(ToEmail);
                msg.Subject = "Nuevo contacto desde el sitio DondeJuego";
                msg.Body = cuerpo;
                msg.IsBodyHtml = false;
                try { msg.ReplyToList.Add(new MailAddress(emailCliente, nombre)); } catch { }

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                using (var smtp = new SmtpClient(SmtpHost, SmtpPort))
                {
                    smtp.EnableSsl = SmtpSsl;
                    smtp.Credentials = new NetworkCredential(SmtpUser, SmtpPass);
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Timeout = 30000;
                    smtp.Send(msg);
                }
            }
        }
    }
}
