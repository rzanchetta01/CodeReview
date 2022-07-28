using System.Collections.Generic;

namespace CodeReviewService.Infra.Database.Repositorio
{
    interface IRepositorioOperations
    {
        List<Models.CloneConfig> GetRepositoriesData();
        List<string> GetRepositoryBranchs(string repoName);
        bool RepositorioExist(string repoName);
    }
}
