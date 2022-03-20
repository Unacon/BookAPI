using BookAPI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BookAPI.Controllers
{
    public class ConexaoController : BaseDAO
    {
        public string Executar(string nomeProcedure, Dictionary<string, object> parametros)
        {
            List<SqlError> listaErro = null;

            try
            {
                base.NovoCmmd(nomeProcedure);

                base.connection.FireInfoMessageEventOnUserErrors = true;
                base.connection.InfoMessage += new SqlInfoMessageEventHandler((object sender, SqlInfoMessageEventArgs e) =>
                {
                    if (listaErro == null)
                    {
                        listaErro = new List<SqlError>();
                    }

                    foreach (SqlError item in listaErro)
                    {
                        listaErro.Add(item);
                    }
                });

                Dictionary<string, string> values = new Dictionary<string, string>();

                foreach (KeyValuePair<string, object> item in parametros)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }

                //seta a configuração para disparar um evento, quando acontecer um erro de baixa relevância na procedure
                base.connection.FireInfoMessageEventOnUserErrors = true;

                //função lambda para tratar cada erro disparado pela procedure
                base.connection.InfoMessage += new SqlInfoMessageEventHandler((object sender, SqlInfoMessageEventArgs e) =>
                {
                    //se a lista não estiver instanciada
                    if (listaErro == null)
                    {
                        //instância uma nova lista
                        listaErro = new List<SqlError>();
                    }

                    foreach (SqlError error in e.Errors)
                    {
                        // adiciona os erros na lista
                        listaErro.Add(error);
                    }
                });

                base.AbreConexao();

                cmd.ExecuteNonQuery();

                if (listaErro != null)
                {
                    return listaErro.FirstOrDefault().Message;
                }
                else
                {
                    return null;
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                base.DescartaComando();
                base.FechaConexao();
                base.DescartaConexao();
            }
        }

        public List<T> GetLista<T>(string nomeProcedure, Dictionary<string, object> parametros = null)
        {
            List<T> list = null;
            SqlDataReader dr = null;

            try
            {
                NovoCmmd(nomeProcedure);

                if (parametros != null)
                {
                    foreach (KeyValuePair<string, object> p in parametros)
                    {
                        cmd.Parameters.AddWithValue(p.Key, p.Value);
                    }
                }

                base.AbreConexao();

                dr = cmd.ExecuteReader();

                list = CriaLista<T>(dr);
            }
            catch
            {
                throw;
            }
            finally
            {
                // Liberar memória DataReader
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                }

                base.DescartaComando();
                base.FechaConexao();
                base.DescartaConexao();
            }

            return list;
        }

        public List<T> ExecutarAcaoList<T>(string procedure, Dictionary<string, object> parametros)
        {
            return GetLista<T>(procedure, parametros);
        }
        private List<T> CriaLista<T>(SqlDataReader dr)
        {
            List<T> list = new List<T>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var item = Activator.CreateInstance<T>();
                    foreach (var property in typeof(T).GetProperties())
                    {
                        string nomecoluna;

                        if (property.GetCustomAttribute<ColumnAttribute>() != null)
                        {
                            nomecoluna = property.GetCustomAttribute<ColumnAttribute>().Name;
                        }
                        else
                        {
                            nomecoluna = property.Name;
                        }

                        int i = GetColumnOrdinal(dr, nomecoluna);

                        // se não achar a coluna no datareader, continua o laço
                        if (i < 0) continue;

                        // se for DBNull, continua o laço
                        if (dr[nomecoluna] == DBNull.Value) continue;

                        if (property.PropertyType.IsEnum)
                        {
                            property.SetValue(item, Enum.Parse(property.PropertyType, dr[nomecoluna].ToString()));
                        }
                        else
                        {
                            Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            property.SetValue(item, Convert.ChangeType(dr[nomecoluna], convertTo), null);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }
        private int GetColumnOrdinal(SqlDataReader dr, string columnName)
        {
            int ordinal = -1;

            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (string.Equals(dr.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    ordinal = i;
                    break;
                }
            }

            return ordinal;
        }
    }
}
