using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutavelGitAnalyzer.Util
{
    class Tools
    {
        public static void InitalConfig()
        {
            CreateRepoFolder();
        }

        public static string GetReposPath()
        {
            //string path = AppDomain.CurrentDomain.BaseDirectory;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path += @"\repos";
            return path;
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

        public static void ShutDownConfigurations()
        {
            Console.WriteLine("Limpando repositórios");
            CleanUpReposFolder(GetReposPath());
            Console.WriteLine("Fim da limpeza");
        }

        private static void CreateRepoFolder()
        {
            Console.WriteLine("Criando temp para armazenar repositórios");
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string cmdCommand = @$"/C cd {path} && mkdir repos";
            
            CmdCommand(cmdCommand);
        }

        private static void CleanUpReposFolder(string rootDir)
        {
            string[] files = Directory.GetFiles(rootDir);
            string[] dirs = Directory.GetDirectories(rootDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                CleanUpReposFolder(dir);
            }

            Directory.Delete(rootDir, false);

        }


    }
}