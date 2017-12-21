using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProgramathon.Entities
{
    [FunctionOutput]
    public class Option
    {
        [Parameter("string", 1)]
        public string Name { set; get; }

        [Parameter("int", 1)]
        public int NumberOfVotes { set; get; }
    }
}
