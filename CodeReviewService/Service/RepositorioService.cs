using CodeReviewService.Infra.Database.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewService.Service
{
    class RepositorioService
    {
        private readonly IRepositorioOperations repositorioOperations;

        public RepositorioService()
        {
            repositorioOperations = new RepositorioOperations();
        }

        public List<Models.CloneConfig> GetRepositoriesData()
        {
            return repositorioOperations.GetRepositoriesData();
        }
        public List<string> GetRepositoryBranchs(string repoName)
        {
            try
            {
                if (!repositorioOperations.RepositorioExist(repoName))
                    throw new Exception("ESSE REPOSITORIO NÃO EXISTE ->" + repoName);

                return repositorioOperations.GetRepositoryBranchs(repoName);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR :" + e.Message);
                return new();
            }
        }

        public bool RepositorioExist(string repoName) => repositorioOperations.RepositorioExist(repoName);
    }
}
