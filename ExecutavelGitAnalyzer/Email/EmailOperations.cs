using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ExecutavelGitAnalyzer.Email
{
    class EmailOperations
    {
        private static readonly string hostEmail = "smtp-mail.outlook.com";

        public static void SendEmail(BaseEmailConfig baseEmailConfig, BaseEmail baseEmail)
        {
            MailMessage msg = ConstructEmail(baseEmailConfig, baseEmail);
            Send(msg, baseEmailConfig);
        }

        private static MailMessage ConstructEmail(BaseEmailConfig baseEmailConfig, BaseEmail email)
        {
            MailMessage msg = new();

            foreach (string to in baseEmailConfig.To)
            {
                if (!string.IsNullOrEmpty(to))
                {
                    msg.To.Add(to);
                }
            }

            if (baseEmailConfig.Cc != null)
            {
                foreach (string cc in baseEmailConfig.Cc)
                {
                    if (!string.IsNullOrEmpty(cc))
                        msg.CC.Add(cc);
                }
            }

            msg.From = new MailAddress(baseEmailConfig.From, baseEmailConfig.FromNome, Encoding.UTF8);
            msg.IsBodyHtml = email.IsHtml;
            msg.Body = email.Conteudo;
            msg.Priority = baseEmailConfig.Prioridade;
            msg.Subject = baseEmailConfig.Titulo;
            msg.BodyEncoding = Encoding.UTF8;
            msg.SubjectEncoding = Encoding.UTF8;

            if (email.NomeAnexo != null)
            {
                Attachment data = new(email.NomeAnexo,
                                                 MediaTypeNames.Application.Zip);
                msg.Attachments.Add(data);
            }

            return msg;

        }

        private static void Send(MailMessage msg, BaseEmailConfig baseEmailConfig)
        {
            SmtpClient client = new();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(
                                  baseEmailConfig.Usuario,
                                  baseEmailConfig.Senha);
            client.Host = hostEmail;
            client.Port = 587;
            client.EnableSsl = true;

            try { client.Send(msg); }
            catch (Exception e)
            {
                Console.WriteLine("ERRO AO ENVIAR EMAIL: {0}", e.Message);
                throw;
            }
            msg.Dispose();
        }
    }
}
