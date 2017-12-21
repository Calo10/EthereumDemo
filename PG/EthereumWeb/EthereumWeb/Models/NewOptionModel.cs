using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class NewOptionModel
    {
        
        public string ProposalId { get; set; }

        public string ProposalName { get; set; }

        [Required(ErrorMessage = "Campo requerido")]
        public string OptionName { get; set; }

       public List<ProposalModel> Proposals { get; set; }

    }
}