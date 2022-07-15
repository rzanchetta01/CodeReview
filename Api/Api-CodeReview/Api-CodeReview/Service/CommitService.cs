using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Service
{
    public class CommitService
    {
        private readonly ICommitRepository repository;

        public CommitService(AppDbContext context)
        {
            repository = new CommitRepository(context);
        }

        public void Delete(int id) 
        {
            if (!repository.CommitExist(id))
                throw new Exception("Id não existe");

            repository.Delete(id);
        }
        public Commit GetById(int id)
        {
            if (!repository.CommitExist(id))
                throw new Exception("Id não existe");

            return repository.GetById(id);
        }
        public void Post(Commit commit)
        {
            if (!repository.CommitExistByIdBranch(commit.Id_branch))
                throw new Exception("Ja existe um commit para essa branch");

            repository.Post(commit);
        }
        public void Update(Commit commit, int id)
        {
            if (!repository.CommitExist(id))
                throw new Exception("Id não existe");

            repository.Update(commit);
        }
    }
}
