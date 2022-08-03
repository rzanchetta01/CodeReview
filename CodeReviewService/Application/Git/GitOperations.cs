using CodeReviewService.Models;
using CodeReviewService.Service;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace CodeReviewService.Application
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

        public void ReadAllRepos(ILogger logger)
        {
            Console.WriteLine($"ANALISANDO REPOSITORIOS");
            logger.LogInformation("ANALISANDO REPOSITORIOS");

            foreach (var folder in ListLocalRepos(logger))
            {

                var repName = folder.Value;
                repName = repName.Remove(0, Util.Tools.GetReposPath().Length + 1);

                Console.WriteLine("GIT PULL IN REP --> " + repName);
                logger.LogWarning("GIT PULL IN REP --> " + repName);

                Util.Tools.CmdCommand(@$"/C cd {Util.Tools.GetReposPath()} && cd {repName} && git pull", logger);

                using var repos = new Repository(folder.Value);
                List<string> repoSelectedBranchs = repositorioService.GetRepositoryBranchs(repName);             
                

                foreach (var branch in repos.Branches)
                {

                    string inspectedBranch = branch.FriendlyName;
                    if(inspectedBranch.Contains("origin/"))
                    {
                        inspectedBranch = inspectedBranch.Remove(0, 7);
                        if (!branch.FriendlyName.EndsWith("HEAD") && (repoSelectedBranchs.Contains(inspectedBranch) || repoSelectedBranchs.Contains(inspectedBranch)))
                        {
                            AnalyzeNewCommits(branch, inspectedBranch, repName, folder.Key, logger);
                            SlaAnalyzer(branch, repName, logger);
                        }
                    }

                }

                
            }
        }

        private Dictionary<string, string> ListLocalRepos(ILogger logger)
        {
            Dictionary<string, string> reposLinks = new();
            List<CloneConfig> dbLinks = repositorioService.GetRepositoriesData();
            Console.WriteLine($"BUSCANDO REPOSITORIOS");
            logger.LogWarning("BUSCANDO REPOSITORIOS");

            foreach (var item in dbLinks)
            {
                Console.Write(item.RepoName + "\n");
                logger.LogWarning(item.ToString());
                DownloadRepo(item, logger);
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

        private void AnalyzeNewCommits(Branch branch,string branchName, string repoName, string repoLink, ILogger logger)
        {

            LibGit2Sharp.Commit lastCommit = branch.Commits.ElementAtOrDefault(0);
            DateTime lastCommitDate = lastCommit.Author.When.DateTime;

            (DateTime, string) dbLastCommitDateAndId = commitService.GetLastCommitDateAndId(branchName, repoName);
            int idBranch = branchService.GetBranchId(branchName, repoName);

            if (dbLastCommitDateAndId.Item2 == null && idBranch != -1 )
                commitService.InsertLastCommit(new Models.Commit(lastCommit.Id.ToString(), lastCommit.Message, lastCommit.Author.Name, lastCommitDate), idBranch);


            if (lastCommitDate.CompareTo(dbLastCommitDateAndId.Item1) > 0)
            {
                SendCommitEmail(branch, repoName, lastCommit, repoLink, logger);
                commitService.UpdateLastCommit(new Models.Commit(lastCommit.Id.ToString(), lastCommit.Message, lastCommit.Author.Name, lastCommitDate), dbLastCommitDateAndId.Item2);
            }
        }

        private void SlaAnalyzer(Branch branch, string repoName, ILogger logger)
        {
            LibGit2Sharp.Commit lastCommit = branch.Commits.ElementAtOrDefault(0);
            DateTime lastCommitDate = lastCommit.Author.When.DateTime;
            DateTime slaCommitDate = slaService.GetSlaCommitDate(repoName);

            if (lastCommitDate.CompareTo(slaCommitDate) > 0)
            {
                SendSlaEmail(branch, repoName, logger);
            }
        }

        private void SendCommitEmail(Branch branch, string repoName, LibGit2Sharp.Commit newCommit, string link, ILogger logger)
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
            emailOperations.SendNewCommitEmail(conteudo, newCommit.Author.Name, branch.FriendlyName, email.Item2, logger);
        }

        private void SendSlaEmail(Branch branch, string repoName, ILogger logger)
        {
            var conteudo =
                $"Atenção dev, dentro repositório {repoName}, a branch {branch.FriendlyName}\nnão recebe um novo commit dentro do prazo limite";

            Console.WriteLine("Nenhum commit novo encontrado, disparando email");
            Console.WriteLine(conteudo);
            Console.WriteLine("\n");


            var email = branchService.GetBranchEmailsAdress(branch.FriendlyName, repoName);
            emailOperations.SendSlaEmail(conteudo, email, branch.FriendlyName, logger);
        }

        private void DownloadRepo(Models.CloneConfig config, ILogger logger)
        {
            if (config.Url.StartsWith("https"))
                config.Url = config.Url[8..];

            config.Password = Service.CriptografiaService.Decrypt(config.Password);
            config.Username = Service.CriptografiaService.Decrypt(config.Username);

            string cloneUrl = $"https://{config.Username}:{config.Password}@{config.Url}";
            
            string cmdCommand = @$"/C cd {Util.Tools.GetReposPath()} && git clone {cloneUrl}";

            Util.Tools.CmdCommand(cmdCommand, logger);
        }
    }
}