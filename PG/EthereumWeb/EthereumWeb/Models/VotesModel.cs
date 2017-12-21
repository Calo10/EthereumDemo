using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class VotesModel
    {
        public string Email { get; set; }

        public string AccountNumber { get; set; }

        public string ContracEthereumProposal { get; set; }

        public List<OptionModel> Options { get; set; }
    }
}