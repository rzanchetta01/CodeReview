using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewService.Util
{
    class Start
    {
        public static void StartApp(ILogger logger)
        {
            Util.Tools.InitalConfig(logger);
            Application.EmailOperations emailOperations = new();

            Service.RepositorioService repositorioService = new();
            Service.BranchService branchService = new(repositorioService);
            Service.CommitService commitService = new(repositorioService, branchService);
            Service.SlaService slaService = new(repositorioService);

            Application.GitAnalisys gitAnalysis = new(emailOperations, branchService, commitService, slaService);
            Application.GitOperations gitOperations = new(repositorioService, gitAnalysis);

            gitOperations.ReadAllRepos(logger);
        }
    }
}
