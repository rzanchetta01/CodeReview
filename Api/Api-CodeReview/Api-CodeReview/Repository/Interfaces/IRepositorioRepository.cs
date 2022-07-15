using Api_CodeReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository
{
    interface IRepositorioRepository
    {
        Task<IEnumerable<Repositorio>> GetAll();
        Task<Repositorio> GetById(int id);
        Task<Repositorio> GetByNome(string nome);
        Task Post(Repositorio repositorio);
        Task Update(Repositorio repositorio);
        Task Delete(int id);
        bool RepositoryExist(int id);
    }
}
