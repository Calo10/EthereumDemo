using EthereumWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using static EthereumWeb.Controllers.ProposalBussiness;
using EthereumWeb.Constants;
using System.Net.Http.Headers;

namespace EthereumWeb.Controllers
{
    public class ProposalController : Controller
    {
        public ActionResult ProposalList()
        {
            UserModel user = (UserModel)Session["User"];
            if (user == null)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ProposalFilter Filtro = new ProposalFilter();
                Filtro.Email = user.Email;
                ///  Filtro.QuestionType = EnumQuestionType.Papeleta;

                ProposalBussiness ProposalLogic = new ProposalBussiness();

                List<ProposalModel> ListProposal = new List<ProposalModel>();

                ListProposal = ProposalLogic.SearchProposalByUser(Filtro);

                return View(ListProposal);
            }

        }
        public ActionResult RegisterUserProposalSumit(string Email)
        {
            SentMail email = new SentMail();

            string res = GenaratorPass();
            UserModel user = new UserModel();
            user.Email = Email;
            user.Password = res;
            string mensaje = "<p>Usted fue invitado a votar a la propuesta Salvemos a Lucho</p>" +
              "<p>Su Usuario es :" + Email + "</p>" +
                "<p>Su número de token para participar en la votación es : " + res + "</p>";

            using (UserDataAccess userDataAccess = new UserDataAccess())
            {
                List<UserModel> list = userDataAccess.SearchUser(user.Email);
                bool isValidUser = false;
                if (list == null || list.Count == 0)
                {
                    isValidUser = userDataAccess.InserUser(user);

                }
                else
                {
                    userDataAccess.UpdateUser(user, 3);
                }
            }

            email.SentEmail(mensaje, Email, "Solicitud de Votación", "Votacion de " + ((ProposalModel)Session["Proposal"]).ProposalName);
            ViewBag.Proposal = Session["Proposal"];
            return View("SendUpload");
        }
        public ActionResult NewProposal()
        {
            try
            {
                ProposalModel UserR = new ProposalModel();
                ViewBag.Cantidad = 10;

                return View(UserR);
            }
            catch
            {
                return View("Error");
            }

        }

        public ActionResult GraficVoting(string NumberContract)
        {
            try
            {
                List<ProposalModel> list = null;
                using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
                {
                    list = proposalDataAccess.SearchProposal(NumberContract, null);
                }
                    
                List<DataPointModel> dataPoints = new List<DataPointModel>();
                ProposalBussiness porpBisness = new ProposalBussiness();

                List<OptionModel> options = porpBisness.ConsultPartialResults(list[0]);
                foreach (var item in options)
                {
                    dataPoints.Add(new DataPointModel(0, item.NumberOfVotes, item.Description, item.Description + "=" + item.NumberOfVotes.ToString()));
                }

                List<VotesModel> Votes = porpBisness.SearchVotes(list[0].ContracEthereumProposal);

                string DatoSource = "[";
                foreach (DataPointModel item in dataPoints)
                {
                    string dat = " y:{0}, legendText: \"{1}\", indexLabel: \"{2}\"";
                    DatoSource += "{" + string.Format(dat, item.y.ToString(), item.legendText, item.indexLabel) + "},";
                }
                DatoSource += "]";
                ViewBag.DataPoints = DatoSource;// System.Web.WebPages.Html.Raw( JsonConvert.SerializeObject(dataPoints).Replace(@"""", @""));
                ViewBag.Description = "\"" + list[0].ProposalName + "\"";
                ViewBag.Proposal = list[0];
                ViewBag.Votes = Votes;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("ProposalList", "Proposal");
            }
        }

