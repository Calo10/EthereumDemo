using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class Connection
    {
        MySqlConnection connection = new MySqlConnection();
        string ConecctionString;

        public MySqlDataReader getExecuteQuery(string stringQuery)
        {
            try
            {
                closeConection();
                openConection();

                MySqlCommand query = connection.CreateCommand();
                query.CommandText = stringQuery;

                MySqlDataReader reader = query.ExecuteReader();


                return reader;
            }
            catch (MySqlException ex)
            {
                closeConection();
                throw ex;
            }

        }

        public string setExecuteQuery(string stringQuery, List<ParameterSchema> lstParameters)
        {
            try
            {
                openConection();
                string ans = string.Empty;

                MySqlCommand command = connection.CreateCommand();

                for (int i = 0; i < lstParameters.Count; i++)
                {
                    command.Parameters.AddWithValue("@" + lstParameters[i].ParamName, lstParameters[i].ParamValue);
                }

                command.CommandText = stringQuery;

                if (command.ExecuteNonQuery() > 0)
                {
                    ans = command.LastInsertedId.ToString() == "0" ? "" : "ID:" + command.LastInsertedId.ToString();
                }
                else
                {
                    ans = "Error de ejecucion en el Servidor de Base de Datos";
                }

                closeConection();

                return ans;

            }
            catch (MySqlException ex)
            {
                closeConection();
                return ex.Message;
            }

        }

        private void openConection()
        {
            try
            {
                connection.Close();
                ConecctionString = "Server=50.62.209.148; Port=3306; Database=dbCMC; Uid=adminCMC; Pwd=creativecalo10;";
                connection.ConnectionString = ConecctionString;
                connection.Open();
            }
            catch (Exception err)
            {
                throw;
            }

        }

        private void closeConection()
        {
            connection.Close();
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
