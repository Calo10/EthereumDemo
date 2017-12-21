using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthereumWeb.Constants
{
    public class API_Dictionary
    {
        public const string Base = "http://40.76.22.76:5660/";
        public const string InsertProposal = "api/Ethereum/InsertProposal/?proposalName={0}&startTime={1}&finishTime={2}&voteType={3}&votationType={4}&advances={5}&minimumQuantitySelected={6}&maximumQuantitySelected={7}";
        public const string UpdateProposal = "api/Ethereum/UpdateProposal/?proposalId={0}&proposalName={1}&startTime={2}&finishTime={3}&votationType={4}&advances={5}&minimumQuantitySelected={6}&maximumQuantitySelected={7}";
        public const string InsertOptionProposal = "api/Ethereum/InsertOptionProposal/?proposalId={0}&OptionName={1}";
        public const string SearchOptionProposal = "api/Ethereum/getVotes/?ProposalId={0}";
        public const string SearchResultProposal = "api/Ethereum/getResults/?ProposalId={0}";
        public const string AddVoteToProposal = "api/Ethereum/AddVoteToProposal/?proposalId={0}&optionId={1}&userSenderAddress={2}&email={3}";
        public const string CreateAccount = "api/Ethereum/CreateAccount/";
    }
}