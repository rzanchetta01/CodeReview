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

    }
}
