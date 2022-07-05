using ExecutavelGitAnalyzer.Email;
using System;
using System.Configuration;

namespace ExecutavelGitAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.Tools.InitalConfig();
            GitOperations.ReadAllRepos();
        }

    }
}
