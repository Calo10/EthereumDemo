using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using Nethereum.Web3.TransactionReceipts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendProgramathon.Controllers
{
    [Route("api/[controller]")]
    public class ContractController : Controller
    {
        // GET: api/values
        [HttpGet]
        public string Get()
        {
            return "1";
        }

       
    }
}
