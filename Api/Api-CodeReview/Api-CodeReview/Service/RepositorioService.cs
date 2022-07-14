using Api_CodeReview.Context;
using Api_CodeReview.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Service
{
    public class RepositorioService
    {
        private readonly IRepositorioRepository repository;

        public RepositorioService(AppDbContext context)
        {
            repository = new RepositorioRepository(context);
        }
    }
}
