using EthereumWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EthereumWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HomeModel modelo = new HomeModel();
            return View(modelo);
        }

        public ActionResult Login(HomeModel Data)
        {
            var email = Data.Email;
            var password = Data.Password;

            if (ModelState.IsValid)
            {
                // ir al Back
            }else
            {
                return View("Index", Data);
            }
            return View("Index", Data);
        }
    }
}
