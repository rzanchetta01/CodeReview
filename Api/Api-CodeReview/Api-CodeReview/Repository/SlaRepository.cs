using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository
{
    public class SlaRepository : ISlaRepository, IDisposable
    {
        private bool disposedValue = false;
        private readonly AppDbContext _context;

        public SlaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var obj = await _context.SLAS.FirstOrDefaultAsync(n => n.Id_SLA == id);
            _context.Remove(obj);
            await Save();
        }

        public async Task<IEnumerable<SLA>> GetAll()
        {
            return await _context.SLAS
                                .AsNoTracking()
                                .ToListAsync();
        }

        public async Task<SLA> GetById(int id)
        {
            return await _context.SLAS
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(n => n.Id_SLA == id);
        }

        public async Task Post(SLA sla)
        {
            await _context.SLAS.AddAsync(sla);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(SLA sla)
        {
            _context.Entry(sla).State = EntityState.Modified;
            await Save();
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

        public bool SlaExist(int id)
        {
            return _context.SLAS.Any(e => e.Id_SLA == id);
        }
    }
}