        public ActionResult Registrar(ProposalModel Data)
        {
            if (Data.QuestionTypeText == 0)
                Data.QuestionTypeText = 1; 

            Data.QuestionType = (EnumQuestionType)Data.QuestionTypeText; 

            ViewBag.Cantidad = 10;

            if (ModelState.IsValid)
            {
                ProposalBussiness ProposalLogic = new ProposalBussiness();
                UserModel user = (UserModel)Session["User"];

                if (Data.SelectedOptions.Count > 0)
                {
                    List<OptionModel> lst = new List<OptionModel>();
                    foreach (var item in Data.SelectedOptions[0].Split(','))
                    {
                        lst.Add(new OptionModel() { Description = item });
                    }
                    Data.Options = lst;
                }


                Data.UserCreator = user.Email;

                if (Data.InitialDate >= Data.FinalDate)
                {
                    ViewBag.Error = "La fecha de inicio no puede ser mayor a la fecha final";
                }
                else
                {

                    if (Data.MinimumQuantitySelected <= Data.MaximumQuantitySelected)
                    {

                        if (Data.InitialDate < DateTime.Now.Date)
                        {
                            ViewBag.Error = "La fecha de inicio no puede ser antes del dia de hoy.";
                        }
                        else
                        {

                            if (Data.QuestionType != EnumQuestionType.Referendum)
                            {
                                if (Data.Options.Count < 2)
                                {
                                    ViewBag.Error = "Debe ingresar al menos dos opciones";
                                }
                                else
                                {
                                    if (ProposalLogic.InsertProposal(Data))
                                    {
                                        return RedirectToAction("ProposalList", "Proposal");
                                    }
                                    else
                                    {
                                        return View("error");
                                    }
                                }
                            }
                            else
                            {
                                if (ProposalLogic.InsertProposal(Data))
                                {
                                    return RedirectToAction("ProposalList", "Proposal");
                                }
                                else
                                {
                                    return View("error");
                                }
                            }
                        }
                    }
                    else
                    {

                        ViewBag.Error = "La cantidad minina, debe ser menor o igual a la cantidad maxima";

                    }
                }

            }
            else
            {
                return View("NewProposal", Data);
            }
            return View("NewProposal", Data);
        }

        public ActionResult Edit(string ContracEthereumProposal)
        {
            try
            {
                ProposalBussiness ProposalLogic = new ProposalBussiness();

                ProposalModel Filtro = new ProposalModel();
                Filtro.ContracEthereumProposal = ContracEthereumProposal;

                ProposalModel CurrentProposal = new ProposalModel();

                CurrentProposal = ProposalLogic.SearchProposal(Filtro)[0];
                Session["Proposal"] = CurrentProposal;
                return View(CurrentProposal);

            }
            catch
            {
                return View("error");
            }
        }
        public ActionResult EditSummit(ProposalModel proposal)
        {
            try
            {
                proposal.ContracEthereumProposal = ((ProposalModel)Session["Proposal"]).ContracEthereumProposal;
                proposal.QuestionType = ((ProposalModel)Session["Proposal"]).QuestionType;
                UserModel user = (UserModel)Session["User"];

                using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
                {
                    if (proposalDataAccess.UpdateProposal(proposal))
                    {
                        ProposalFilter Filtro = new ProposalFilter();
                        Filtro.Email = user.Email;
                        //  Filtro.TypeVoting = 0;

                        ProposalBussiness ProposalLogic = new ProposalBussiness();

                        List<ProposalModel> ListProposal = new List<ProposalModel>();

                        ListProposal = ProposalLogic.SearchProposalByUser(Filtro);

                        ViewBag.Success = "Se modificó correctamente";
                        return View("ProposalList", ListProposal);

                    }
                    else
                    {
                        ProposalBussiness ProposalLogic = new ProposalBussiness();

                        ProposalModel Filtro = new ProposalModel();
                        Filtro.ContracEthereumProposal = proposal.ContracEthereumProposal;

                        ProposalModel CurrentProposal = new ProposalModel();

                        CurrentProposal = ProposalLogic.SearchProposal(Filtro)[0];
                        ViewBag.SucceErrorss = "Se modificó correctamente";
                        return View("Edit", CurrentProposal);

                    }
                }
            }
            catch
            {
                return View("error");
            }
        }

        public ActionResult RegisterOption(NewOptionModel Data)
        {
            try
            {
                ProposalBussiness ProposalLogic = new ProposalBussiness();

                if (ProposalLogic.InsertOption(Data.ProposalId, Data.OptionName))
                {

                    return RedirectToAction("NewOption", "Proposal", new { p = Data.ProposalId, n = Data.ProposalName });
                    //return RedirectToAction("ProposalList", "Proposal");
                }
                else
                {
                    return View("error");
                }

            }
            catch
            {
                return View("error");
            }
        }

