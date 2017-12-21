using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProgramathon.Entities
{
    [FunctionOutput]
    public class Vote
    {
        [Parameter("address", 1)]
        public string Voter { set;  get; }

        [Parameter("int", 1)]
        public int OptionId { set; get; }

        //[Parameter("string", 1)]
        //public string  Email { set; get; }
    }
}
