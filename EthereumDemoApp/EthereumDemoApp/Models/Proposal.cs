using System;

using Xamarin.Forms;

namespace EthereumDemoApp.Models
{
    public class Proposal
    {
        string owner;
       string processName;
       uint votingDeadlineStart;
       uint votingDeadlineFinish;
       uint voteType; //1 = Papeleta, 2 = Referendum, 3 = OpcionMultiple
       uint votationType; //1= Private, 2 = Public
       uint advances; // 1= SI, 2= NO
       uint numberOfVotes;

       Member[] members;
       Option[] options;
       Vote[] votes;
    }
}

