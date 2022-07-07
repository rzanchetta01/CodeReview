using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExecutavelGitAnalyzer
{
    class GitOperations
    {

        public static void ReadAllRepos()
        {
            foreach (var folder in ListRepos())
            {
                using var repos = new Repository(folder);
                foreach (var branch in repos.Branches)
                {
                    var repName = folder;
                    repName = repName.Remove(0, Util.Tools.GetReposPath().Length + 1);
                    AnalyzeNewCommits(branch, repName);
                }
            }
        }

        private static string[] ListRepos()
        {

            string[] reposLinks = Db.SelectOperations.GetRepositoriesLinks();
            Console.WriteLine($"BUSCANDO REPOSITORIOS");

            foreach (var item in reposLinks)
            {
                Console.Write(item + "\n");
                DownloadRepo(item);
            }

            string[] repos = Directory.GetDirectories(Util.Tools.GetReposPath());

            return repos;
        }

        private static void AnalyzeNewCommits(Branch branch, string repoName)
        {

            Commit lastCommit = branch.Commits.ElementAtOrDefault(0);
            DateTime lastCommitDate = lastCommit.Author.When.DateTime;
            DateTime dbLastCommitDate = Db.SelectOperations.GetLastCommitDate(branch.FriendlyName, repoName);

            if (lastCommitDate.CompareTo(dbLastCommitDate) > 0)
            {
                SendCommitEmail(branch, repoName, lastCommit);
            }
        }

        private static void SendCommitEmail(Branch branch, string repoName, Commit commit)
        {
            string link = @"https://tfs.seniorsolution.com.br/Eseg/_git/" + repoName + @$"/commit/{commit.Id}?refName=refs%2Fheads%2F{branch.FriendlyName}";

            var conteudo =
            $"Um novo commit foi registrado\n" +
            $"{commit.Author.Name} | {commit.Author.Email} " +
            $"{commit.Author.When.DateTime} \n" +
            $"{commit.MessageShort} |" +
            $"{branch.FriendlyName} " +
            $"{link}";

            Console.WriteLine("Novo commit encontrado, disparando email");
            Console.WriteLine(conteudo);
            Console.WriteLine("\n");
            //Email.EmailOperations.SendNewCommitEmail(conteudo, commit.Author.Name, branch.FriendlyName);
        }

        private static void DownloadRepo(string gitUrl)
        {
            string cmdCommand = @$"/C cd repos && git clone {gitUrl}";
            Util.Tools.CmdCommand(cmdCommand);
        }

    }
}