using EthereumWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EthereumWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: Users
        public ActionResult UserList()
        {
            UserModel user = (UserModel)Session["User"];
            if (user == null)
            {
                //Enviar a login

            }





            List<UserModel> ListUsers = new List<UserModel>();

            ListUsers = UsersList(null);

            return View(ListUsers);
        }

        public ActionResult NewUser()
        {
            try
            {
                UserModel UserR = new UserModel();


                return View(UserR);
            }
            catch
            {
                return View("Error");
            }

        }

        public ActionResult Modify(string m)
        {
            try
            {
                UserModel UserByModify = new UserModel();

                UserByModify = UsersList(m)[0];
                return View(UserByModify);
            }
            catch
            {
                return View("Error");
            }

        }

        public ActionResult Delete(string m)
        {
            try
            {

                if (DeleteUsers(m))
                {
                    return RedirectToAction("UserList", "User");
                }
                else
                {
                    return RedirectToAction("UserList", "User");
                }


            }
            catch
            {
                return View("Error");
            }
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult Registrar(UserModel Data)
        {
            if (ModelState.IsValid)
            {
                if (IsValidEmail(Data.Email))
                {
                    if (Data.Profile != 0)
                    {

                        if (Data.Password != Data.RepitePassword)
                        {
                            ViewBag.PasswordNoCoincide = "Las contraseñas deben coincidir";
                        }
                        else
                        {
                            bool existUser = false;
                            bool valid = false;
                            valid = RegisterUser(Data, out existUser);
                            if (existUser)
                            {
                                //si es true el usuario existe mesaje de que existe en la vista
                                ViewBag.ErrorUsuarioExiste = "El usuario " + Data.UserName + " ya fue registrado anteriormente";

                            }
                            else
                            {
                                if (valid)
                                {
                                    return RedirectToAction("UserList", "User");
                                }
                                else
                                {

                                    ViewBag.ErrorDesconocido = "Occurió un error insertando. Por favor intentelo nuevamente.";
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ErrorDesconocido = "Debe seleccionar un Tipo de Usuario";
                    }
                }
                else
                {
                    ViewBag.ErrorDesconocido = "Correo Inválido";
                }
            }
            else
            {
                return View("NewUser", Data);
            }
            return View("NewUser", Data);
        }

        public ActionResult ModifyUser(ModifyUserModel ModifyUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (UpdateUsers(ModifyUser))
                    {
                        return RedirectToAction("UserList", "User");
                    }
                    else
                    {
                        ViewBag.ErrorModificandoUsuario = "Ocurio un error modificando al usuario " + ModifyUser.UserName + ". Por favor intente nuevamente.";
                    }

                }
            }
            catch
            {
                return View("Error");
            }

            return View("Modify", ModifyUser);
        }

        #region Logica
        public bool RegisterUser(UserModel user, out bool UserExist)
        {
            bool valid = false;
            UserExist = false;

            List<UserModel> userConsult = null;

            using (UserDataAccess userDataAccess = new UserDataAccess())
            {
                userConsult = userDataAccess.SearchUser(user.Email);


                if (userConsult != null && userConsult.Count > 0)
                {
                    UserExist = true;
                }
                else
                {
                    valid = userDataAccess.InserUser(user);

                    if (valid)
                    {
                        SentMail mail = new SentMail();
                        if (user.Profile == 1)
                        {



                            EmailMasagge emailMesagge = new EmailMasagge();
                            string htmlTemplade = "<table border='0' cellpadding='0' cellspacing='0' width='100%'><tr><td style='padding: 10px 0 30px 0;'><table align='center' border='0' cellpadding='0' cellspacing='0' width='600' style='border: 1px solid #cccccc; border-collapse: collapse;'>" +
        "<tr><td align='center' bgcolor='#70bbd9' style='padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;'>" +
        "</td></tr><tr Style='font-family: Calibri'><td bgcolor='#ffffff' style='padding: 40px 30px 40px 30px; font-family: Calibri'>{0}" +
        "</td></tr><tr><td bgcolor='#0a4871' style='padding: 30px 30px 30px 30px;'><table border='0' cellpadding='0' cellspacing='0' width='100%'><tr>" +
        "<td style='color: #ffffff; font-family: Calibri; font-size: 14px;' width='75%'>Copyright 2017 | Todos los Derechos Reservados<br/>" +
        "<a href='http://raykel.eastus.cloudapp.azure.com/EthereumWeb/' style='color: #ffffff;'><font color='#ffffff'>Ingreso</font></a> </td><td align='right' width='25%'></td></tr></table></td></tr></table></td></tr></table>";
                            emailMesagge.AdressSMTP = "smtp.gmail.com";
                            emailMesagge.Body = string.Format(htmlTemplade, "Se acaba de registrar el correo electrónico:" + user.Email + " y su contraseña: " + user.Password);
                            emailMesagge.CredencialPass = "prosoft123";
                            emailMesagge.CredencialUser = "testprosofter@gmail.com";
                            emailMesagge.Emails = new List<string> { user.Email };
                            emailMesagge.EnableCredencial = true;
                            emailMesagge.EnableSSL = true;
                            emailMesagge.PortSMTP = 587;
                            emailMesagge.From = "Permiso de Creación de Propuesta";
                            emailMesagge.Subject = "Datos de acceso";

                            mail.Email(emailMesagge);
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }
            }
            return valid;
        }

        public bool RegistroMasivo(string rutaArchivo)
        {

            System.IO.StreamReader objReader = new System.IO.StreamReader("c:\\test.txt");
            string sLine = "";
            System.Collections.ArrayList arrText = new System.Collections.ArrayList();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    arrText.Add(sLine);
            }
            objReader.Close();
            List<string> correos = new List<string>();
            foreach (string sOutput in arrText)
            {
                correos.AddRange(sOutput.Split(';'));
            }
            foreach (var item in correos)
            {
                UserModel user = new UserModel()
                {
                    Profile = 3,
                    Email = item,
                    Password = "123"
                }
                ;
            }

            return true;
        }
        public List<UserModel> UsersList(string email)
        {
            List<UserModel> users = null;

            using (UserDataAccess userDataAccess = new UserDataAccess())
            {
                users = userDataAccess.SearchUser(email);
            }

            return users;
        }

        public bool DeleteUsers(string email)
        {
            bool valid;
            using (UserDataAccess userDataAccess = new UserDataAccess())
            {
                valid = userDataAccess.DeleteUser(email);
            }
            return valid;
        }

        public bool UpdateUsers(ModifyUserModel user)
        {
            UserModel userM = new UserModel()
            {
                Email = user.Email,
                UserLastName = user.UserLastName,
                UserName = user.UserName
            };
            bool valid;
            using (UserDataAccess userDataAccess = new UserDataAccess())
            {
                valid = userDataAccess.UpdateUser(userM, 2);
            }
            return valid;
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
        #endregion
    }
}