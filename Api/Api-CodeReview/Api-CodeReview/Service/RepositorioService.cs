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

        public RepositorioService(AppDbContext context)
        {
            repository = new RepositorioRepository(context);
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
            await repository.Post(repositorio);
        }
        public async Task Update(Repositorio repositorio)
        {
            await repository.Update(repositorio);
        }
        
        public async Task Delete(int id)
        {
            await repository.Delete(id);
        }
    }
}
