using System;

namespace CodeReviewService.Infra.Database.Commit
{
    interface ICommitOperations
    {
        (DateTime, string) GetLastCommitDateAndId(string branchName, string repoName);
        void InsertLastCommit(Models.Commit commit, int idBranch);
        void UpdateLastCommit(Models.Commit commit, string oldIdCommit);
        bool CommitExist(string idCommit);
    }
}
