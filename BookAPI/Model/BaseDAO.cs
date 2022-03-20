using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BookAPI.Model
{
    public class BaseDAO
    {
        // Objeto de conexão com o banco de dados
        public SqlConnection connection = null;
        
        //Comando para utilização por quem herdar
        public SqlCommand cmd = null;

        public IConfiguration Configuration { get; }

        private void SetConnection()
        {
            connection = new SqlConnection("Server = JOAO\\SQLEXPRESS; Database = Biblioteca; Trusted_Connection = True;");
        }

        public BaseDAO()
        {
            SetConnection();
        }

        public void NovoCmmd(string nomeProcedure)
        {
            if (!string.IsNullOrEmpty(nomeProcedure) && nomeProcedure.IndexOf("dbo.") != 0)
            {
                nomeProcedure = "dbo." + nomeProcedure;
            }

            cmd = new SqlCommand(nomeProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 60
            };
        }

        public void AbreConexao()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void FechaConexao()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public void DescartaComando()
        {
            cmd.Dispose();
        }

        public void DescartaConexao()
        {
            connection.Dispose();
        }



    }
}
