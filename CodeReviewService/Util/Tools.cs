using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewService.Util
{
    class Tools
    {
        public static void InitalConfig(ILogger logger)
        {
            CreateRepoFolder(logger);
        }

        public static string GetReposPath()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            path += @"\repos";
            return path;
        }

        public static void CmdCommand(string command, ILogger logger)
        {
            List<string> output = new();
            using var process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
            process.StartInfo = startInfo;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            using System.IO.StreamReader stdOut = process.StandardOutput;
            process.WaitForExit();
            while (!stdOut.EndOfStream)
                output.Add(stdOut.ReadLine());

            string result = "";
            foreach (var item in output)
            {
                result += item;
            }
            logger.LogWarning("CMD COMMAND RESULT {Output}\nFROM COMMAND {Command} ",result, command);
        }

        public static void ShutDownConfigurations()
        {
            Console.WriteLine("Limpando repositórios");
            CleanUpReposFolder(GetReposPath());
            Console.WriteLine("Fim da limpeza");
        }

        private static void CreateRepoFolder(ILogger logger)
        {
            Console.WriteLine("Criando temp para armazenar repositórios");
            logger.LogWarning("Criando temp para armazenar repositórios");

            string path = AppDomain.CurrentDomain.BaseDirectory;
            string cmdCommand = @$"/C cd {path} && mkdir repos";
            
            CmdCommand(cmdCommand, logger);
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