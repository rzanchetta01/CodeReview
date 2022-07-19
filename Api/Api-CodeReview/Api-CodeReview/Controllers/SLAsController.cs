using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
            try
            {
                var slas = await service.GetAll();
                return Ok(slas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // GET: api/SLAs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SLA>> GetSLA(int id) 
        {
            try
            {
                var sla = await service.GetById(id);
                return Ok(sla);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/SLAs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSLA(int id, SLA sla)
        {
            try
            {
                await service.Update(sla, id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/SLAs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SLA>> PostSLA(SLA sla)
        {
            try
            {
                await service.Post(sla);
                return CreatedAtAction(nameof(GetSLAS), new { id = sla.Id_SLA }, sla);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE: api/SLAs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSLA(int id)
        {
            try
            {
                await service.Delete(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
