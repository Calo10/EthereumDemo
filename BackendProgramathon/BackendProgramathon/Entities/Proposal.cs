using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendProgramathon.Entities
{
    public class Proposal
    {
        public string Description { set; get; }
        public int StartTime { set; get; }
        public int FinishTime { set; get; }
        public int VoteType { set; get; }
        public int VotationType { set; get; }
        public int Advances { set; get; }
        


    }
}
