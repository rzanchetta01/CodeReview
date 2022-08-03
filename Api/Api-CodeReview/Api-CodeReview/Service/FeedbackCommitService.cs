using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Service
{
    public class FeedbackCommitService
    {
        private readonly FeedbackCommitRepository repository;

        public FeedbackCommitService(AppDbContext context)
        {
            repository = new(context);
        }

        public void PostFeedback(string idCommit, string feedback)
        {
            FeedbackCommit feedbackCommit = new();
            feedbackCommit.Id_Commit = idCommit;
            feedbackCommit.Status_resposta = "feedback enviado";
            feedbackCommit.Feedback = feedback;
            feedbackCommit.Id = 0;

            repository.PostFeedback(feedbackCommit);
        }
        public bool FeedbackExist(string idCommit)
        {
            return repository.FeedbackExist(idCommit);
        }
    }
}
