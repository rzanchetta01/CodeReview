using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutavelGitAnalyzer.Util
{
    class Tools
    {
        public static void InitalConfig()
        {
            GetReposPath(true);   
        }

        public static string GetReposPath(bool isFirstTime)
        {
            if (isFirstTime)
                CreateRepoFolder();


            string path = AppDomain.CurrentDomain.BaseDirectory;
            path += @"\repos";
            return path;

        }

        private static void CreateRepoFolder()
        {
            string cmdCommand = @"/C mkdir repos";
            CmdCommand(cmdCommand);
        }

        public static void CmdCommand(string command)
        {
            using var process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new();
            startInfo.FileName = "CMD.exe";
            startInfo.Arguments = command;

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
    }
}
