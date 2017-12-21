using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EthereumWeb.Models;
using static EthereumWeb.Controllers.ProposalBussiness;

namespace EthereumWeb.Controllers
{
    public class VotesController : Controller
    {
        // GET: Votes
        public ActionResult Index(string id)
        {
            Session["propId"] = id;

            UserModel user = (UserModel)Session["User"];
            if (user == null)
            {
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ProposalFilter Filtro = new ProposalFilter();
                Filtro.Email = user.Email;
              //  Filtro.TypeVoting = 0;

                ProposalBussiness ProposalLogic = new ProposalBussiness();

                List<ProposalModel> ListProposal = new List<ProposalModel>();

                ListProposal = ProposalLogic.SearchProposalByUser(Filtro);

                ProposalModel currentProposal = ListProposal.Where(x => x.ContracEthereumProposal == id).FirstOrDefault();

                ViewBag.UserEmail = user.Email;
                return View(currentProposal);
            }
        }

        [HttpPost]
        public ActionResult Index(ProposalModel model)
        {
           
            List<OptionModel> lst = new List<OptionModel>();

            foreach (var item in model.SelectedOptions)
            {
                lst.Add(new OptionModel { IdOption = Convert.ToInt32(item) });
            }

            VotesModel vote = new VotesModel { Email = ((UserModel)Session["User"]).Email, Options = lst, ContracEthereumProposal = Session["propId"].ToString() };

            ProposalBussiness propBuss = new ProposalBussiness();

            bool resp = propBuss.DoVote(vote);

            //save
            if (resp)
            {
                return RedirectToAction("Proposal", "ProposalList");
            }

            return RedirectToAction("Proposal", "ProposalList");
            
        }
    }
}