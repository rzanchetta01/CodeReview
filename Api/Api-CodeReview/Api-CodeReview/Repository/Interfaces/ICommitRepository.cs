using Api_CodeReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository.Interfaces
{
    interface ICommitRepository
    {
        Commit GetById(int id);
        void Post(Commit commit);
        void Update(Commit commit);
        void Delete(int id);
        bool CommitExist(int id);
        bool CommitExistByIdBranch(int idBranch);
    }
}
