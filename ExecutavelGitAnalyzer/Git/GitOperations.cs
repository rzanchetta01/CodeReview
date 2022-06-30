using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutavelGitAnalyzer
{
    class GitOperations
    {
        public static void DownloadRepo(string gitUrl)
        {   
            string cmdCommand = @$"/C cd repos && git clone {gitUrl}";
            Util.Tools.CmdCommand(cmdCommand);
        }

        public static string[] ListRepos()
        {
            //string command = @"/C cd repos && dir";
            //Util.Tools.CmdCommand(command);
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] mockReturn = { @$"{path}\repos\eSeg_Vida_1841" };
            return mockReturn;
        }

        public static void ReadAllRepos()
        {
            foreach (var folder in ListRepos())
            {
                using(var repos = new Repository(folder))
                {
                    foreach (var branch in repos.Branches)
                    {
                        Console.WriteLine(branch.Commits.ElementAt(0).Message);
                    }

                }
            }
        }

        public static void AnalyzeNewCommit()
        {

        }

    }
}
