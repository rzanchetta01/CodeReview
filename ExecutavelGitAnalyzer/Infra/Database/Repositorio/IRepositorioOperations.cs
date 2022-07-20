using System.Collections.Generic;

namespace ExecutavelGitAnalyzer.Infra.Database.Repositorio
{
    interface IRepositorioOperations
    {
        List<Models.CloneConfig> GetRepositoriesData();
        List<string> GetRepositoryBranchs(string repoName);
        bool RepositorioExist(string repoName);
    }
}
