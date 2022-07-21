using ExecutavelGitAnalyzer.Models;
using ExecutavelGitAnalyzer.Service;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExecutavelGitAnalyzer.Application
{
    class GitOperations
    {
        private readonly EmailOperations emailOperations;
        private readonly BranchService branchService;
        private readonly CommitService commitService;
        private readonly SlaService slaService;
        private readonly RepositorioService repositorioService;

        public GitOperations(EmailOperations emailOperations, BranchService branchService,
            CommitService commitService, SlaService slaService, RepositorioService repositorioService)
        {
            this.emailOperations = emailOperations;
            this.branchService = branchService;
            this.commitService = commitService;
            this.slaService = slaService;
            this.repositorioService = repositorioService;
        }

        public void ReadAllRepos()
        {

            Console.WriteLine($"ANALISANDO REPOSITORIOS");
            foreach (var folder in ListLocalRepos())
            {
                using var repos = new Repository(folder.Value);

                var repName = folder.Value;
                repName = repName.Remove(0, Util.Tools.GetReposPath().Length + 1);
                List<string> repoSelectedBranchs = repositorioService.GetRepositoryBranchs(repName);             

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

        private Dictionary<string, string> ListLocalRepos()
        {
            Dictionary<string, string> reposLinks = new();
            List<CloneConfig> dbLinks = repositorioService.GetRepositoriesData();
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

        private void AnalyzeNewCommits(Branch branch, string repoName, string repoLink)
        {

            LibGit2Sharp.Commit lastCommit = branch.Commits.ElementAtOrDefault(0);
            DateTime lastCommitDate = lastCommit.Author.When.DateTime;
            (DateTime, string) dbLastCommitDateAndId = commitService.GetLastCommitDateAndId(branch.FriendlyName, repoName);
            int idBranch = branchService.GetBranchId(branch.FriendlyName, repoName);

            if (dbLastCommitDateAndId.Item2 == null && idBranch != -1 )
                commitService.InsertLastCommit(new Models.Commit(lastCommit.Id.ToString(), lastCommit.Message, lastCommit.Author.Name, lastCommitDate), idBranch);


            if (lastCommitDate.CompareTo(dbLastCommitDateAndId.Item1) > 0)
            {
                SendCommitEmail(branch, repoName, lastCommit, repoLink);
                commitService.UpdateLastCommit(new Models.Commit(lastCommit.Id.ToString(), lastCommit.Message, lastCommit.Author.Name, lastCommitDate), dbLastCommitDateAndId.Item2);
            }
        }

        private void SlaAnalyzer(Branch branch, string repoName)
        {
            LibGit2Sharp.Commit lastCommit = branch.Commits.ElementAtOrDefault(0);
            DateTime lastCommitDate = lastCommit.Author.When.DateTime;
            DateTime slaCommitDate = slaService.GetSlaCommitDate(repoName);

            if (lastCommitDate.CompareTo(slaCommitDate) > 0)
            {
                SendSlaEmail(branch, repoName);
            }
        }

        private void SendCommitEmail(Branch branch, string repoName, LibGit2Sharp.Commit newCommit, string link)
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

            var email = branchService.GetBranchEmailsAdress(branch.FriendlyName, repoName);
            emailOperations.SendNewCommitEmail(conteudo, newCommit.Author.Name, branch.FriendlyName, email.Item2);
        }

        private void SendSlaEmail(Branch branch, string repoName)
        {
            var conteudo =
                $"Atenção dev, dentro repositório {repoName}, a branch {branch.FriendlyName}\nnão recebe um novo commit dentro do prazo limite";

            Console.WriteLine("Nenhum commit novo encontrado, disparando email");
            Console.WriteLine(conteudo);
            Console.WriteLine("\n");


            var email = branchService.GetBranchEmailsAdress(branch.FriendlyName, repoName);

            emailOperations.SendSlaEmail(conteudo, email, branch.FriendlyName);
        }

        private void DownloadRepo(Models.CloneConfig config)
        {
            if (config.Url.StartsWith("https"))
                config.Url = config.Url[8..];

            config.Password = Service.CriptografiaService.Decrypt(config.Password);
            config.Username = Service.CriptografiaService.Decrypt(config.Username);

            string cloneUrl = $"https://{config.Username}:{config.Password}@{config.Url}";
            string cmdCommand = @$"/C cd repos && git clone {cloneUrl}";

            Util.Tools.CmdCommand(cmdCommand);
        }
    }
}