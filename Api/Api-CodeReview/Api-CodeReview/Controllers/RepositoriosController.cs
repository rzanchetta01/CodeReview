using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepositoriosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RepositoriosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Repositorios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Repositorio>>> GetRepositorios()
        {
            return await _context
                                .Repositorios
                                .AsNoTracking()
                                .ToListAsync();
        }

        // GET: api/Repositorios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Repositorio>> GetByIdRepositorio(int id)
        {
            var repositorio = await _context
                                            .Repositorios
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(x => x.Id_repositorio == id);

            if (repositorio == null)
            {
                return NotFound();
            }

            return repositorio;
        }

        // PUT: api/Repositorios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepositorio(int id, Repositorio repositorio)
        {
            if (id != repositorio.Id_repositorio)
            {
                return BadRequest();
            }

            _context.Entry(repositorio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepositorioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Repositorios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Repositorio>> PostRepositorio(Repositorio repositorio)
        {
            _context.Repositorios.Add(repositorio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRepositorios), new { id = repositorio.Id_repositorio}, repositorio);
        }

        // DELETE: api/Repositorios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepositorio(int id)
        {
            var repositorio = await _context.Repositorios.FindAsync(id);
            var branch = await _context.Branchs.FirstOrDefaultAsync(x => x.Id_repositorio == id);

            if (repositorio == null)
            {
                return NotFound();
            } 
            else if (branch == null)
            { 
                _context.Repositorios.Remove(repositorio);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else if (branch.Id_repositorio == repositorio.Id_repositorio)
            {
                return BadRequest("This repository has connection with others tables in the database");
            } 
            else
            {
                return NoContent();
            }
        }

        private bool RepositorioExists(int id)
        {
            return _context.Repositorios.Any(e => e.Id_repositorio == id);
        }
    }
}
