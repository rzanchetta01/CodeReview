using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Api_CodeReview.Service;
using LibGit2Sharp;
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
    public class BranchesController : ControllerBase
    {
        private readonly BranchService service;

        public BranchesController(AppDbContext context)
        {
            service = new(context);
        }

        // GET: api/Branches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Branch>>> GetBranchs()
        {
            var branches = await service.GetBranches();
            if (branches != null)
                return Ok(branches);

            return NotFound();
        }

        // GET: api/Branches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Branch>> GetByIdBranch(int id)
        {
            var branch = await service.GetBranch(id);

            if (branch == null)
            {
                return NotFound();
            }

            return Ok(branch);
        }

        // PUT: api/Branches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranch(int id, Models.Branch branch)
        {
            try 
            {
                await service.PutBranch(branch, id);
                return Accepted();
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        // POST: api/Branches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Branch>> PostBranch(Models.Branch branch)
        {
            try
            {
                await service.PostBranch(branch);
                return CreatedAtAction(nameof(GetBranchs), new { id = branch.Id_branch }, branch);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }     
        }
        
        // DELETE: api/Branches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            try
            {
                await service.DeleteBranch(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
