using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace ZREL.ZiPago.Libreria.Mail
{
    public class Manage
    {

        public bool Enviar(string receptor, string direccion, string asunto, string mensaje)
        {
            bool respuesta = false;
            var message = new MimeMessage();

            try
            {
                message.From.Add(new MailboxAddress(Settings.NombreRemitente, Settings.DireccionRemitente));
                message.To.Add(new MailboxAddress(receptor, direccion));
                message.ReplyTo.Add(new MailboxAddress(Settings.NombreRemitente, Settings.DireccionRemitente));
                message.Subject = asunto;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = mensaje,
                    ContentTransferEncoding = ContentEncoding.QuotedPrintable
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(Settings.Host, Settings.Puerto);                    
                    client.Authenticate(Settings.Usuario, Settings.Clave);
                    client.Send(message);
                    client.Disconnect(true);
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return respuesta;
        }

        public bool Enviar(string receptor, string direccion, string concopia, string asunto, string mensaje)
        {
            bool respuesta = false;
            var message = new MimeMessage();

            try
            {
                message.From.Add(new MailboxAddress(Settings.NombreRemitente, Settings.DireccionRemitente));
                message.To.Add(new MailboxAddress(receptor, direccion));
                message.ReplyTo.Add(new MailboxAddress(Settings.NombreRemitente, Settings.DireccionRemitente));
                message.Cc.Add(new MailboxAddress(concopia, concopia));
                message.Subject = asunto;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = mensaje,
                    ContentTransferEncoding = ContentEncoding.QuotedPrintable
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(Settings.Host, Settings.Puerto);
                    client.Authenticate(Settings.Usuario, Settings.Clave);
                    client.Send(message);
                    client.Disconnect(true);
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return respuesta;
        }

        public bool SendMail(string[] direccion, string[] concopia, string asunto, string mensaje)
        {
            bool respuesta = false;
            var message = new MimeMessage();

            try
            {
                message.From.Add(new MailboxAddress(Settings.NombreRemitente, Settings.DireccionRemitente));
                foreach (string mail in direccion)
                {
                    message.To.Add(new MailboxAddress(mail, mail));
                }
                if (concopia != null)
                {
                    if (concopia.Length > 0)
                    {
                        foreach (string mail in concopia)
                        {
                            message.Cc.Add(new MailboxAddress(mail, mail));
                        }
                    }
                }                

                message.Subject = asunto;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = mensaje,
                    ContentTransferEncoding = ContentEncoding.QuotedPrintable
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(Settings.Host, Settings.Puerto);                    
                    client.Authenticate(Settings.Usuario, Settings.Clave);
                    client.Send(message);
                    client.Disconnect(true);
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return respuesta;
        }

    }
}
