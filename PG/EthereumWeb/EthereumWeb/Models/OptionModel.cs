using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthereumWeb.Models
{
    public class OptionModel
    {
        public int IdOption { get; set; }

        public string Description { get; set; }

        public long NumberOfVotes { get; set; }
    }
}