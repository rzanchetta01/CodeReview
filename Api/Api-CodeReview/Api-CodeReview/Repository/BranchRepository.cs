using Api_CodeReview.Context;
using Api_CodeReview.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_CodeReview.Repository
{
    public class BranchRepository : IBranchRepository, IDisposable
    {
        private bool disposedValue = false;
        private readonly AppDbContext _context;

        public BranchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var branch = await _context.Branchs.FindAsync(id);
            var commit = await _context.Commits.FirstOrDefaultAsync(x => x.Id_branch == id);

            if (branch == null)
                throw new Exception("Branch not found");

            if (commit != null)
                throw new Exception("This branch has connection with others tables in the database");
            

            _context.Branchs.Remove(branch);
            await Save();

            return;
            
        }

        public async Task<IEnumerable<Branch>> GetAll()
        {
            return await _context
                                .Branchs
                                .AsNoTracking()
                                .ToListAsync();
        }

        public async Task<Branch> GetById(int id)
        {
            return await _context
                                .Branchs
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id_branch == id);
        }

        public async Task<Branch> GetByNome(string nome)
        {
            return await _context
                                .Branchs
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Nm_branch == nome);
        }

        public async Task Post(Branch branch)
        {
            try
            {
                var repositorio = _context.Repositorios.FirstOrDefault(x => x.Id_repositorio == branch.Id_repositorio);

                string[] branchs = ListarPossiveisBranchs(repositorio);

                foreach (var refBranch in branchs)
                {
                    if (refBranch.Contains(branch.Nm_branch))
                    {
                        await _context.Branchs.AddAsync(branch);
                        await Save();

                        //No momento da criação de uma branch, buscar o último commit e gravar na base de dados junto com a data.
                        return;
                    }
                }
            } catch (Exception) { throw; }
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(Branch branch)
        { 
            _context.Entry(branch).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await Save();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public bool BranchExist(int id)
        {
            return _context.Branchs.Any(e => e.Id_branch == id);
        }

        private string[] ListarPossiveisBranchs(Models.Repositorio repo)
        {
            string urlClone = repo.Nm_url_clone[8..];
            List<string> branchs = new();

            using System.Diagnostics.Process process = new();
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.Arguments = $"git ls-remote 'https://{repo.Nm_usuario}:{repo.Nm_senha}@{urlClone}' 'refs/heads/*'";
            process.Start();
            using System.IO.StreamReader stdOut = process.StandardOutput;
            process.WaitForExit();
            while (!stdOut.EndOfStream)
                branchs.Add(stdOut.ReadLine());

            return branchs.ToArray();
        }
    }
}
