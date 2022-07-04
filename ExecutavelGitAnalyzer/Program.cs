using ExecutavelGitAnalyzer.Email;
using System;
using System.Configuration;

namespace ExecutavelGitAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            const int INTERVALO_ANALISE = 60000; //tempo em milisegundos


            while (true)
            {
                GitOperations.ReadAllRepos();
                System.Threading.Thread.Sleep(INTERVALO_ANALISE);
            }

        }

        private static void mockTest()
        {
            //Util.Tools.InitalConfig();
            //var repos = GitOperations.ListRepos();
            //GitOperations.DownloadRepo("https://tfs.seniorsolution.com.br/Eseg/_git/eSeg_Vida_1841");
            //GitOperations.ReadAllRepos();

            //BaseEmail bs = new();
            //bs.IsHtml = false;
            //bs.Conteudo = repos.ToString();
            //bs.NomeAnexo = null;

            //BaseEmailConfig bsc = new();
            //bsc.Usuario = ConfigurationManager.AppSettings["username"];
            //bsc.Senha = ConfigurationManager.AppSettings["password"];
            //bsc.Prioridade = System.Net.Mail.MailPriority.Normal;
            //bsc.Titulo = "Isso é um teste";
            //bsc.To = new string[] { "digozanchetta@gmail.com" };
            //bsc.Cc = null;
            //bsc.From = ConfigurationManager.AppSettings["username"];
            //bsc.FromNome = ConfigurationManager.AppSettings["name"];

            //Email.EmailOperations.SendEmail(bsc, bs);
        }

    }
}
