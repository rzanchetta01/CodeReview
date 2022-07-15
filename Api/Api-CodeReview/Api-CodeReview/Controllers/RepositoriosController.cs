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
    public class RepositoriosController : ControllerBase
    {
        private readonly RepositorioService service;

        public RepositoriosController(AppDbContext context)
        {
            service = new(context);
        }

        // GET: api/Repositorios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Repositorio>>> GetRepositorios()
        {
            try
            {
                var repos = await service.GetAll();
                return Ok(repos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: api/Repositorios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Repositorio>> GetByIdRepositorio(int id)
        {
            try
            {
                var repositorio = await service.GetById(id);
                return Ok(repositorio);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // PUT: api/Repositorios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRepositorio(int id, Repositorio repositorio)
        {
            try
            {
                await service.Update(repositorio, id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: api/Repositorios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Repositorio>> PostRepositorio(Repositorio repositorio)
        {
            try
            {
                await service.Post(repositorio);
                return CreatedAtAction(nameof(GetRepositorios), new { id = repositorio.Id_repositorio }, repositorio);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // DELETE: api/Repositorios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRepositorio(int id)
        {
            try
            {
                await service.Delete(id);
                return Ok();
            }
            catch (Exception e )
            {
                return BadRequest(e.Message);
            }
        }
    }
}
