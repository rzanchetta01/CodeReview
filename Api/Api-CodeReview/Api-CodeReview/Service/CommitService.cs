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

        public async void Delete(string id) 
        {
            if (!await repository.CommitExist(id))
                throw new Exception("Id não existe");

            await repository .Delete(id);
        }
        public async Task<Commit> GetById(string id)
        {
            if (!await repository.CommitExist(id))
                throw new Exception("Id não existe");

            return repository.GetById(id);
        }
        public async void Post(Commit commit)
        {
            if (!await repository .CommitExistByIdBranch(commit.Id_branch))
                throw new Exception("Ja existe um commit para essa branch");

            await repository.Post(commit);
        }
        public async void Update(Commit commit, string id)
        {
            if (!await repository.CommitExist(id))
                throw new Exception("Id não existe");

            repository.Update(commit);
        }
    }
}
