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

            Application.GitOperations gitOperations = new(emailOperations, branchService, commitService,
                slaService, repositorioService);

            gitOperations.ReadAllRepos(logger);
        }
    }
}
