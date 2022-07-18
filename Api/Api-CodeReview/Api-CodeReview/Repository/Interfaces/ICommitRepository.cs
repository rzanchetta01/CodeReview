using Api_CodeReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository.Interfaces
{
    interface ICommitRepository
    {
        Commit GetById(string id);
        Task Post(Commit commit);
        void Update(Commit commit);
        Task Delete(string id);
        Task<bool> CommitExist(string id);
        Task<bool> CommitExistByIdBranch(int idBranch);
        Task<Commit> CommitByIdBranch(int idBranch);
    }
}
