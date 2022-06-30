using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutavelGitAnalyzer.Email
{
    class BaseEmail
    {
        public bool IsHtml { get; set; }
        public string Conteudo { get; set; }
        public string NomeAnexo { get; set; }
    }
}
