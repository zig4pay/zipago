using System;
using MailKit.Net.Smtp;
using MimeKit;

namespace ZREL.ZiPago.Libreria.Mail
{
    public class Manage
    {

        public string Enviar(string receptor, string direccion, string asunto, string mensaje, Mail.Settings settings)
        {
            string respuesta = "";
            var message = new MimeMessage();

            try
            {
                message.From.Add(new MailboxAddress(settings.NombreRemitente, settings.DireccionRemitente));
                message.To.Add(new MailboxAddress(receptor, direccion));
                message.ReplyTo.Add(new MailboxAddress(settings.NombreRemitente, settings.DireccionRemitente));
                message.Subject = asunto;
                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = mensaje,
                    ContentTransferEncoding = ContentEncoding.QuotedPrintable
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(settings.Host, Convert.ToInt32(settings.Puerto));
                    client.Authenticate(settings.Usuario, settings.Clave);
                    client.Send(message);
                    client.Disconnect(true);                    
                }                
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
            return respuesta;
        }

        public string Enviar(string receptor, string direccion, string concopia, string asunto, string mensaje, Mail.Settings settings)
        {
            string respuesta = "";
            var message = new MimeMessage();

            try
            {
                message.From.Add(new MailboxAddress(settings.NombreRemitente, settings.DireccionRemitente));
                message.To.Add(new MailboxAddress(receptor, direccion));
                message.ReplyTo.Add(new MailboxAddress(settings.NombreRemitente, settings.DireccionRemitente));
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
                    client.Connect(settings.Host, Convert.ToInt32(settings.Puerto));
                    client.Authenticate(settings.Usuario, settings.Clave);
                    client.Send(message);
                    client.Disconnect(true);                    
                }                
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
            return respuesta;
        }

        public string SendMail(string[] direccion, string[] concopia, string asunto, string mensaje, Mail.Settings settings)
        {
            string respuesta = "";
            var message = new MimeMessage();

            try
            {
                message.From.Add(new MailboxAddress(settings.NombreRemitente, settings.DireccionRemitente));
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
                    client.Connect(settings.Host, Convert.ToInt32(settings.Puerto));
                    client.Authenticate(settings.Usuario, settings.Clave);
                    client.Send(message);
                    client.Disconnect(true);                    
                }                
            }
            catch (Exception ex)
            {
                respuesta = ex.Message;
            }
            return respuesta;
        }

    }
}
