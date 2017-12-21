using EthereumWeb.Constants;
using EthereumWeb.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace EthereumWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session.Clear();
            Session.Abandon();
            HomeModel modelo = new HomeModel();


            return View(modelo);
        }

        public ActionResult Login(HomeModel Data)
        {

            var response = Request["g-recaptcha-response"];
            string secretKey = "6LebxDEUAAAAAFVuoojjTYtcRyClz12WdHS8MNlG";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            ViewBag.Message = status ? "Google reCaptcha validation success" : "Validación del captcha es incorrecta";
            if (true)
            {
                if (ModelState.IsValid)
                {
                    UserModel userLogged = null;
                    using (UserDataAccess userDataAccess = new UserDataAccess())
                    {
                        userLogged = userDataAccess.GetUserByPassword(Data.Email, Data.Password);
                    }

                    if (userLogged != null)
                    {
                        Session["User"] = userLogged;

                        return RedirectToAction("ProposalList", "Proposal");
                    }
                    else
                    {
                        ViewBag.ErrorLogin = "Usuario o password inválido";
                    }

                }
                else
                {
                    return View("Index", Data);
                }
            }
            else
            {
                return View("Index", Data);

            }
            return View("Index", Data);
        }

        public UserModel LoginConenction(string email, string pass)
        {
            UserModel user = null;
            using (UserDataAccess userDataAccess = new UserDataAccess())
            {
                user = userDataAccess.GetUserByPassword(email, pass);
            }

            return user;
        }
        public UserModel LoginToken(string token)
        {
            UserModel user = null;
            using (UserDataAccess userDataAccess = new UserDataAccess())
            {
                user = userDataAccess.GetUserByToken(token);
                if (user.IsFirstLogger == EnumRegister.NoRegister)
                {
                    SecurityBusiness securyty = new SecurityBusiness();
                    user.Contract = securyty.ContractEthereum();
                    user.IsFirstLogger = EnumRegister.Register;
                    userDataAccess.UpdateUser(user, 4);
                }
                else
                {
                    //cuando el usuario ya fue registrado el contrato sera nulo
                    user.Contract = string.Empty;
                }
            }



            return user;
        }

        public ActionResult ForgotPassword()
        {

            return View();
        }

        public ActionResult SentEmailForgotPasswor(string email)
        {
            SentMail mail = new SentMail();

            mail.SentEmail("Se solicito un cambio de contraseña, por favor dirigirse a la direccion http://localhost/EthereumWeb/Home/ChangePassword?email=" + email, email, "Solicitud de Cambio de Contraseña", "Cambio de COntraseña");
            ViewBag.Envio = "Se envió el correo para la modificación de la contraseña";
            return View(ViewBag);
        }
        public ActionResult ChangePassword(string email)
        {
            UserChangePasswordModel user = new UserChangePasswordModel();
            user.Email = email;
            return View(user);
        }

        public ActionResult ChangePasswordSumit(UserChangePasswordModel user)
        {
            ViewBag.ErrorLogin = "";
            HomeModel model = new HomeModel()
            {
                Email = user.Email,
                Password = user.Password
            };

            if (user.Password == user.PasswordValidated)
            {
                UserModel userm = new UserModel
                {
                    Password = user.Password,
                    Email = user.Email
                };

                bool valid;
                using (UserDataAccess userDataAccess = new UserDataAccess())
                {
                    valid = userDataAccess.UpdateUser(userm, 3);
                }

                ViewBag.Succefull = "Cambio exitoso";

                return View("Index", new HomeModel());
            }
            else
            {
                UserChangePasswordModel user1 = new UserChangePasswordModel();
                user1.Email = user.Email;
                ViewBag.Error = "Las Contraseñas son distintas";
                return View("ChangePassword", user1);
            }
        }

        public bool UpdateUserPassword(HomeModel user)
        {
            UserModel userm = new UserModel
            {
                Password = user.Password,
                Email = user.Email
            };

            bool valid;
            using (UserDataAccess userDataAccess = new UserDataAccess())
            {
                valid = userDataAccess.UpdateUser(userm, 3);
            }

            return valid;
        }
    }
    public class ContractDemo
    {
        public string Address { set; get; }
    }
    public class SecurityBusiness
    {
        public string ContractEthereum()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(API_Dictionary.Base);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(API_Dictionary.CreateAccount).Result;

                ContractDemo contract = response.Content.ReadAsAsync<ContractDemo>().Result;

                return contract.Address;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
    }
}
