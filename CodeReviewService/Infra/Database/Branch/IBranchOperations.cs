using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewService.Infra.Database.Branch
{
    interface IBranchOperations
    {
        int GetBranchId(string nmBranch, string repoName);
        (string,string) GetBranchEmailsAdress(string nmBranch, string repoName);
        bool BranchExist(string nmBranch);

        bool BranchExist(int idBranch);
    }
}
