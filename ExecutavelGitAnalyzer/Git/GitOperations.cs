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

                var repName = folder.Value;
                repName = repName.Remove(0, Util.Tools.GetReposPath().Length + 1);
                string[] repoSelectedBranchs = Db.SelectOperations.GetRepositoryBranchs(repName);             

                foreach (var branch in repos.Branches)
                {
                    if (!branch.FriendlyName.EndsWith("HEAD") && repoSelectedBranchs.Contains(branch.FriendlyName))
                    {
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
                    {
                        try { reposLinks.Add(repLink.Url, folder); }
                        catch (Exception) { continue;  }

                    }
                }
            }

            return reposLinks;
        }

        private static void AnalyzeNewCommits(Branch branch, string repoName, string repoLink)
        {

            Commit lastCommit = branch.Commits.ElementAtOrDefault(0);
            DateTime lastCommitDate = lastCommit.Author.When.DateTime;
            (DateTime, string) dbLastCommitDateAndId = Db.SelectOperations.GetLastCommitDateAndId(branch.FriendlyName, repoName);
            int idBranch = Db.SelectOperations.GetBranchId(branch.FriendlyName, repoName);

            if (dbLastCommitDateAndId.Item2 == null)
                Db.OtherOperations.InsertLastCommit(new Models.Commit(lastCommit.Id.ToString(), lastCommit.Message, lastCommit.Author.Name, lastCommitDate), idBranch);


            if (lastCommitDate.CompareTo(dbLastCommitDateAndId.Item1) > 0)
            {
                SendCommitEmail(branch, repoName, lastCommit, repoLink);
                Db.OtherOperations.UpdateLastCommit(new Models.Commit(lastCommit.Id.ToString(), lastCommit.Message, lastCommit.Author.Name, lastCommitDate), dbLastCommitDateAndId.Item2);
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

        private static void SendCommitEmail(Branch branch, string repoName, Commit newCommit, string link)
        {
            if (link.EndsWith(".git"))
                link = link.Remove(link.Length - 4, 4);

            string url = @$"{link}" + @$"/commit/{newCommit.Id}?refName=refs%2Fheads%2F{branch.FriendlyName}";

            var conteudo =
            $"Um novo commit foi registrado\n" +
            $"{newCommit.Author.Name.Trim()} | {newCommit.Author.Email.Trim()} " +
            $"{newCommit.Author.When.DateTime} \n" +
            $"{newCommit.MessageShort.Trim()} |" +
            $"{branch.FriendlyName.Trim()} " +
            $"{url.Trim()}";

            Console.WriteLine("\nNovo commit encontrado, disparando email");
            Console.WriteLine(conteudo);
            Console.WriteLine("\n");

            var email = Db.SelectOperations.GetBranchEmails(branch.FriendlyName, repoName);
            Email.EmailOperations.SendNewCommitEmail(conteudo, newCommit.Author.Name, branch.FriendlyName, email.Item2);
        }

        private static void SendSlaEmail(Branch branch, string repoName)
        {
            var conteudo =
                $"Atenção dev, dentro repositório {repoName}, a branch {branch.FriendlyName}\nnão recebe um novo commit dentro do prazo limite";

            Console.WriteLine("Nenhum commit novo encontrado, disparando email");
            Console.WriteLine(conteudo);
            Console.WriteLine("\n");


            var email = Db.SelectOperations.GetBranchEmails(branch.FriendlyName, repoName);

            Email.EmailOperations.SendSlaEmail(conteudo, email, branch.FriendlyName);
        }

        private static void DownloadRepo(CloneConfig config)
        {
            if (config.Url.StartsWith("https"))
                config.Url = config.Url[8..];

            config.Password = Util.Criptografia.Decrypt(config.Password);
            config.Username = Util.Criptografia.Decrypt(config.Username);

            string cloneUrl = $"https://{config.Username}:{config.Password}@{config.Url}";
            string cmdCommand = @$"/C cd repos && git clone {cloneUrl}";

            Util.Tools.CmdCommand(cmdCommand);
        }


    }
}