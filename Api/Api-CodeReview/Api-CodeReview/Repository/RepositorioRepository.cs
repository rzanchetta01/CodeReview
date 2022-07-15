using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Microsoft.EntityFrameworkCore;
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


        public async Task<IEnumerable<Repositorio>> GetAll()
        {
            return await _context
                                .Repositorios
                                .AsNoTracking()
                                .ToListAsync();
        }

        public async Task<Repositorio> GetById(int id)
        {
            return await _context.Repositorios.FirstOrDefaultAsync(n => n.Id_repositorio == id);
        }

        public async Task<Repositorio> GetByNome(string nome)
        {
            return await _context.Repositorios.FirstOrDefaultAsync(n => n.Nm_repositorio.Equals(nome));
        }

        public async Task Post(Repositorio repositorio)
        {
            await _context.Repositorios.AddAsync(repositorio);
            await Save();
        }

        public async Task Delete(int id)
        {
            var rep = await _context.Repositorios.FindAsync(id);
            _context.Repositorios.Remove(rep);
            await Save();
        }

        public async Task Update(Repositorio repositorio)
        {
            _context.Entry(repositorio).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await Save();
        }

        private async Task Save()
        {
            await _context.SaveChangesAsync();
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
