using Api_CodeReview.Context;
using Api_CodeReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository
{
    public class RepositorioRepository : IRepositorioRepository, IDisposable
    {
        private readonly AppDbContext _context;
        private bool disposedValue = false;

        public RepositorioRepository(AppDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Repositorio> GetAll()
        {
            return _context.Repositorios.ToList();
        }

        public Repositorio GetById(int id)
        {
            return _context.Repositorios.FirstOrDefault(n => n.Id_repositorio == id);
        }

        public Repositorio GetByNome(string nome)
        {
            return _context.Repositorios.FirstOrDefault(n => n.Nm_repositorio.Equals(nome));
        }

        public void Post(Repositorio repositorio)
        {
            _context.Repositorios.Add(repositorio);
        }

        public void Delete(int id)
        {
            var rep = _context.Repositorios.Find(id);
            _context.Repositorios.Remove(rep);
        }

        public void Update(Repositorio repositorio)
        {
            _context.Entry(repositorio).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
