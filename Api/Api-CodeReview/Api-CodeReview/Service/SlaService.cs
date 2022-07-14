using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Repository;
using Api_CodeReview.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Service
{
    public class SlaService
    {
        private readonly ISlaRepository repository;

        public SlaService(AppDbContext context)
        {
            repository = new SlaRepository(context);
        }

        public async Task<IEnumerable<SLA>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<SLA> GetById(int id)
        {
            return await repository.GetById(id);
        }

        public async Task Post(SLA sla)
        {
            await repository.Post(sla);
        }

        public async Task Update(SLA sla)
        {
            await repository.Update(sla);
        }

        public async Task Delete(int id)
        {
            await repository.Delete(id);
        }

    }
}
