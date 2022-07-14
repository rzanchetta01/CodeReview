using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Service;
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
        private readonly SlaService service;

        public SLAsController(AppDbContext context)
        {
            service = new SlaService(context);
        }

        // GET: api/SLAs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SLA>>> GetSLAS()
        {
            var slas = await service.GetAll();

            return Ok(slas);
        }

        // GET: api/SLAs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SLA>> GetSLA(int id) 
        {
            var sla = await service.GetById(id);

            if (sla == null)
            {
                return NotFound();
            }

            return Ok(sla);
        }

        // PUT: api/SLAs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSLA(int id, SLA sla)
        {
            if (id != sla.Id_SLA)
            {
                return BadRequest();
            }

            await service.Update(sla);
            return Ok();
        }

        // POST: api/SLAs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SLA>> PostSLA(SLA sla)
        {
            await service.Post(sla);
            return CreatedAtAction(nameof(GetSLAS), new { id = sla.Id_SLA }, sla);
        }

        // DELETE: api/SLAs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSLA(int id)
        {
            await service.Delete(id);

            return Ok();
        }
    }
}
