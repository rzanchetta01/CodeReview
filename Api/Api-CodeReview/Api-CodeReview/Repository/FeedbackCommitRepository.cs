using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository
{
    public class FeedbackCommitRepository : IFeedbackCommitRepository
    {
        private readonly AppDbContext _context;

        public FeedbackCommitRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool FeedbackExist(string idCommit)
        {
            try
            {
                if (_context.FeedbackCommits.Any(x => x.Id_Commit == idCommit))
                    return true;

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void PostFeedback(FeedbackCommit feedbackCommit)
        {

            _context.Entry(feedbackCommit).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            _context.FeedbackCommits.Add(feedbackCommit);
            _context.SaveChanges();
        }
    }
}