        //public ActionResult RegistrarOption(UserModel Data)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ProposalBussiness ProposalLogic = new ProposalBussiness();
        //        ProposalLogi.


        //        bool existUser = false;
        //                bool valid = false;
        //                valid = RegisterUser(Data, out existUser);
        //                if (existUser)
        //                {
        //                    //si es true el usuario existe mesaje de que existe en la vista
        //                    ViewBag.ErrorUsuarioExiste = "El usuario " + Data.UserName + " ya fue registrado anteriormente";

        //                }
        //                else
        //                {
        //                    if (valid)
        //                    {
        //                        return RedirectToAction("UserList", "User");
        //                    }
        //                    else
        //                    {

        //                        ViewBag.ErrorDesconocido = "Occurió un error insertando. Por favor intentelo nuevamente.";
        //                    }
        //                }


        //    }
        //    else
        //    {
        //        return View("NewUser", Data);
        //    }
        //    return View("NewUser", Data);
        //}

        public ActionResult NewOption(string p, string n)
        {
            try
            {

                UserModel user = (UserModel)Session["User"];
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ProposalFilter Filtro = new ProposalFilter();
                    Filtro.Email = user.Email;
                    //   Filtro.TypeVoting = 0;

                    ProposalBussiness ProposalLogic = new ProposalBussiness();

                    List<ProposalModel> ListProposal = new List<ProposalModel>();


                    NewOptionModel NewOption = new NewOptionModel();

                    NewOption.ProposalId = p;
                    NewOption.ProposalName = n;
                    NewOption.Proposals = ProposalLogic.SearchProposalByUser(Filtro);

                    return View(NewOption);

                }


            }
            catch
            {
                return View("Error");
            }

        }

        public ActionResult SendUpload(string ContracEthereumProposal)
        {
            List<ProposalModel> list = null;

            using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
            {
               list = proposalDataAccess.SearchProposal(ContracEthereumProposal, null);
            }
           
            Session["Proposal"] = list[0];

            ViewBag.Proposal = list[0];

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

        public ActionResult Upload(HttpPostedFileBase file)
        {

            ProposalModel proposal = (ProposalModel)Session["Proposal"];
            if (file == null)
            {
                ViewBag.Error = "No selecciono un archivo";
                return View("RegisterUserProposal", null);
            }

            string archivo = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
            string ruta = Server.MapPath("~/Files/" + archivo);
            file.SaveAs(Server.MapPath("~/Files/" + archivo));
            if (RegistroMasivo(ruta, ((ProposalModel)Session["Proposal"]).ContracEthereumProposal, ((ProposalModel)Session["Proposal"]).ProposalName))
            {

                ViewBag.Success = "Se enviaron las invitaciones con éxito.";
                ProposalBussiness ProposalLogic = new ProposalBussiness();

                ProposalFilter Filtro = new ProposalFilter();
                UserModel user = (UserModel)Session["User"];
                Filtro.Email = user.Email;
                Filtro.QuestionType = null;
                List<ProposalModel> ListProposal = new List<ProposalModel>();

                ListProposal = ProposalLogic.SearchProposalByUser(Filtro);
                return View("ProposalList", ListProposal);
            }
            else
            {
                ViewBag.Error = "Hay Correos Invalidos";
                return View("RegisterUserProposal", null);
            }
            //  return View("ProposalList");
        }

        public ActionResult Delete(string p)
        {
            try
            {
                ProposalBussiness ProposalLogic = new ProposalBussiness();

                if (ProposalLogic.DeleteProposal(new ProposalModel() { ContracEthereumProposal = p }))
                {
                    return RedirectToAction("ProposalList", "Proposal");
                }
                else
                {
                    return View("error");
                }
            }
            catch
            {
                return View("error");
            }
        }

        public bool RegistroMasivo(string rutaArchivo, string ContracEthereumProposal, string NameProposal)
        {
            SentMail email = new SentMail();
            List<string> contrac = new List<string> { "0x12890d2cce102216644c59daE5baed380d84830c", "0x12890d2cce102216644c59daE5baed380d84830c", "0x12890d2cce102216644c59daE5baed380d84830c", "0x12890d2cce102216644c59daE5baed380d84830c", "0x72bc90bcee4f850e5f746dccf8afc48faff037b2", "0x6ba8d4304efae5edc7942762af0029ff514f8ceb", "0xca11d36c46848c289e9ab6d94ab012b26a5970bf", "0xae63afbac20dfd5a952bb7efe196196c40a7acd0", "0x6b23d35cfd1ce9c95b52e5cbfdc2ee1ffdec6a93", "0xdd3a891ca6ec6fe35ab69f42707c762868f7edf6", "0xd9708db1b7749e6ed3f4bdcb01ee033fe26f9da4", "0x34a4bab52953d436861d893a0d0133ab84d96c56" };
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

            using (ProposalDataAccess propasalDataAccess = new ProposalDataAccess())
            {
                using (UserDataAccess userDataAccess = new UserDataAccess())
                {
                    foreach (var item in correos)
                    {

                        string AccountNumber = contrac[0];
                        UserModel user = new UserModel()
                        {
                            Profile = 3,
                            Email = item,
                            Password = GenaratorPass(),
                            IsFirstLogger = EnumRegister.NoRegister,
                        };
                        contrac.Remove(AccountNumber);

                        List<UserModel> list = userDataAccess.SearchUser(user.Email);
                        bool isValidUser = false;
                        if (list == null || list.Count == 0)
                        {
                            isValidUser = userDataAccess.InserUser(user);

                        }
                        else
                        {
                            //el usuario existe no se registra
                            isValidUser = true;
                        }
                        if (isValidUser)
                        {
                            isValidUser = propasalDataAccess.InsertProposalUser(ContracEthereumProposal, item);
                            if (list.Count > 0)
                            {
                                string mensaje = "<p>Usted fue invitado a votar a la propuesta " + NameProposal + "</p>" +
                                   "<p>Su Usuario es :" + item + "</p>" +
                                   "<p>Ruta de acceso: <a href = 'http://raykel.eastus.cloudapp.azure.com/EthereumWeb/' >Ingreso </a> </p>";

                                email.SentEmail(mensaje, item, "Solicitud de Votación", "Votacion de " + NameProposal);
                            }
                            else
                            {
                                if (isValidUser)
                                {
                                    string mensaje = "<p>Usted fue invitado a votar a la propuesta " + NameProposal + "</p>" +
                                     "<p>Su Usuario es :" + item + "</p>" +
                                     "<p>Su número de token para participar en la votación es : " + user.Password + "</p>" +
                                     "<p>Ruta de acceso: <a href='http://raykel.eastus.cloudapp.azure.com/EthereumWeb/' >Ingreso </a></p>";

                                    email.SentEmail(mensaje, item, "Solicitud de Votación", "Votacion de " + NameProposal);
                                }
                            }
                        }
                    }
                }
            }


            return true;
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
    }

    public class ProposalAPIController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<ProposalModel> SearchProposal(ProposalModel filter)
        {
            ProposalBussiness bussiness = new ProposalBussiness();
            return bussiness.SearchProposal(filter);
        }

        [System.Web.Http.HttpPost]
        public List<ProposalModel> SearchProposalByUser(ProposalFilter filter)
        {
            ProposalBussiness bussiness = new ProposalBussiness();
            return bussiness.SearchProposalByUser(filter);
        }

        [System.Web.Http.HttpPost]
        public List<OptionModel> ConsultPartialResults(ProposalModel filter)
        {
            ProposalBussiness bussiness = new ProposalBussiness();
            return bussiness.ConsultPartialResults(filter);
        }

        [System.Web.Http.HttpPost]
        public bool DoVote(VotesModel vote)
        {
            ProposalBussiness bussiness = new ProposalBussiness();
            return bussiness.DoVote(vote);
        }
    }


    public class VoteJson
    {
        public string Voter { set; get; }
        public string Email { set; get; }
        public int OptionId { set; get; }
    }
    public class Option
    {
        public string Name { set; get; }

        public int NumberOfVotes { set; get; }
    }
    public class ProposalBussiness
    {
        public bool DeleteProposal(ProposalModel proposal)
        {
            bool isSuccess;
            using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
            {
                isSuccess = proposalDataAccess.DeleteProposal(proposal);
            }
            return isSuccess;
        }

        public List<ProposalModel> SearchProposal(ProposalModel filter)
        {
            List<ProposalModel> list = null;
            using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
            {
                if (filter != null)
                {
                    list = proposalDataAccess.SearchProposal(filter.ContracEthereumProposal, filter.ProposalName);
                }

                list = proposalDataAccess.SearchProposal(null, null);
            }
            return list;
        }

        public List<ProposalModel> SearchProposalByUser(ProposalFilter filter)
        {
            List<ProposalModel> list = null;
            if (filter != null && filter.Email != null && filter.Email != string.Empty)
            {
                using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
                {
                    if (filter != null)
                    {
                        list = proposalDataAccess.SearchProposalByUser(filter.Email, (EnumQuestionType?)filter.QuestionType);
                    }

                    list = proposalDataAccess.SearchProposal(null, null);
                }
            }

            return list;
        }

        public bool InsertProposal(ProposalModel proposal)
        {
            try
            {
                if (proposal == null)
                    return false;
                proposal.QuestionType = proposal.QuestionType == EnumQuestionType.Referendum ? EnumQuestionType.Referendum : proposal.QuestionType;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(API_Dictionary.Base);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                DateTime dateCalcule = DateTime.Today;
                HttpResponseMessage response = client.GetAsync(string.Format(API_Dictionary.InsertProposal,
                                                                            proposal.ProposalName,
                                                                            DiffDateTimeInMinutes(dateCalcule, proposal.InitialDate),
                                                                            DiffDateTimeInMinutes(dateCalcule, proposal.FinalDate),
                                                                           (int)proposal.QuestionType,
                                                                           (int)proposal.SecurityType,
                                                                            proposal.AdvancedSearch,
                                                                            proposal.MinimumQuantitySelected,
                                                                            proposal.MaximumQuantitySelected
                                                                        )).Result;

                bool IsSuccess;
                if (response.IsSuccessStatusCode)
                {
                    proposal.ContracEthereumProposal = response.Content.ReadAsAsync<string>().Result;

                    if (proposal.Options == null)
                    {
                        proposal.Options = new List<OptionModel>();
                    }

                    if (proposal.QuestionType == EnumQuestionType.Referendum)
                    {
                        proposal.Options = new List<OptionModel>();
                        proposal.Options.Add(new OptionModel() { Description = "SI" });
                        proposal.Options.Add(new OptionModel() { Description = "NO" });
                    }

                    foreach (OptionModel option in proposal.Options)
                    {
                        int idOption = 0;

                        IsSuccess = InsertInBlockChain(option.Description, proposal.ContracEthereumProposal, ref idOption);

                        option.IdOption = idOption;

                        if (!IsSuccess)
                        {
                            return false;
                        }
                    }

                    using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
                    {
                        IsSuccess = InsertProposal(proposal);
                    }
                    return IsSuccess;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private double DiffDateTimeInMinutes(DateTime dateNow, DateTime date)
        {
            TimeSpan span = date - dateNow;
            return span.TotalMinutes;
        }

        private bool InsertInBlockChain(string description, string contracEthereumProposal, ref int idOption)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(API_Dictionary.Base);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(string.Format(API_Dictionary.InsertOptionProposal,
                                                                        contracEthereumProposal,
                                                                        description
                                                                    )).Result;

            idOption = response.Content.ReadAsAsync<int>().Result;

            return response.IsSuccessStatusCode;
        }

        private List<VoteJson> SearchVotesInBlockChain(string contracEthereumProposal)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(API_Dictionary.Base);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(string.Format(API_Dictionary.SearchOptionProposal,
                                                                        contracEthereumProposal
                                                                      )).Result;
            List<VoteJson> listVotes = response.Content.ReadAsAsync<List<VoteJson>>().Result;

            return listVotes;
        }

        private List<Option> SearchResultInBlockChain(string contracEthereumProposal)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(API_Dictionary.Base);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync(string.Format(API_Dictionary.SearchResultProposal,
                                                                        contracEthereumProposal
                                                                      )).Result;
            List<Option> listVotes = response.Content.ReadAsAsync<List<Option>>().Result;

            return listVotes;
        }
        public List<VotesModel> SearchVotes(string contracEthereumProposal)
        {
            List<VotesModel> list = new List<VotesModel>();
            List<VoteJson> listVotes = SearchVotesInBlockChain(contracEthereumProposal);
            List<OptionModel> optios = null;
            using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
            {
                optios = proposalDataAccess.SearchOptionByProposal(contracEthereumProposal);
            }

            foreach (var item in listVotes)
            {
                if (item.Voter != null)
                {
                    list.Add(new VotesModel
                    {
                        //TODO JOSE debe de enviar el contrato
                        AccountNumber = item.Voter,
                        ContracEthereumProposal = contracEthereumProposal,
                        Email = item.Email,
                        Options = new List<OptionModel>() { optios.FirstOrDefault(a => a.IdOption == item.OptionId) }
                    });
                }
            }
            return list;
        }

        public bool InsertOption(string contracEthereumProposal, string description)
        {
            int idOption = 0;
            if (InsertInBlockChain(description, contracEthereumProposal, ref idOption))
            {
                bool isUpdateSuccess; ;
                using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
                {
                    isUpdateSuccess = proposalDataAccess.InsertOption(idOption, contracEthereumProposal, description);
                }
            }
            return false;
        }

        public bool UpdateProposal(ProposalModel proposal)
        {
            try
            {
                if (proposal == null)
                    return false;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(API_Dictionary.Base);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                DateTime dateCalcule = DateTime.Today;
                HttpResponseMessage response = client.GetAsync(string.Format(API_Dictionary.UpdateProposal,
                                                                            proposal.ContracEthereumProposal,
                                                                            proposal.ProposalName,
                                                                            DiffDateTimeInMinutes(dateCalcule, proposal.InitialDate),
                                                                            DiffDateTimeInMinutes(dateCalcule, proposal.FinalDate),
                                                                            (int)proposal.SecurityType,
                                                                            proposal.AdvancedSearch,
                                                                            proposal.MinimumQuantitySelected,
                                                                            proposal.MaximumQuantitySelected
                                                                        )).Result;
                if (response.IsSuccessStatusCode)
                {
                    proposal.ContracEthereumProposal = response.Content.ReadAsAsync<string>().Result;

                    bool isUpdateSuccess;
                    using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
                    {
                        isUpdateSuccess = proposalDataAccess.UpdateProposal(proposal);
                    }
                    return isUpdateSuccess;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<OptionModel> ConsultPartialResults(ProposalModel filter)
        {
            List<Option> result = SearchResultInBlockChain(filter.ContracEthereumProposal);
            List<OptionModel> list = new List<OptionModel>();
            foreach (var item in result)
            {
                list.Add(new OptionModel() { Description = item.Name, NumberOfVotes = item.NumberOfVotes });
            }
            return list;
        }


        public bool DoVote(VotesModel vote)
        {
            if (vote != null && vote.ContracEthereumProposal != null && vote.ContracEthereumProposal != string.Empty && vote.Options != null && vote.Options.Count > 0
                && ((vote.Email != null && vote.Email != string.Empty) || (vote.AccountNumber != null && vote.AccountNumber != string.Empty)))
            {
                foreach (OptionModel option in vote.Options)
                {
                    InsertVote(vote.ContracEthereumProposal, option.IdOption.ToString(), vote.AccountNumber, vote.Email);
                }

                using (ProposalDataAccess proposalDataAccess = new ProposalDataAccess())
                {
                    proposalDataAccess.UserVoted(vote.Email, vote.ContracEthereumProposal);
                }

                return true;
            }
            return false;
        }

        private bool InsertVote(string proposalId, string optionId, string userSenderAddress, string email)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(API_Dictionary.Base);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(string.Format(API_Dictionary.AddVoteToProposal,
                                                                            proposalId,
                                                                            optionId,
                                                                            userSenderAddress,
                                                                            email
                                                                        )).Result;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public class ProposalFilter
        {
            public string Email { get; set; }

            public EnumQuestionType? QuestionType { get; set; }
        }
    }


}