using ExecutavelGitAnalyzer.Models;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace ExecutavelGitAnalyzer.Application
{
    class EmailOperations
    {
        private readonly string hostEmail = "smtp-mail.outlook.com";

        public EmailOperations()
        {

        }

        public void SendNewCommitEmail(string conteudo, string autor, string branch, string reviewEmail)
        {
            BaseEmail bs = new();
            bs.IsHtml = false;
            bs.Conteudo = conteudo;

            BaseEmailConfig bsc = new();
            bsc.Usuario = Service.CriptografiaService.Decrypt(ConfigurationManager.AppSettings["username"]);
            bsc.Senha = Service.CriptografiaService.Decrypt(ConfigurationManager.AppSettings["password"]);
            bsc.Prioridade = MailPriority.Normal;
            bsc.Titulo = @$"NOVO REVIEW DE COMMIT NA BRANCH {branch} // AUTOR {autor}";
            bsc.To = new string[] { Service.CriptografiaService.Decrypt(reviewEmail) };
            bsc.Cc = null;
            bsc.From = Service.CriptografiaService.Decrypt(ConfigurationManager.AppSettings["username"]);
            bsc.FromNome = Service.CriptografiaService.Decrypt(ConfigurationManager.AppSettings["name"]);

            SendEmail(bsc, bs);
        }

        public void SendSlaEmail(string conteudo, (string,string) devResponsavelAndSupervisor, string branch)
        {

            BaseEmail bs = new();
            bs.IsHtml = false;
            bs.Conteudo = conteudo;

            BaseEmailConfig bsc = new();
            bsc.Usuario = Service.CriptografiaService.Decrypt(ConfigurationManager.AppSettings["username"]);
            bsc.Senha = Service.CriptografiaService.Decrypt(ConfigurationManager.AppSettings["password"]);
            bsc.Prioridade = MailPriority.Normal;
            bsc.Titulo = @$"AVISO DE VIOLAÇÃO DE SLA NA BRANCH {branch} // DEV RESPONSAVEL {Service.CriptografiaService.Decrypt(devResponsavelAndSupervisor.Item1)}";
            bsc.To = new string[] { Service.CriptografiaService.Decrypt(devResponsavelAndSupervisor.Item1),
                Service.CriptografiaService.Decrypt(devResponsavelAndSupervisor.Item2) };
            bsc.Cc = null;
            bsc.From = Service.CriptografiaService.Decrypt(ConfigurationManager.AppSettings["username"]);
            bsc.FromNome = Service.CriptografiaService.Decrypt(ConfigurationManager.AppSettings["name"]);

            SendEmail(bsc, bs);
        }

        private void SendEmail(BaseEmailConfig baseEmailConfig, BaseEmail baseEmail)
        {
            MailMessage msg = ConstructEmail(baseEmailConfig, baseEmail);
            Send(msg, baseEmailConfig);
        }

        private MailMessage ConstructEmail(BaseEmailConfig baseEmailConfig, BaseEmail email)
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

            return msg;

        }

        private void Send(MailMessage msg, BaseEmailConfig baseEmailConfig)
        {
            SmtpClient client = new();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(
                                  baseEmailConfig.Usuario,
                                  baseEmailConfig.Senha);
            client.Host = hostEmail;
            client.Port = 587;
            client.EnableSsl = true;

            try
            {
                client.Send(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO AO ENVIAR EMAIL: {0}", e.Message);
            }
            msg.Dispose();
            Console.WriteLine("EMAIL ENVIADO\n");
            System.Threading.Thread.Sleep(9000);//Delay para evitar spam
        }

    }
}