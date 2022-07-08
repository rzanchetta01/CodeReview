using ExecutavelGitAnalyzer.Git;
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

        public static CloneConfig[] GetRepositoriesLinks()
        {
            List<CloneConfig> result = new();

            string connString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            using SqlConnection conn = new(connString);

            string cmd = @"SELECT Nm_repositorio, Nm_url_clone, Nm_usuario, Nm_senha FROM tbRepositorio";
            using SqlCommand command = new(cmd, conn);

            try
            {
                conn.Open();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    CloneConfig config = new();
                    config.RepoName = (string)reader[0];
                    config.Url = (string)reader[1];
                    config.Username = (string)reader[2];
                    config.Password = (string)reader[3];

                    result.Add(config);
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

        public static DateTime GetSlaCommitDate(string repoName)
        {
            DateTime result = DateTime.Now;

            string connString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            using SqlConnection conn = new(connString);

            SqlParameter pRepoName = new();
            pRepoName.ParameterName = "@repoName";
            pRepoName.Value = repoName;

            string cmd = @"SELECT s.Nr_dias_sla_commit FROM tbSLA s
	                            JOIN tbRepositorio r
                            ON s.id_repositorio = r.Id_repositorio
	                            WHERE r.Nm_repositorio = @repoName";

            using SqlCommand command = new(cmd, conn);
            command.Parameters.Add(pRepoName);

            try
            {
                conn.Open();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int dayLimit = reader.GetInt32(0);
                    result = DateTime.Now.AddDays(-dayLimit);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO AO PEGAR DATA SLA COMMIT DO REPOSITORIO: " + repoName + "\n" + e.Message);
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static (string, string) GetBranchEmails(string nmBranch, string repoName)
        {
            (string, string) result = (null, null);

            string connString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            using SqlConnection conn = new(connString);

            SqlParameter pBranchName = new();
            pBranchName.ParameterName = "@branchName";
            pBranchName.Value = nmBranch;

            SqlParameter pRepoName = new();
            pRepoName.ParameterName = "@repoName";
            pRepoName.Value = repoName;

            string cmd = @"SELECT b.Nm_email_dev, b.Nm_email_review FROM tbBranch b
	                            JOIN tbRepositorio r
                            ON b.Id_repositorio = r.Id_repositorio
                            WHERE b.Nm_branch = @branchName and r.Nm_repositorio = @repoName";
            
            using SqlCommand command = new(cmd, conn);
            command.Parameters.Add(pRepoName);
            command.Parameters.Add(pBranchName);

            try
            {
                conn.Open();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Item1 = (string)reader[0];
                    result.Item2 = (string)reader[1];
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO AO PEGAR EMAILS DO DEV OU RESPONSAVEL PELA REVISÃO\n" + e.Message);
            }
            finally
            {
                conn.Close();
            }


            return result;
        }
    }
}