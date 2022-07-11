using LibGit2Sharp;
using System;
using System.Linq;

namespace Api_CodeReview.Service
{
    public class ListarRepositorio
    {
        public static void Listar()
        {
            var repos = new Repository();
            var refs = repos.Network.ListReferences("https://github.com/rzanchetta01/ProjetoLetsCode01.git").ToList();
            var result = refs.Where(x => x.CanonicalName.EndsWith("main"));
            foreach (var item in result)
            {
                Console.WriteLine(item.CanonicalName);
            }
        }
    }
}
