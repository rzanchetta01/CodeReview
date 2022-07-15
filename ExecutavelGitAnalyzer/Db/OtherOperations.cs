using ExecutavelGitAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecutavelGitAnalyzer.Db
{
    class OtherOperations
    {
        public static void InsertLastCommit(Commit commit, int idBranch)
        {
            string connString = Util.Criptografia.Decrypt(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            using SqlConnection conn = new(connString);

            SqlParameter pIdCommit = new();
            pIdCommit.ParameterName = "@id_Commit";
            pIdCommit.Value = commit.IdCommit;

            SqlParameter pIdBranch = new();
            pIdBranch.ParameterName = "@id_Branch";
            pIdBranch.Value = idBranch;

            SqlParameter pNm_mensagem = new();
            pNm_mensagem.ParameterName = "@nm_Mensagem";
            pNm_mensagem.Value = commit.Nm_mensagem;

            SqlParameter pNm_autor = new();
            pNm_autor.ParameterName = "@nm_Autor";
            pNm_autor.Value = commit.Nm_autor;

            SqlParameter pDt_commit = new();
            pDt_commit.ParameterName = "@dt_Commit";
            pDt_commit.Value = commit.Dt_commit;

            string cmd = @"INSERT INTO tbCommit VALUES (@id_Commit, @id_Branch, @nm_Mensagem, @nm_Autor, @dt_Commit)";

            using SqlCommand command = new(cmd, conn);
            command.Parameters.Add(pIdCommit);
            command.Parameters.Add(pIdBranch);
            command.Parameters.Add(pNm_mensagem);
            command.Parameters.Add(pNm_autor);
            command.Parameters.Add(pDt_commit);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO AO INSERIR UM NOVO ULTIMO COMMIT\n" + e.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateLastCommit(Commit commit, string oldIdCommit)
        {
            string connString = Util.Criptografia.Decrypt(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
            using SqlConnection conn = new(connString);

            SqlParameter pIdCommit = new();
            pIdCommit.ParameterName = "@id_Commit";
            pIdCommit.Value = commit.IdCommit;

            SqlParameter pNm_mensagem = new();
            pNm_mensagem.ParameterName = "@nm_Mensagem";
            pNm_mensagem.Value = commit.Nm_mensagem;

            SqlParameter pNm_autor = new();
            pNm_autor.ParameterName = "@nm_Autor";
            pNm_autor.Value = commit.Nm_autor;

            SqlParameter pDt_commit = new();
            pDt_commit.ParameterName = "@dt_Commit";
            pDt_commit.Value = commit.Dt_commit;

            SqlParameter pId_Commit_old = new();
            pId_Commit_old.ParameterName = "@id_Commit_old";
            pId_Commit_old.Value = oldIdCommit;

            string cmd = @"UPDATE t
	                        SET
	                        t.Dt_commit = @dt_Commit,
	                        t.Id_Commit = @id_Commit,
	                        t.Nm_autor = @nm_Autor,
	                        t.Nm_mensagem = @nm_Mensagem

	                        FROM tbCommit t
	                        WHERE t.Id_Commit = @id_Commit_old";

            using SqlCommand command = new(cmd, conn);
            command.Parameters.Add(pIdCommit);
            command.Parameters.Add(pNm_mensagem);
            command.Parameters.Add(pNm_autor);
            command.Parameters.Add(pDt_commit);
            command.Parameters.Add(pId_Commit_old);

            try
            {
                conn.Open();
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO AO DAR UPDATE NO ULTIMO COMMIT --> "+oldIdCommit+"\n" + e.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
