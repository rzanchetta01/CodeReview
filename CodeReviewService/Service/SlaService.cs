using CodeReviewService.Infra.Database.Sla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewService.Service
{
    class SlaService
    {
        private readonly ISlaOperations slaOperations;
        private readonly RepositorioService repositorioService;

        public SlaService(RepositorioService repositorio)
        {
            slaOperations = new SlaOperations();
            repositorioService = repositorio;
        }

        public DateTime GetSlaCommitDate(string repoName)
        {
            try
            {
                if (!repositorioService.RepositorioExist(repoName))
                    throw new Exception("REPOSITORIO NÃO ENCONTRADO -->" + repoName);

                return slaOperations.GetSlaCommitDate(repoName);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR : " + e.Message);
                return DateTime.Now;
            }
        }
    }
}
