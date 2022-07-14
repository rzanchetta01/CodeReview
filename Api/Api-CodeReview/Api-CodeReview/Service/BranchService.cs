using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Repository;
using LibGit2Sharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Service
{
    public class BranchService
    {
        private readonly IBranchRepository repository;

        public BranchService(AppDbContext context)
        {
            repository = new BranchRepository(context);
        }
    
        public async Task<IEnumerable<Models.Branch>> GetBranches()
        {
            return await repository.GetAll();
        }

        public async Task<Models.Branch> GetBranch(int id)
        {
            return await repository.GetById(id);
        }

        public async Task<Models.Branch> GetBranch(string nome)
        {
            return await repository.GetByNome(nome);
        }

        public async Task PutBranch(Models.Branch branch, int id)
        {
            if (id != branch.Id_branch)
            {
                return;
            }

            try
            {
                await repository.Update(branch);
                await repository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!repository.BranchExist(id))
                {
                    return;
                }
                else
                {
                    throw;
                }
            }
        }
    
        public async Task PostBranch(Models.Branch branch)
        {
            await repository.Post(branch);
        }

        public async Task DeleteBranch(int id)
        {
            try
            {
                await repository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
