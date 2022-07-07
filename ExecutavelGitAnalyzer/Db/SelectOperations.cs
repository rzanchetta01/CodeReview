using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ExecutavelGitAnalyzer.Db
{
    class SelectOperations
    {
        public static DateTime GetLastCommitDate(string branchName, string repoName)
        {
            DateTime result = DateTime.Now;

            string connString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            using SqlConnection conn = new(connString);

            SqlParameter pBranchName = new();
            pBranchName.ParameterName = "@branchName";
            pBranchName.Value = branchName;

            SqlParameter pRepoName = new();
            pRepoName.ParameterName = "@repoName";
            pRepoName.Value = repoName;

            string cmd = @"SELECT c.Dt_commit FROM tbCommit c
	                        JOIN tbBranch b
                        ON b.Id_branch = c.Id_branch
                            join tbRepositorio r
                        ON r.Id_repositorio = b.Id_repositorio
	                        WHERE b.Nm_branch = @branchName and r.Nm_repositorio = @repoName";

            using SqlCommand command = new(cmd, conn);
            command.Parameters.Add(pBranchName);
            command.Parameters.Add(pRepoName);

            try
            {
                conn.Open();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result = reader.GetDateTime(0);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO AO PEGAR DATA ULTIMO COMMIT DA BRANCH: " + branchName + "\n" + e.Message);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static string[] GetRepositoriesLinks()
        {
            List<String> result = new();

            string connString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            using SqlConnection conn = new(connString);

            string cmd = @"SELECT Nm_url_clone FROM tbRepositorio";
            using SqlCommand command = new(cmd, conn);

            try
            {
                conn.Open();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(reader.GetString(0));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO AO PEGAR REPOSITORIOS\n" + e.Message);
            }
            finally
            {
                conn.Close();
            }

            return result.ToArray();
        }

    }
}