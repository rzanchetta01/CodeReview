using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Repository;
using Api_CodeReview.Repository.Interfaces;
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
        private readonly IRepositorioRepository repositorioRepository;
        private readonly ICommitRepository commitRepository;

        public BranchService(AppDbContext context)
        {
            repository = new BranchRepository(context);
            repositorioRepository = new RepositorioRepository(context);
            commitRepository = new CommitRepository(context);
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
                throw new Exception("Id's diferentes");

            var repositorio = await repositorioRepository.GetById(branch.Id_repositorio);
            if (repositorio == null)
                throw new Exception("Repositorio não existe");

            foreach (var item in repository.ListarPossiveisBranchs(repositorio))
            {
                if(item.EndsWith(branch.Nm_branch))
                {
                    branch.Nm_email_dev = CriptografiaService.Encrypt(branch.Nm_email_dev);
                    branch.Nm_email_review = CriptografiaService.Encrypt(branch.Nm_email_review);

                    await repository.Update(branch);
                    return;
                }
            }

            throw new Exception("Erro ao alterar dados da branch");
            
        }
    
        public async Task PostBranch(Models.Branch branch)
        {
            if (repository.BranchExist(branch.Id_branch))
                throw new Exception("Branch id ja existe");

            var repositorio = await repositorioRepository.GetById(branch.Id_repositorio);
            if (repositorio == null)
                throw new Exception("Repositorio não existe");

            foreach (var item in repository.ListarPossiveisBranchs(repositorio))
            {
                if (item.EndsWith(branch.Nm_branch))
                {
                    branch.Nm_email_dev = CriptografiaService.Encrypt(branch.Nm_email_dev);
                    branch.Nm_email_review = CriptografiaService.Encrypt(branch.Nm_email_review);

                    await repository.Post(branch);
                    return;
                }

            }


            throw new Exception("Erro ao alterar dados da branch");
        }

        public async Task DeleteBranch(int id)
        {
            if (!repository.BranchExist(id))
                throw new Exception("id não existe ja existe");

            if (commitRepository.CommitExistByIdBranch(id))
                throw new Exception("essa branch ainda tem ultimo commit registrado");


            await repository.Delete(id);
        }
    }
}
