using Api_CodeReview.Context;
using Api_CodeReview.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Service
{
    public class CommitService
    {
        private readonly ICommitRepository repository;

        public CommitService(AppDbContext context)
        {
            repository = new CommitRepository(context);
        }
    }
}
