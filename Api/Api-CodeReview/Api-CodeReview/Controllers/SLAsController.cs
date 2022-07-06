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
    public class SLAsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SLAsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SLAs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SLA>>> GetSLAS()
        {
            return await _context
                                .SLAS
                                .AsNoTracking()
                                .ToListAsync();
        }

        // GET: api/SLAs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SLA>> GetSLA(int id) 
        {
            var sLA = await _context
                                    .SLAS
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id_SLA == id);

            if (sLA == null)
            {
                return NotFound();
            }

            return sLA;
        }

        // PUT: api/SLAs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSLA(int id, SLA sLA)
        {
            if (id != sLA.Id_SLA)
            {
                return BadRequest();
            }

            _context.Entry(sLA).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SLAExists(id))
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

        // POST: api/SLAs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SLA>> PostSLA(SLA sLA)
        {
            _context.SLAS.Add(sLA);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSLA", new { id = sLA.Id_SLA }, sLA);
        }

        // DELETE: api/SLAs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSLA(int id)
        {
            var sLA = await _context.SLAS.FindAsync(id);
            if (sLA == null)
            {
                return NotFound();
            }

            _context.SLAS.Remove(sLA);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SLAExists(int id)
        {
            return _context.SLAS.Any(e => e.Id_SLA == id);
        }
    }
}
