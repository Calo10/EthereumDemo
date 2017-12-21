using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class DataAccess : IDisposable
    {

        private string ConnectionString { get; set; }
        public MySqlConnection Connection { get; set; }

        public DataAccess()
        {
            ConnectionString = "Server=50.62.209.148; Port=3306; Database=dbEthereumDemo; Uid=adminCMC; Pwd=creativecalo10";

            Connection = new MySqlConnection()
            {
                ConnectionString = ConnectionString
            };
            Connection.Open();
        }

        public MySqlDataReader ExecuteQuery(string stringQuery)
        {
            MySqlDataReader reader = null;
            using (MySqlCommand command = Connection.CreateCommand())
            {
                command.CommandText = stringQuery;
                reader = command.ExecuteReader();
            }

            return reader;
        }

        public void ExecuteNonQuery(string stringQuery)
        {
            using (MySqlCommand command = Connection.CreateCommand())
            {
                command.CommandText = stringQuery;
                command.ExecuteNonQuery();
            }
        }

        public long ExecuteQuery(string stringQuery, List<ParameterSchema> lstParameters)
        {
            long idInsert = 0;
            using (MySqlCommand command = Connection.CreateCommand())
            {
                for (int i = 0; i < lstParameters.Count; i++)
                {
                    command.Parameters.AddWithValue("@" + lstParameters[i].ParamName, lstParameters[i].ParamValue);
                }

                command.CommandText = stringQuery;

                if (command.ExecuteNonQuery() > 0)
                {
                    idInsert = command.LastInsertedId;
                }
            }

            return idInsert;
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }

    public class ParameterSchema
    {
        public string ParamName { get; set; }
        public object ParamValue { get; set; }

        public ParameterSchema() { }

        public ParameterSchema(string ParamName, object ParamValue)
        {
            this.ParamName = ParamName;
            this.ParamValue = ParamValue;
        }


    }
}
