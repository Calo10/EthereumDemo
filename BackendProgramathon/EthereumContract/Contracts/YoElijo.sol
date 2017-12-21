contract YoElijo {
    
    enum VoteType { Papelete, Referendum, MultipleOption }
    enum VotationType { Private, Public }
    enum Advances { YES, NO}

    Proposal[] public proposals;
    uint public numProposals;
    mapping (address => uint) public memberId;
 
    event ProposalAdded(uint proposalID,string description);
    event Voted(string result);


    struct Proposal {
        uint proposalID;
        address owner;
        string description;
        uint votingDeadlineStart;
        uint votingDeadlineFinish;
        
        VoteType voteType;
        VotationType votationType;
        Advances advances;
        

        uint numberOfVotes;
        uint8 minimumQuantitySelected;
        uint8 maximumQuantitySelected;

        Member[] members;
        Option[] options;
        Vote[] votes;

        mapping (address => uint256) votesByUser;

        mapping (address => bool) voted;
    }

    struct Member {
        address member;
    }

    struct Option {
        string name;
        uint id;
        uint numberOfVotes;
    }
    
    struct Vote {
        address voter;
        uint optionId;
    }

    function YoElijo ()  payable {
        //Nothing to do in contructor
    }


    function newProposal(
        string description,
        uint startTime, //in Minutes
        uint finishTime, //in Minutes
        VoteType voteType, 
        VotationType votationType, 
        Advances advances,
        uint8 minimumQuantitySelected,
        uint8 maximumQuantitySelected
    )
        payable
        returns (uint proposalID)
    {
        proposalID = proposals.length++;
        Proposal storage p = proposals[proposalID];
        p.proposalID = proposalID;
        p.owner = msg.sender;
        p.description = description;
        p.votingDeadlineStart = now + startTime * 1 minutes;
        p.votingDeadlineFinish = now + finishTime * 1 minutes;
        p.voteType = voteType;
        p.votationType = votationType;
        p.advances = advances;
        p.numberOfVotes = 0;
        p.minimumQuantitySelected = minimumQuantitySelected;
        p.maximumQuantitySelected = maximumQuantitySelected;


        ProposalAdded(proposalID,description);
        numProposals = proposalID+1;


        return proposalID;
    }

    function updateProposal(
        uint proposalId,
        string description,
        uint startTime, //in Minutes
        uint finishTime, //in Minutes
        VotationType votationType, 
        Advances advances,
        uint8 minimumQuantitySelected,
        uint8 maximumQuantitySelected
    )
        payable
        returns (bool res)
    {
        
        Proposal storage p = proposals[proposalId];

        //The proposal can be modify only before it start.
        require(now < p.votingDeadlineStart );
        
        p.description = description;
        p.votingDeadlineStart = now + startTime * 1 minutes;
        p.votingDeadlineFinish = now + finishTime * 1 minutes;
        p.votationType = votationType;
        p.advances = advances;
        p.numberOfVotes = 0;
        p.minimumQuantitySelected = minimumQuantitySelected;
        p.maximumQuantitySelected = maximumQuantitySelected;
        ProposalAdded(proposalId,description);
      

        return true;
    }


    
    function addOptionToProposal( uint proposalID, string optionName) returns (uint optId) 
    {
        Proposal storage p = proposals[proposalID];
        
        //The proposal can be modify only before it start.
        //require(now < p.votingDeadlineStart );
        

        optId = p.options.length++;
        p.options[optId] = Option ({name: optionName, id : optId, numberOfVotes : 0});
        
        Voted(uintToString(optId));

        return optId;
        
    }

    function addMemberToProposal( uint proposalID, address memberAddress) returns (uint memberId) 
    {
        Proposal storage p = proposals[proposalID];
        
        //The proposal can be modify only before it start.
        require(now < p.votingDeadlineStart );

        memberId = p.members.length++;
        p.members[memberId] = Member ({member: memberAddress});
        
        Voted(uintToString(memberId));

        return memberId;
        
    }



    function addVoteToProposal(
        uint proposalId,
        uint optionId
    )
        payable
        returns (uint voteID)
    {
        Voted("Entra a votación");
        Proposal storage p = proposals[proposalId];         // Get the proposal
        Option storage o = p.options[optionId];

        //The proposal can be modify only before it start.
        require(now >= p.votingDeadlineStart );
        require(now <= p.votingDeadlineFinish );
        Voted("Votaciones abiertas");
        //require( p.votesByUser[msg.sender] < p.maximumQuantitySelected );
        Voted("Maxima cantidad de votos permitida");

        //uint voteType; //1 = Papeleta, 2 = Referendum, 3 = OpcionMultiple
        //uint votationType; //1= Private, 2 = Public
        p.votesByUser[msg.sender] += 1; 
        p.numberOfVotes++;
        o.numberOfVotes++;

        uint optId = 200;

        if (p.votationType == VotationType.Public) {
            //Save the option
            optId = optionId;  
        } 
        
        voteID = p.votes.length++;
        p.votes[voteID] = Vote ({voter: msg.sender, optionId : optId});
        
        Voted("true");  
              
        return voteID;
        
    }


    function addVoteToProposalv2(
        uint proposalId,
        uint optionId
    )
        payable
        returns (uint voteID)
    {
        Voted("Entra a votación");
       
        
        Proposal storage p = proposals[proposalId];         // Get the proposal
        Option storage o = p.options[optionId];


        Voted("Hoy: ");
        Voted(uintToString(now));
        Voted("Inicio: ");
        Voted(uintToString(p.votingDeadlineStart));
        Voted("Fin: ");
        Voted( uintToString(p.votingDeadlineFinish));
        //The proposal can be modify only before it start.
        if(now >= p.votingDeadlineStart && now <= p.votingDeadlineFinish){

            //require( p.votesByUser[msg.sender] < p.maximumQuantitySelected );
            Voted("Maxima cantidad de votos permitida");

            //uint voteType; //1 = Papeleta, 2 = Referendum, 3 = OpcionMultiple
            //uint votationType; //1= Private, 2 = Public
            p.votesByUser[msg.sender] += 1; 
            p.numberOfVotes++;
            o.numberOfVotes++;

            uint optId = 200;

            if (p.votationType == VotationType.Public) {
                //Save the option
                optId = optionId;  
            } 
            
            voteID = p.votes.length++;
            p.votes[voteID] = Vote ({voter: msg.sender, optionId : optId});
            
            Voted("true");  
        }else{
            Voted("No es posible realizar votación debido a que no se esta dentro del periodo establecido");
        
        }
      
              
        return voteID;
        
    }

    function proposalsCount() returns (uint d) 
    {
           return proposals.length;    
    }

    function proposalsVotes(uint proposalId ) returns (uint d) 
    {
        Proposal storage p = proposals[proposalId];         // Get the proposal
        return p.numberOfVotes;    
    }

    
    function proposalsoptionsCount(uint proposalId ) returns (uint d) 
    {
        Proposal storage p = proposals[proposalId];         // Get the proposal
        return p.options.length;    
    }


    function proposalsVotesByOption(uint proposalId, uint optionId ) returns (uint d) 
    {
        Proposal storage p = proposals[proposalId];         // Get the proposal
        Option storage o = p.options[optionId];

        return o.numberOfVotes;    
    }


    function getVoteinfo(uint proposalId, uint voteId ) returns (address voter, uint optionId) 
    {
        Proposal storage p = proposals[proposalId];         // Get the proposal
       
        if ( p.advances == Advances.YES || now > p.votingDeadlineFinish ) {
            //Permite ver avances o la fecha ya vencio
            Vote storage v = p.votes[voteId];

            return (v.voter,v.optionId);    
        } else {
            return (p.owner,300);
        }   
       
    }


    function getOptioninfo(uint proposalId, uint optionId ) returns (string name, uint numberOfVotes) 
    {
        Proposal storage p = proposals[proposalId];         // Get the proposal
        
        if ( p.advances ==  Advances.YES || now > p.votingDeadlineFinish ) {
            //Permite ver avances o la fecha ya vencio

            Option storage o = p.options[optionId];
 
            return (o.name,o.numberOfVotes);  
        } else {
            return ("", 1 );
        }  
    } 
    
    

    function uintToString(uint v) constant returns (string) {
          bytes32 ret;
            if (v == 0) {
                 ret = "0";
            }
            else {
                 while (v > 0) {
                      ret = bytes32(uint(ret) / (2 ** 8));
                      ret |= bytes32(((v % 10) + 48) * 2 ** (8 * 31));
                      v /= 10;
                 }
            }

            bytes memory bytesString = new bytes(32);
            for (uint j = 0; j < 32; j++) {
                 byte char = byte(bytes32(uint(ret) * 2 ** (8 * j)));
                 if (char != 0) {
                      bytesString[j] = char;
                 }
            }

            return string(bytesString);
      }
}
