using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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

        public void Delete(int id)
        {
            var com = _context.Commits.Find(id);
            _context.Commits.Remove(com);
            Save();
        }

        public Commit GetById(int id)
        {
            return _context.Commits.Find(id);
        }

        public void Post(Commit commit)
        {
            _context.Commits.Add(commit);
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

        public bool CommitExist(int id)
        {
            try
            {
                _context.Commits.Find(id);
                return true;
            }
            catch (Exception)
            {
                return false;
            } 
        }

        public bool CommitExistByIdBranch(int id)
        {
            try
            {
                var result = _context.Commits.AsNoTracking().FirstOrDefaultAsync(n => n.Id_branch == id);
                if (result == null)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
