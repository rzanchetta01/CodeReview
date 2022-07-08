using ExecutavelGitAnalyzer.Git;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExecutavelGitAnalyzer
{
    class GitOperations
    {
        public static void DownloadRepo(string gitUrl)
        {
            string cmdCommand = @$"/C cd repos && git clone {gitUrl}";
            Util.Tools.CmdCommand(cmdCommand);
        }

        public static void ReadAllRepos()
        {

            Console.WriteLine($"ANALISANDO REPOSITORIOS");
            foreach (var folder in ListRepos())
            {
                using var repos = new Repository(folder.Value);

                foreach (var branch in repos.Branches)
                {
                    if (!branch.FriendlyName.EndsWith("HEAD"))
                    {
                        var repName = folder.Value;
                        repName = repName.Remove(0, Util.Tools.GetReposPath().Length + 1);
                        AnalyzeNewCommits(branch, repName, folder.Key);
                        SlaAnalyzer(branch, repName);
                    }
                }
            }
        }

        private static Dictionary<string, string> ListRepos()
        {
            Dictionary<string, string> reposLinks = new();
            CloneConfig[] dbLinks = Db.SelectOperations.GetRepositoriesLinks();
            Console.WriteLine($"BUSCANDO REPOSITORIOS");

            foreach (var item in dbLinks)
            {
                Console.Write(item.RepoName + "\n");

                DownloadRepo(item);
            }

            string[] repos = Directory.GetDirectories(Util.Tools.GetReposPath());

            foreach (var folder in repos)
            {
                var repName = folder;
                repName = repName.Remove(0, Util.Tools.GetReposPath().Length + 1);

                foreach (var repLink in dbLinks)
                {
                    if (repLink.Url.Contains(repName))
                        reposLinks.Add(repLink.Url, folder);
                }
            }

            return reposLinks;
        }

        private static void AnalyzeNewCommits(Branch branch, string repoName, string repoLink)
        {

            Commit lastCommit = branch.Commits.ElementAtOrDefault(0);
            DateTime lastCommitDate = lastCommit.Author.When.DateTime;
            DateTime dbLastCommitDate = Db.SelectOperations.GetLastCommitDate(branch.FriendlyName, repoName);

            if (lastCommitDate.CompareTo(dbLastCommitDate) > 0)
            {
                SendCommitEmail(branch, repoName, lastCommit, repoLink);
            }
        }

        private static void SlaAnalyzer(Branch branch, string repoName)
        {
            Commit lastCommit = branch.Commits.ElementAtOrDefault(0);
            DateTime lastCommitDate = lastCommit.Author.When.DateTime;
            DateTime slaCommitDate = Db.SelectOperations.GetSlaCommitDate(repoName);

            if (lastCommitDate.CompareTo(slaCommitDate) > 0)
            {
                SendSlaEmail(branch, repoName);
            }
        }

        private static void SendCommitEmail(Branch branch, string repoName, Commit commit, string link)
        {
            if (link.EndsWith(".git"))
                link = link.Remove(link.Length - 4, 4);

            string url = @$"{link}" + @$"/commit/{commit.Id}?refName=refs%2Fheads%2F{branch.FriendlyName}";

            var conteudo =
            $"Um novo commit foi registrado\n" +
            $"{commit.Author.Name.Trim()} | {commit.Author.Email.Trim()} " +
            $"{commit.Author.When.DateTime} \n" +
            $"{commit.MessageShort.Trim()} |" +
            $"{branch.FriendlyName.Trim()} " +
            $"{url.Trim()}";

            Console.WriteLine("\nNovo commit encontrado, disparando email");
            Console.WriteLine(conteudo);
            Console.WriteLine("\n");

            var email = Db.SelectOperations.GetBranchEmails(branch.FriendlyName, repoName);

            Email.EmailOperations.SendNewCommitEmail(conteudo, commit.Author.Name, branch.FriendlyName, email.Item2);
        }

        private static void SendSlaEmail(Branch branch, string repoName)
        {
            var conteudo =
                $"Atenção dev, dentro repositório {repoName}, a branch {branch.FriendlyName}\nnão recebe um novo commit dentro do prazo limite";

            Console.WriteLine("Nenhum commit novo encontrado, disparando email");
            Console.WriteLine(conteudo);
            Console.WriteLine("\n");
            Email.EmailOperations.SendSlaEmail(conteudo, "teste", branch.FriendlyName);
        }

        private static void DownloadRepo(CloneConfig config)
        {
            if (config.Url.StartsWith("https"))
                config.Url = config.Url[8..];

            string cloneUrl = $"https://{config.Username}:{config.Password}@{config.Url}";
            string cmdCommand = @$"/C cd repos && git clone {cloneUrl}";

            Util.Tools.CmdCommand(cmdCommand);
        }


    }
}