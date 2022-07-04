using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutavelGitAnalyzer
{
    class GitOperations
    {
        public static void DownloadRepo(string gitUrl)
        {   
            string cmdCommand = @$"/C cd repos && git clone {gitUrl}";
            Util.Tools.CmdCommand(cmdCommand);
        }

        private static string[] ListRepos()
        {
            //string command = @"/C cd repos && dir";
            //Util.Tools.CmdCommand(command);
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path = @$"{path}\repos";

            string[] repos = Directory.GetDirectories(path);
            Console.WriteLine($"LENDO TODOS OS REPOS");
            foreach (var item in repos)
            {
                Console.Write(item + "\n");
            }
            return repos;
        }

        public static void ReadAllRepos()
        {
            foreach (var folder in ListRepos())
            {
                using(var repos = new Repository(folder))
                {
                    foreach (var branch in repos.Branches)
                    {
                        var repName = folder;
                        repName = repName.Remove(0, 78); //PREVISTO DAR ERRO NO FUTURO

                        if(branch.FriendlyName.Contains("AD_Integracao_VT_BizFlow"))
                            AnalyzeNewCommit(branch, repName);
                    }

                }
            }
        }

        private static void AnalyzeNewCommit(Branch branch, string repoName)
        {
            Commit commit = branch.Commits.ElementAtOrDefault(0);
            string link = @"https://tfs.seniorsolution.com.br/Eseg/_git/" + repoName + @$"/commit/{commit.Id}?refName=refs%2Fheads%2F{branch.FriendlyName}";

                var conteudo =
                $"Um novo commit foi registrado\n" +
                $"{commit.Author.Name} | {commit.Author.Email} " +
                $"{commit.Author.When.DateTime} " +
                $"{commit.MessageShort} " +
                $"{branch.FriendlyName} " +
                $"{link}";

            Console.WriteLine("Novo commit encontrado, disparando email");
            Email.EmailOperations.SendNewCommitEmail(conteudo, commit.Author.Name, branch.FriendlyName);
        }

    }
}
