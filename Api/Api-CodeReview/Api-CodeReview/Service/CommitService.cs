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
            repository.Delete(id);
        }
        public Commit GetById(int id)
        {
            return repository.GetById(id);
        }
        public void Post(Commit commit)
        {
            repository.Post(commit);
        }
        public void Update(Commit commit, int id)
        {
            repository.Update(commit);
        }
    }
}
