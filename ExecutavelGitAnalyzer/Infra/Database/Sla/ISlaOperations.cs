using System;

namespace ExecutavelGitAnalyzer.Infra.Database.Sla
{
    interface ISlaOperations
    {
        DateTime GetSlaCommitDate(string repoName);
        bool SlaExist(string repoName);
    }
}
