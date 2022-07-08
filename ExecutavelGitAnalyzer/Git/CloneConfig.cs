using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutavelGitAnalyzer.Git
{
    class CloneConfig
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RepoName { get; set; }

        public override string ToString()
        {
            return $"{Url}\n{Username}\n{Password}\n{RepoName}\n";
        }
    }
}