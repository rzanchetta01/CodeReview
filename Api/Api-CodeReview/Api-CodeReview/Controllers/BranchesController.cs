using Api_CodeReview.Context;
using Api_CodeReview.Models;
using LibGit2Sharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BranchesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Branches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Branch>>> GetBranchs()
        {
            return await _context
                                .Branchs
                                .AsNoTracking()
                                .ToListAsync();
        }

        // GET: api/Branches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Branch>> GetByIdBranch(int id)
        {
            var branch = await _context
                                        .Branchs
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Id_branch == id);

            if (branch == null)
            {
                return NotFound();
            }

            return branch;
        }

        // PUT: api/Branches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranch(int id, Models.Branch branch)
        {
            if (id != branch.Id_branch)
            {
                return BadRequest();
            }

            _context.Entry(branch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchExists(id))
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

        // POST: api/Branches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Branch>> PostBranch(Models.Branch branch)
        {

            var repositorio = _context.Repositorios.FirstOrDefault(x => x.Id_repositorio == branch.Id_repositorio);

            string[] branchs = Service.BranchService.ListarPossiveisBranchs(repositorio);

            foreach (var refBranch in branchs)
            {
                if (refBranch.Contains(branch.Nm_branch))
                {
                    _context.Branchs.Add(branch);
                    await _context.SaveChangesAsync();

                    //No momento da criação de uma branch, buscar o último commit e gravar na base de dados junto com a data.
                    return CreatedAtAction(nameof(GetBranchs), new { id = branch.Id_branch }, branch);

                }
            }


            return BadRequest();
        }







        // DELETE: api/Branches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var branch = await _context.Branchs.FindAsync(id);
            var commit = await _context.Commits.FirstOrDefaultAsync(x => x.Id_branch == id);

            if (branch == null)
            {
                return NotFound();
            }
            else if (commit == null) 
            { 
                _context.Branchs.Remove(branch);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else if (commit.Id_branch == branch.Id_branch)
            {
                return BadRequest("This branch has connection with others tables in the database");
            }
            else
            {
                return NoContent();
            }
        }

        private bool BranchExists(int id)
        {
            return _context.Branchs.Any(e => e.Id_branch == id);
        }
    }
}
