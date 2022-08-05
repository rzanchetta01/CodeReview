using Api_CodeReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository.Interfaces
{
    interface IFeedbackCommitRepository
    {
        void PostFeedback(FeedbackCommit feedbackCommit);
        bool FeedbackExist(string idCommit);
    }
}
