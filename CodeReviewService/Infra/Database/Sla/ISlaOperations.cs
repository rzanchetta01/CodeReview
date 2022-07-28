using System;

namespace CodeReviewService.Infra.Database.Sla
{
    interface ISlaOperations
    {
        DateTime GetSlaCommitDate(string repoName);
        bool SlaExist(string repoName);
    }
}
