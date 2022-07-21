using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Service
{
    public class RepositorioService
    {
        private readonly IRepositorioRepository repository;
        private readonly IBranchRepository branchRepository;


        public RepositorioService(AppDbContext context)
        {
            repository = new RepositorioRepository(context);
            branchRepository = new BranchRepository(context);
        }

        public async Task<IEnumerable<Repositorio>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<Repositorio> GetByNome(string nome)
        {
            return await repository.GetByNome(nome);
        }
        public async Task<Repositorio> GetById(int id)
        {
            return await repository.GetById(id);
        }
        public async Task Post(Repositorio repositorio)
        {
            if (repository.RepositoryExist(repositorio.Id_repositorio))
                throw new Exception("Repositorio id ja existe");

            if(!repositorio.Nm_url_clone.Contains(repositorio.Nm_repositorio))
                throw new Exception("Link é de outro repositorio ou nome está errado");

            if (repository.RepositoryExistByName(repositorio.Nm_repositorio))
                throw new Exception("Esse repositorio ja existe");

            if (repository.RepositoryExistByGitUrl(repositorio.Nm_url_clone))
                throw new Exception("Esse repositorio ja existe");

            repositorio.Nm_email_admin = CriptografiaService.Encrypt(repositorio.Nm_email_admin);
            repositorio.Nm_senha = CriptografiaService.Encrypt(repositorio.Nm_senha); 
            repositorio.Nm_usuario = CriptografiaService.Encrypt(repositorio.Nm_usuario); 
            
            await repository.Post(repositorio);
        }
        public async Task Update(Repositorio repositorio, int id)
        {
            if (id != repositorio.Id_repositorio)
                throw new Exception("Id's diferentes");

            if (!repository.RepositoryExist(id))
                throw new Exception("Id não existe");

            repositorio.Nm_email_admin = CriptografiaService.Encrypt(repositorio.Nm_email_admin);
            repositorio.Nm_senha = CriptografiaService.Encrypt(repositorio.Nm_senha);
            repositorio.Nm_usuario = CriptografiaService.Encrypt(repositorio.Nm_usuario);

            await repository.Update(repositorio);
        }
        
        public async Task Delete(int id)
        {
            if (!repository.RepositoryExist(id))
                throw new Exception("Id não existe");

            if (!branchRepository.BranchExistByRepositoryId(id))
                throw new Exception("Delete as branchs registradas a esse repositorio primeiro");

            await repository.Delete(id);
        }
    }
}
