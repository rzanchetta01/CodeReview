using Api_CodeReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository
{
    interface IRepositorioRepository
    {
        IEnumerable<Repositorio> GetAll();
        Repositorio GetById(int id);
        Repositorio GetByNome(string nome);
        void Post(Repositorio repositorio);
        void Update(Repositorio repositorio);
        void Delete(int id);
        void Save();
    }
}
