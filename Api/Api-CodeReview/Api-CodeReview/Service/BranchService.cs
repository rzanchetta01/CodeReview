using Api_CodeReview.Models;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Api_CodeReview.Service
{
    public class BranchService
    {
        public static string[] ListarPossiveisBranchs(Repositorio repo)
        {
            string urlClone = repo.Nm_url_clone[8..];
            List<string> branchs = new();

            Process process = new();
            process.StartInfo.FileName = "powershell.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.Arguments = $"git ls-remote 'https://{repo.Nm_usuario}:{repo.Nm_senha}@{urlClone}' 'refs/heads/*'";
            process.Start();
            StreamReader stdOut = process.StandardOutput;
            process.WaitForExit();
            while (!stdOut.EndOfStream)
                branchs.Add(stdOut.ReadLine());


            return branchs.ToArray();
        }
    }
}
