using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class UserDataAccess : DataAccess
    {
        public bool UpdateUser(UserModel user,int UpdateType )
        {
            try
            {
                List<ParameterSchema> lstParams = new List<ParameterSchema>();
                string query = string.Empty;

                lstParams.Add(new ParameterSchema("Email", user.Email));
                lstParams.Add(new ParameterSchema("UserName", user.UserName));
                lstParams.Add(new ParameterSchema("UserLastName", user.UserLastName));
                lstParams.Add(new ParameterSchema("Password", user.Password));
                lstParams.Add(new ParameterSchema("Profile", user.Profile));

                //Full user update
                if (UpdateType == 1)
                {
                    //Inserta Nuevo Curso
                    query = "UPDATE dbEthereumDemo.User SET " +
                    "UserName = '" + user.UserName + "',UserLastName = '" + user.UserLastName + "',Password = '" + user.Password + "',Profile = 1" +
                    " WHERE Email = '" + user.Email + "'";


                }//Name aand lasname updateuser
                else if(UpdateType==2) {
                    query = "UPDATE dbEthereumDemo.User SET " +
                   "UserName = '" + user.UserName + "',UserLastName = '" + user.UserLastName +"'"+
                   " WHERE Email = '" + user.Email + "'";
                }//update only pass
                else if (UpdateType == 3)
                {
                    query = "UPDATE dbEthereumDemo.User SET " +
                   "Password = '" + user.Password + "'"+
                   " WHERE Email = '" + user.Email + "'";
                }//only isFirtsLogger
                else if (UpdateType == 4)
                {
                    query = "UPDATE dbEthereumDemo.User SET " +
                   "IsFirstLogger = '" + (byte)user.IsFirstLogger+ "'" +
                   " WHERE Email = '" + user.Email + "'";
                }

                ExecuteNonQuery(query);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool InserUser(UserModel user)
        {
            try
            {
                List<ParameterSchema> lstParams = new List<ParameterSchema>();
                string query = string.Empty;

                lstParams.Add(new ParameterSchema("Email", user.Email));
                lstParams.Add(new ParameterSchema("UserName", user.UserName));
                lstParams.Add(new ParameterSchema("UserLastName", user.UserLastName));
                lstParams.Add(new ParameterSchema("Password", user.Password));
                lstParams.Add(new ParameterSchema("Profile", user.Profile));
                lstParams.Add(new ParameterSchema("IsFirstLogger",(byte) user.IsFirstLogger));

                
                if (!string.IsNullOrEmpty(user.Email))
                {
                    //Inserta Nuevo Curso
                    query = "INSERT INTO dbEthereumDemo.User(Email,UserName,UserLastName,Password,Profile,IsFirstLogger)" +
                        "VALUES(@Email, @UserName, @UserLastName, @Password, @Profile,@IsFirstLogger); ";
                }

                ExecuteQuery(query, lstParams);

                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
        public UserModel GetUserByPassword(string email, string pass)
        {
            UserModel user = null;
            try
            {
                string query = "SELECT User.Email,User.UserName,User.UserLastName,User.Profile  FROM User Where User.Email='" + email + "' AND User.Password='" + pass + "'";

                using (MySqlDataReader reader = ExecuteQuery(query))
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Email = reader["Email"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            UserLastName = reader["UserLastName"].ToString(),
                            Profile = int.Parse(reader["Profile"].ToString())
                        };
                    }
                }
            }
            catch (Exception)
            {
            }
            return user;
        }

        public UserModel GetUserByToken(string token)
        {
            UserModel user = null;
            try
            {
                string query = "SELECT User.Email,User.UserName,User.UserLastName,User.Profile,User.IsFirstLogger FROM User Where User.Password='" + token + "'; ";

                using (MySqlDataReader reader = ExecuteQuery(query))
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Email = reader["Email"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            UserLastName = reader["UserLastName"].ToString(),
                            Profile = int.Parse(reader["Profile"].ToString()),
                            IsFirstLogger = reader["IsFirstLogger"] == null ? EnumRegister.NoRegister : (EnumRegister)int.Parse(reader["IsFirstLogger"].ToString())
                        };
                    }
                }
            }
            catch (Exception)
            {
            }
            return user;
        }
        public List<UserModel> SearchUser(string email)
        {
            List<UserModel> users = new List<UserModel>();
            try
            {
                string query = string.Empty;
                if (string.IsNullOrEmpty(email))
                {
                     query = "SELECT User.Email,User.UserName,User.UserLastName,User.Profile, CASE WHEN Exists(Select ContracEthereumProposal from Proposal where UserCreator=Email) THEN 1 else 0 END 'IsProcessActive',IsFirstLogger  FROM User";

                }
                else
                {
                     query = "SELECT User.Email,User.UserName,User.UserLastName,User.Profile , CASE WHEN Exists(Select ContracEthereumProposal from Proposal where UserCreator=Email) THEN 1 else 0 END 'IsProcessActive',IsFirstLogger FROM User Where (User.Email='" + email + "' or '" + email + "'=null)";

                }
       
                MySqlDataReader reader = ExecuteQuery(query);

                while (reader.Read())
                {
                    UserModel user = new UserModel();
                    user.Email = reader["Email"].ToString();
                    user.UserName = reader["UserName"].ToString();
                    user.UserLastName = reader["UserLastName"].ToString();
                    user.Profile = Convert.ToInt32(reader["Profile"]);
                    user.IsProcessActive = Convert.ToBoolean(reader["IsProcessActive"]);
                    user.IsFirstLogger = reader["IsFirstLogger"] == null? EnumRegister.NoRegister : (EnumRegister) int.Parse(reader["IsFirstLogger"].ToString());
                    users.Add(user);
                }
            }
            catch (Exception)
            {
            }
            return users;
        }

        public bool DeleteUser(string email)
        {
            try
            {
                string query = "delete  FROM User Where (User.Email='" + email + "')";
                ExecuteNonQuery(query);
                             
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
    }
}

