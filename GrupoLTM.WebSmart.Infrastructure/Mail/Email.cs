using GrupoLTM.WebSmart.Infrastructure.Helpers;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace GrupoLTM.WebSmart.Infrastructure.Mail
{
    public class Email
    {
        public EnviarEmailResult EnviarEmailAutenticado(
            string strDisplayFrom,
            string strFrom,
            string strTo,
            string strSubject,
            string strBody,
            string strSmtp,
            string strPorta,
            string login,
            string senha,
            string arquivoAnexo = null)
        {

            EnviarEmailResult enviarEmailResult = new EnviarEmailResult();
            enviarEmailResult.emailEnviado = false;
            enviarEmailResult.mensagem = "";

            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(strSmtp);

                SmtpServer.Credentials = new NetworkCredential(login, senha);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Port = Int32.Parse(strPorta);
                SmtpServer.EnableSsl = true;

                MailAddress mailFrom = new MailAddress(strFrom, strDisplayFrom);
                MailAddress mailTo = new MailAddress(strTo);

                mail.From = mailFrom;
                mail.To.Add(strTo);
                mail.Subject = strSubject;
                mail.Body = StringHelper.ReplaceEnconding(strBody);
                mail.IsBodyHtml = true;

                mail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                mail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");

                if (arquivoAnexo != null)
                {
                    Attachment attachment = new Attachment(arquivoAnexo, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(arquivoAnexo);
                    disposition.ModificationDate = File.GetLastWriteTime(arquivoAnexo);
                    disposition.ReadDate = File.GetLastAccessTime(arquivoAnexo);
                    disposition.FileName = Path.GetFileName(arquivoAnexo);
                    disposition.Size = new FileInfo(arquivoAnexo).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                }



                SmtpServer.Send(mail);

                mail.Dispose();

                enviarEmailResult.emailEnviado = true;
                enviarEmailResult.mensagem = "Mensagem enviada com sucesso.";

                return enviarEmailResult;

            }
            catch (Exception exc)
            {
                enviarEmailResult.emailEnviado = false;
                enviarEmailResult.mensagem = "Não foi possível enviar a mensagem.\n" + exc.Message;

                return enviarEmailResult;
            }
        }

        public object EnviarEmailAutenticado()
        {
            throw new NotImplementedException();
        }

        public static bool EnviarEmail(string assunto, string content, bool IsBodyHtml = false)
        {
            try
            {
                // Obtém dados
                string address = ConfigurationManager.AppSettings["smtp"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["portaSmtp"]);
                string user = ConfigurationManager.AppSettings["user"];
                string password = ConfigurationManager.AppSettings["password"];
                string destinatario = ConfigurationManager.AppSettings["destinatario"];
                string domain = ConfigurationManager.AppSettings["domain"];

                // cria uma mensagem
                MailMessage mensagemEmail = new MailMessage(user, destinatario, assunto, content);
                mensagemEmail.IsBodyHtml = IsBodyHtml;

                SmtpClient client = new SmtpClient(address, port);
                client.EnableSsl = true;
                NetworkCredential cred = new NetworkCredential(user, password, domain);
                client.Credentials = cred;

                // envia a mensagem
                client.Send(mensagemEmail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool ValidarEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

    public class EnviarEmailResult
    {
        public bool emailEnviado { get; set; }
        public string mensagem { get; set; }
    }
}
