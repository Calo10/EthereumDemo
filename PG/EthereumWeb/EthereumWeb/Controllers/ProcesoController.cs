using EthereumWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EthereumWeb.Controllers
{
    public class ProcesoController : Controller
    {
        // GET: Proceso
        public ActionResult Proceso()
        {
            return View();
        }

        public ActionResult Registrar(UserModel Data)
        {
            var email = Data.Email;
            var password = Data.Password;

            if (ModelState.IsValid)
            {
                // ir al Back
            }
            else
            {
                return View("Index", Data);
            }
            return View("Index", Data);
        }
        public ActionResult RegisterUserProposal()
        {

            return View();
        }
        public ActionResult RegisterUserProposalSumit(string Email)
        {
            SentMail email = new SentMail();

            string res = GenaratorPass();
            UserModel user = new UserModel()
            {
                Email = Email,
                Password = res,
            };


            string mensaje = "<p>Usted fue invitado a votar a la propuesta Salvemos a Lucho</p>" +
              "<p>Su Usuario es :" + Email + "</p>" +
                "<p>Su número de token para participar en la votación es : " + res + "</p>";

            bool valid;
            using (UserDataAccess userDataAccess = new UserDataAccess())
            {
                valid = userDataAccess.UpdateUser(user, 3);
            }

            return View();
        }

        private static string GenaratorPass()
        {
            Random aleatorio = new Random();
            Random aleatorio2 = new Random();
            int l = aleatorio.Next(6, 7);
            string res = "";
            string[] vals = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            for (int i = 0; i <= l; i++)
            {
                res = res + vals[aleatorio.Next(vals.GetUpperBound(0) + 1)];
            }

            return res;
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.Error = "No selecciono un archivo";
                return View("RegisterUserProposal", null);
            }

            string archivo = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
            string ruta = Server.MapPath("~/Files/" + archivo);
            file.SaveAs(Server.MapPath("~/Files/" + archivo));
            if (RegistroMasivo(ruta, "com.webs.Bytecard"))
            {

            }
            else
            {
                ViewBag.Error = "Hay Correos Invalidos";
                return View("RegisterUserProposal", null);
            }
            return View();
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
        public ActionResult Edit()
        {

            return View();
        }
        public bool RegistroMasivo(string rutaArchivo, string ContracEthereumProposal)
        {
            SentMail email = new SentMail();
            System.IO.StreamReader objReader = new System.IO.StreamReader(rutaArchivo);
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
                if (!IsValidEmail(item))
                {
                    return false;
                }
            }

            foreach (var item in correos)
            {

                UserModel user = new UserModel()
                {
                    Profile = 3,
                    Email = item,
                    Password = GenaratorPass(),
                    IsFirstLogger = EnumRegister.NoRegister
                };

                using (UserDataAccess userDataAccess = new UserDataAccess())
                {
                    if (userDataAccess.UpdateUser(user, 3))
                    {
                        using (ProposalDataAccess proposal = new ProposalDataAccess())
                        {
                            if (proposal.InsertProposalUser(ContracEthereumProposal, item))
                            {
                                string mensaje = "<p>Usted fue invitado a votar a la propuesta Salvemos a Lucho</p>" +
                                 "<p>Su Usuario es :" + item + "</p>" +
                                 "<p>Su número de token para participar en la votación es : " + user.Password + "</p>";
                                // email.SentEmail(mensaje, item);
                            }
                        }
                    }
                }
            }


            return true;
        }

        public ActionResult GraficoVoting(string NumberContract)
        {
            List<DataPointModel> dataPoints = new List<DataPointModel>{
                new DataPointModel(10, 22,"Raykel 22%","Raykel 22%"),
                new DataPointModel(20, 36,"Calos 36%","raykel 36%"),
                new DataPointModel(30, 42,"Jose 42%","Jose 42%"),
                new DataPointModel(40, 51,"Johan 51%","Joan 51%"),
                new DataPointModel(50, 46,"Jean 46%","Jean 46"),
            };
            string DatoSource = "[";
            foreach (DataPointModel item in dataPoints)
            {
                string dat = " y:{0}, legendText: \"{1}\", indexLabel: \"{2}\"";
                DatoSource += "{" + string.Format(dat, item.y.ToString(), item.legendText, item.indexLabel) + "},";
            }
            DatoSource += "]";
            ViewBag.DataPoints = DatoSource;// System.Web.WebPages.Html.Raw( JsonConvert.SerializeObject(dataPoints).Replace(@"""", @""));

            return View();
        }
    }


}