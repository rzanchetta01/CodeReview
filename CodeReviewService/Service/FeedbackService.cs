using CodeReviewService.Infra.Database.Feedback;
using CodeReviewService.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewService.Service
{
    class FeedbackService
    {
        private readonly IFeedbackRepositoryOperations repository;
        public FeedbackService(ILogger logger)
        {
            repository = new FeedbackRepository(logger);
        }
        public void PostInitialBaseFeedback(string IdCommit, int IdBranch)
        {
            try
            {
                if (repository.FeedbackExist(IdCommit))
                    throw new Exception("Ja existe um feedback para esse commit");

                repository.PostFeedback(IdCommit, IdBranch);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR --> " + e.Message);
            }
        }

        public IEnumerable<ReviewSla> GetFeedbackNotReviwed()
        {
            try
            {
                var content = repository.GetAllFeedbacks();

                if (content is null)
                    throw new Exception("Nenhum feedback encontrado");

                content = content.Where(x => x.DtFeedback != DateTime.MinValue).ToList();

                foreach (var item in content)
                {
                    Console.WriteLine(item.ToString());
                }

                return content;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR --> " + e.Message);
                return null;
            }
        }
    }
}
