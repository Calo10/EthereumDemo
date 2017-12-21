using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EthereumWeb.Models
{
  public  class EmailMasagge
    {
        public string AdressSMTP { get; set; }
        public int PortSMTP { get; set; }
        public bool EnableSSL { get; set; }
        public bool EnableCredencial { get; set; }
        public  List<string> Emails { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string CredencialUser{ get; set; }
        public string Body { get; set; }
        public string CredencialPass { get; set; }
    }
}
