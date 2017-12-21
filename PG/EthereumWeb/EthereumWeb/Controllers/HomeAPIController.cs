using EthereumWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EthereumWeb.Controllers
{
    public class HomeAPIController : ApiController
    {
        [HttpPost]
        public UserModel Login(string email, string pass)
        {
            UserModel user = new UserModel();
            HomeController controller = new HomeController();
           return controller.LoginConenction(email, pass);
        }

       
    }
}