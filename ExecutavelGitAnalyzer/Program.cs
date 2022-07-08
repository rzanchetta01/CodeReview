using ExecutavelGitAnalyzer.Email;
using LibGit2Sharp;
using System;
using System.Configuration;
using System.Linq;

namespace ExecutavelGitAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {

            // Util.Tools.ShutDownConfigurations();
            //Util.Tools.InitalConfig();
            GitOperations.ReadAllRepos();

        }

    }
}
