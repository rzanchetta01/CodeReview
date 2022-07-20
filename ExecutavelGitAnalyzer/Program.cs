namespace ExecutavelGitAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Util.Tools.ShutDownConfigurations();
            //Util.Tools.InitalConfig();
            Start();
        }

        static void Start()
        {
            Application.EmailOperations emailOperations = new();

            Service.RepositorioService repositorioService = new();
            Service.BranchService branchService = new(repositorioService);
            Service.CommitService commitService = new(repositorioService, branchService);
            Service.SlaService slaService = new(repositorioService);

            Application.GitOperations gitOperations = new(emailOperations, branchService, commitService,
                slaService, repositorioService);

            gitOperations.ReadAllRepos();
        }
    }
}