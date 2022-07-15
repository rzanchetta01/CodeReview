using Api_CodeReview.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository
{
    interface IBranchRepository
    {
        Task<IEnumerable<Branch>> GetAll();
        Task<Branch> GetById(int id);
        Task<Branch> GetByNome(string nome);
        Task Post(Branch branch);
        Task Update(Branch branch);
        Task Delete(int id);
        bool BranchExist(int id);

        bool BranchExistByNome(string nome);
        string[] ListarPossiveisBranchs(Repositorio repositorio);
    }
}
