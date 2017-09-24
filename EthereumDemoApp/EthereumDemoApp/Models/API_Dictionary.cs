using System;

using Xamarin.Forms;

namespace EthereumDemoApp.Models
{
    public static class API_Dictionary 
    {


        public const string Base = "http://13.82.232.153/";

        public const string ApiLogin =  Base + "EthereumWeb/Api/Security/login";

        public const string ApiSearchProposalByUser = Base + "EthereumWeb/Api/ProposalAPI/SearchProposalByUser";

        public const string ApiToVoted = Base + "EthereumWeb/Api/ProposalAPI/DoVote";

        public const string ApiConsultPartialResult = Base + "EthereumWeb/Api/ProposalAPI/ConsultPartialResults";

    }
}

