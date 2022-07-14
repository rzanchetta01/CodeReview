using Api_CodeReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository.Interfaces
{
    public interface ISlaRepository
    {
        Task<IEnumerable<SLA>> GetAll();
        Task<SLA> GetById(int id);
        Task Save();
        Task Update(SLA sla);
        Task Delete(int id);
        Task Post(SLA sla);
        bool SlaExist(int id);
    }
}
