using Api_CodeReview.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Api_CodeReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositorioController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public RepositorioController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetRepositorio()
        {
            return Ok(new
            {
                sucess = true,
                data = await _appDbContext.Repositorios.ToListAsync()
            });    
        }
    }
}
