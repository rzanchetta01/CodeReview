﻿using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace ExecutavelGitAnalyzer.Email
{
    class EmailOperations
    {
        private static readonly string hostEmail = "smtp-mail.outlook.com";

        public static void SendNewCommitEmail(string conteudo, string autor, string branch)
        {
            BaseEmail bs = new();
            bs.IsHtml = false;
            bs.Conteudo = conteudo;

            BaseEmailConfig bsc = new();
            bsc.Usuario = ConfigurationManager.AppSettings["username"];
            bsc.Senha = ConfigurationManager.AppSettings["password"];
            bsc.Prioridade = MailPriority.Normal;
            bsc.Titulo = @$"NOVO REVIEW DE COMMIT NA BRANCH {branch} // AUTOR {autor}";
            bsc.To = new string[] {"rzanchetta02@gmail.com"};
            bsc.Cc = null;
            bsc.From = ConfigurationManager.AppSettings["username"];
            bsc.FromNome = ConfigurationManager.AppSettings["name"];

            SendEmail(bsc, bs);
        }

        public static void SendSlaEmail()
        {

            BaseEmail bs = new();
            bs.IsHtml = false;
            bs.Conteudo = null;

            BaseEmailConfig bsc = new();
            bsc.Usuario = ConfigurationManager.AppSettings["username"];
            bsc.Senha = ConfigurationManager.AppSettings["password"];
            bsc.Prioridade = MailPriority.Normal;
            bsc.Titulo = null;
            bsc.To = null;
            bsc.Cc = null;
            bsc.From = ConfigurationManager.AppSettings["username"];
            bsc.FromNome = ConfigurationManager.AppSettings["name"];

            SendEmail(bsc, bs);
        }

        private static void SendEmail(BaseEmailConfig baseEmailConfig, BaseEmail baseEmail)
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

            try 
            { 
                client.Send(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO AO ENVIAR EMAIL: {0}", e.Message);
                throw;
            }
            msg.Dispose();
            Console.WriteLine("EMAIL ENVIADO");
            System.Threading.Thread.Sleep(50000);//Delay para evitar spam
        }

    }
}
