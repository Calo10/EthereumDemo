using EthereumWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EthereumWeb.Controllers
{
    public class LoginInfo
    {
        public string Email { set; get; }
        public string Pass { set; get; }
    }
    public class LoginInfoDelete
    {
        public string Email { set; get; }


        public class LoginInfoToken
        {
            public string Token { set; get; }
        }

        public class LoginInfoUpdate
        {
            public string UserName { set; get; }
            public string UserLastName { set; get; }
            public string Email { set; get; }
            public string Pass { set; get; }
        }


        public class LoginInfoRegister
        {
            public string UserName { set; get; }
            public string UserLastName { set; get; }
            public string Email { set; get; }
            public string Pass { set; get; }
        }
        public class SecurityController : ApiController
        {


            [HttpPost]
            public UserModel Login(LoginInfoToken info)
            {
                UserModel user = new UserModel();
                HomeController controller = new HomeController();
                return controller.LoginToken(info.Token);

                //return new UserModel();
            }
            [HttpPost]
            public bool UpdatePassword(LoginInfo info)
            {
                UserModel user = new UserModel();
                UserController controller = new UserController();
                HomeModel home = new HomeModel
                {
                    Email = info.Email,
                    Password = info.Pass
                };
                return controller.UpdateUserPassword(home);

                //return new UserModel();
            }

            [HttpPost]
            public bool UserRegister(LoginInfoRegister userR)
            {
                UserModel user = new UserModel()
                {
                    Email = userR.Email,
                    Password = userR.Pass,
                    Profile = 1,
                    UserLastName = userR.UserLastName,
                    UserName = userR.UserName

                };

                UserController controller = new UserController();
                bool exist = false;
                bool valid = controller.RegisterUser(user, out exist);
                return valid;

                //return new UserModel();
            }

            [HttpPost]
            public bool UserUpdate(LoginInfoUpdate userUpdate)
            {
                ModifyUserModel user = new ModifyUserModel()
                {
                    Email = userUpdate.Email,
                    //  Password = userUpdate.Pass,
                    //  Profile = profile,
                    UserLastName = userUpdate.UserLastName,
                    UserName = userUpdate.UserName

                };

                UserController controller = new UserController();
                return controller.UpdateUsers(user);

                //return new UserModel();
            }
            [HttpGet]
            public List<UserModel> UserList(string email)
            {
                UserController controller = new UserController();
                return controller.UsersList(email);

                //return new UserModel();
            }

            [HttpPost]
            public bool DeleteUser(LoginInfoDelete user)
            {
                UserController controller = new UserController();
                return controller.DeleteUsers(user.Email);

                //return new UserModel();
            }
            // GET api/<controller>
            public IEnumerable<string> Get()
            {
                return new string[] { "value1", "value2" };
            }

            // GET api/<controller>/5
            public string Get(int id)
            {
                return "value";
            }

            // POST api/<controller>
            public void Post([FromBody]string value)
            {
            }

            // PUT api/<controller>/5
            public void Put(int id, [FromBody]string value)
            {
            }

            // DELETE api/<controller>/5
            public void Delete(int id)
            {
            }
        }
    }
}