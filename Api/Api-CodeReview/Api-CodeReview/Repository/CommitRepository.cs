using Api_CodeReview.Context;
using Api_CodeReview.Models;
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
        }

        public Commit GetById(int id)
        {
            return _context.Commits.Find(id);
        }

        public void Post(Commit commit)
        {
            _context.Commits.Add(commit);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Commit commit)
        {
            _context.Entry(commit).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
