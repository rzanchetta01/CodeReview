using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository.Interfaces
{
    public class CommitRepository : ICommitRepository, IDisposable
    {
        private bool disposedValue = false;
        private readonly AppDbContext _context;

        public CommitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(string id)
        {
            var com = await _context.Commits.FindAsync(id);
             _context.Commits.Remove(com);
            Save();
        }

        public Commit GetById(string id)
        {
            return _context.Commits.Find(id);
        }

        public async Task Post(Commit commit)
        {
            await _context.Commits.AddAsync(commit);
            Save();
        }

        private void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Commit commit)
        {
            _context.Entry(commit).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            Save();
        }

        public async Task<bool> CommitExist(string id)
        {
            try
            {
                await _context.Commits.FindAsync(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            } 
        }

        public async Task<bool> CommitExistByIdBranch(int id)
        {
            try
            {
                var result = await _context.Commits.AsNoTracking().FirstOrDefaultAsync(n => n.Id_branch == id);
                if (result == null)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Commit> CommitByIdBranch(int idBranch)
        {

            var commit = await _context.Commits.AsNoTracking().FirstOrDefaultAsync(n => n.Id_branch == idBranch);
            return commit;

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
