using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class UserConnection
    {
        Connection conexionM = new Connection();

        public bool UpdateUser(UserModel user)
        {
            bool isValid = true;

            try
            {

              

                List<ParameterSchema> lstParams = new List<ParameterSchema>();
                string query = string.Empty;

                lstParams.Add(new ParameterSchema("Email", user.Email));
                lstParams.Add(new ParameterSchema("UserName", user.UserName));
                lstParams.Add(new ParameterSchema("UserLastName", user.UserLastName));
                lstParams.Add(new ParameterSchema("IDUser", user.IDUser));
                lstParams.Add(new ParameterSchema("Password", user.Password));
                lstParams.Add(new ParameterSchema("Profile", user.Profile));


                if (!string.IsNullOrEmpty(user.Email))
                {
                    //Inserta Nuevo Curso
                    query = "UPDATE dbEthereumDemo.User SET" +
                    "UserName = @UserName,UserLastName = @UserLastName,Password = @Password,Profile = @Profile" +
                    "WHERE Email = @Email ";


                }

                return conexionM.setExecuteQuery(query, lstParams).Contains("ID");

            }
            catch (Exception err)
            {

                isValid = false;
            }
            return isValid;
        }
        public bool InserUser(UserModel user)
        {
            bool isValid = true;

            try
            {

                List<ParameterSchema> lstParams = new List<ParameterSchema>();
                string query = string.Empty;

                lstParams.Add(new ParameterSchema("Email", user.Email));
                lstParams.Add(new ParameterSchema("UserName", user.UserName));
                lstParams.Add(new ParameterSchema("UserLastName", user.UserLastName));
                lstParams.Add(new ParameterSchema("IDUser", user.IDUser));
                lstParams.Add(new ParameterSchema("Password", user.Password));
                lstParams.Add(new ParameterSchema("Profile", user.Profile));


                if (!string.IsNullOrEmpty(user.Email))
                {
                    //Inserta Nuevo Curso
                    query = "INSERT INTO dbEthereumDemo.User(Email,UserName,UserLastName,Password,perfil)"+
                        "VALUES(@Email, @UserName, @UserLastName, @Password, @Profile); ";


                }
              


                return conexionM.setExecuteQuery(query, lstParams).Contains("ID");

            }
            catch (Exception err)
            {

                isValid = false;
            }
            return isValid;
        }
        public UserModel GetUserByPassword(string userID, string pass)
        {
            try
            {
                string query = "SELECT User.EthereumContractUser,User.UserName,User.UserLastName,User.IDUser,User.Password FROM User Where User.IDUser='" + userID + "' AND User.Password='" + pass + "'";

                UserModel user = new UserModel();
                MySqlDataReader reader = conexionM.getExecuteQuery(query);

                while (reader.Read())
                {
                    user.Email = reader["EthereumContractUser"].ToString();
                    user.IDUser = reader["IDUser"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.UserLastName = reader["UserLastName"].ToString();
                    user.UserName = reader["UserName"].ToString();


                }

                return user;

            }
            catch (Exception err)
            {

                throw err;
            }
        }

        public UserModel SearchUser(string userID)
        {
            try
            {
                string query = "SELECT User.EthereumContractUser,User.UserName,User.UserLastName,User.IDUser,User.Password FROM User Where (User.IDUser='" + userID + "' or '"+userID+"'=null)";

                UserModel user = new UserModel();
                List<UserModel> users = new List<UserModel>();
                MySqlDataReader reader = conexionM.getExecuteQuery(query);

                while (reader.Read())
                {
                    user.Email = reader["EthereumContractUser"].ToString();
                    user.IDUser = reader["IDUser"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.UserLastName = reader["UserLastName"].ToString();
                    user.UserName = reader["UserName"].ToString();
                    users.Add(user);

                }

                return user;

            }
            catch (Exception err)
            {

                throw err;
            }
        }
    }
    public class UserModel
        {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserLastName { get; set; }
        public string IDUser { get; set; }
        public string Password { get; set; }
        public string Profile { get; set; }
    }
}

