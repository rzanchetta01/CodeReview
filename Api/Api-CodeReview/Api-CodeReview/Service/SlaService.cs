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
        private readonly IRepositorioRepository repositorioRepository;

        public SlaService(AppDbContext context)
        {
            repository = new SlaRepository(context);
            repositorioRepository = new RepositorioRepository(context);
        }

        public async Task<IEnumerable<SLA>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<SLA> GetById(int id)
        {
            if (!repository.SlaExist(id))
                throw new Exception("Id não existe");

            return await repository.GetById(id);
        }

        public async Task Post(SLA sla)
        {
            if (!repositorioRepository.RepositoryExist(sla.Id_repositorio))
                throw new Exception("Repositorio id não existe");

            if (sla.Nr_dias_sla_commit <= 0)
                throw new Exception("Nr_dias_sla_commit não pode ser 0 ou menor");

            if (sla.Nr_dias_sla_review <= 0)
                throw new Exception("Nr_dias_sla_review não pode ser 0 ou menor");

            await repository.Post(sla);
        }

        public async Task Update(SLA sla, int id)
        {
            if (id != sla.Id_SLA)
                throw new Exception("Id's são diferentes");

            if (!repositorioRepository.RepositoryExist(sla.Id_repositorio))
                throw new Exception("Repositorio id não existe");

            if (sla.Nr_dias_sla_commit <= 0)
                throw new Exception("Nr_dias_sla_commit não pode ser 0 ou menor");

            if (sla.Nr_dias_sla_review <= 0)
                throw new Exception("Nr_dias_sla_review não pode ser 0 ou menor");

            await repository.Update(sla);
        }

        public async Task Delete(int id)
        {
            if (!repository.SlaExist(id))
                throw new Exception("Repositorio id não existe");

            await repository.Delete(id);
        }
    }
}
