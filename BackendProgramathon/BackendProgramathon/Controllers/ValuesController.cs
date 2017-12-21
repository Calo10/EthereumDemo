using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using Nethereum.Web3.TransactionReceipts;
using System.Threading;
using Nethereum.RPC.Eth.DTOs;
using System.Numerics;
using Nethereum.Geth;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace BackendProgramathon.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()
        {

            //var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            //var password = "password";
            //var abi =
            //    @"[{""constant"":false,""inputs"":[{""name"":""val"",""type"":""int256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""int256""}],""type"":""function""},{""inputs"":[{""name"":""multiplier"",""type"":""int256""}],""type"":""constructor""}]";
            //var byteCode =
            //    "0x60606040526040516020806052833950608060405251600081905550602b8060276000396000f3606060405260e060020a60003504631df4f1448114601a575b005b600054600435026060908152602090f3";

            //var multiplier = 7;

            //var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));

            //var transactionPolling = new TransactionReceiptPollingService(web3);

            ////var miningResult =  web3.Miner.Start.SendRequestAsync(6);
            ////assumed client is mining already
            //var contractAddress = 
            //    transactionPolling.DeployContractAndGetAddressAsync(
            //        () =>
            //            web3.Eth.DeployContract.SendRequestAsync(abi, byteCode, senderAddress, new HexBigInteger(900000),
            //                multiplier)
            //    ).Result;
            ////var contractAddress = "0x6c498f0f83d0bbec758ee7f23e13c9ee522a4c8f";
            //var contract = web3.Eth.GetContract(abi, contractAddress);

            //var multiplyFunction = contract.GetFunction("multiply");

            //var result =  multiplyFunction.CallAsync<int>(7).Result;

            // return result.ToString() + " " + contractAddress.ToString();
            return "";
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            try
            {
                var privateKey = "0x6ab37b464dbc10fd61f304f14112c30096cef4a9bbfeff9fcbc6468f06348859";

                // var account = new Account(privateKey);


                var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
                var password = "password";



                var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));

                var transactionPolling = new TransactionReceiptPollingService(web3);

                var contractAddress = "0x3b01809c8f10ae35ff2d38e3e29d7456cc442f48";
                var contract = web3.Eth.GetContract(abi, contractAddress);

                var multiplyFunction = contract.GetFunction("multiply");

                var result = multiplyFunction.CallAsync<int>(id).Result;

                var proposalsCountFunc = contract.GetFunction("proposalsCount");
                var resultPropCount = proposalsCountFunc.CallAsync<int>().Result;


                var newProposalFunction = contract.GetFunction("newProposal");

                var initialBalance = web3.Eth.GetBalance.SendRequestAsync(senderAddress).Result;

                //var unlockResult =  web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, new HexBigInteger(120)).Result;
                var unlockResult = web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, 60).Result;

                //var tranHas = newProposalFunction.SendTransactionAsync(account.Address,senderAddress, 10,"New Job description").Result;

                //var transactionReceipt =  transactionPolling.SendRequestAsync(() =>
                //    newProposalFunction.SendTransactionAsync(senderAddress, senderAddress, 10, "New Job description")
                //);


                //var transactionReceipt =  MineAndGetReceiptAsync(web3, tranHas).Result;
                //int i = 0;
                //while (transactionReceipt.Result == null && i < 10)
                //{
                //    Thread.Sleep(1000);
                //    i++;
                //}

                web3.TransactionManager.DefaultGas = BigInteger.Parse("290000");
                web3.TransactionManager.DefaultGasPrice = BigInteger.Parse("290000");

                var resultPro = newProposalFunction.SendTransactionAsync(senderAddress, senderAddress, 10, "New Job description").Result;

                var resultTran = MineAndGetReceiptAsync(web3, resultPro).Result;


                var newProposalTestFunction = contract.GetFunction("newProposalTest");
                var resultPropTest = newProposalTestFunction.CallAsync<int>(senderAddress, 10, "New Job description").Result;


                var newProposalTest3Function = contract.GetFunction("newProposalTest3");
                var resultPropTest3 = newProposalTest3Function.CallAsync<string>("New Job description").Result;


                return result.ToString() + " " + contractAddress.ToString() + "----- New Proposal Id= " + resultPro.ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public async Task<TransactionReceipt> MineAndGetReceiptAsync(Nethereum.Web3.Web3 web3, string transactionHash)
        {
            var web3Geth = new Web3Geth();

            var miningResult = await web3Geth.Miner.Start.SendRequestAsync(6);

            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            }

            miningResult = await web3Geth.Miner.Stop.SendRequestAsync();

            return receipt;
        }

        [FunctionOutput]
        public class Document
        {
            [Parameter("int", "amount", 1)]
            public string Amount { get; set; }

            [Parameter("string", "description", 2)]
            public string Description { get; set; }

            [Parameter("int", "votingDeadline", 3)]
            public string VotingDeadline { get; set; }

            [Parameter("bool", "executed", 4)]
            public string Executed { get; set; }

            [Parameter("bool", "proposalPassed", 5)]
            public string ProposalPassed { get; set; }

            [Parameter("int", "numberOfVotes", 6)]
            public string NumberOfVotes { get; set; }

            [Parameter("int", "currentResult", 7)]
            public string CurrentResult { get; set; }

          
        }
        
        public string abi =
                  @"[
    {
      ""constant"": true,
      ""inputs"": [
        {
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""proposals"",
      ""outputs"": [
        {
          ""name"": ""recipient"",
          ""type"": ""address""
        },
        {
          ""name"": ""amount"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""description"",
          ""type"": ""string""
        },
        {
          ""name"": ""votingDeadline"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""executed"",
          ""type"": ""bool""
        },
        {
          ""name"": ""proposalPassed"",
          ""type"": ""bool""
        },
        {
          ""name"": ""numberOfVotes"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""currentResult"",
          ""type"": ""int256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [],
      ""name"": ""proposalsCount"",
      ""outputs"": [
        {
          ""name"": ""d"",
          ""type"": ""uint256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""beneficiary"",
          ""type"": ""address""
        },
        {
          ""name"": ""weiAmount"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""jobDescription"",
          ""type"": ""string""
        }
      ],
      ""name"": ""newProposalTest"",
      ""outputs"": [
        {
          ""name"": ""d"",
          ""type"": ""uint256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""val"",
          ""type"": ""int256""
        }
      ],
      ""name"": ""multiply"",
      ""outputs"": [
        {
          ""name"": ""d"",
          ""type"": ""int256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": true,
      ""inputs"": [
        {
          ""name"": """",
          ""type"": ""address""
        }
      ],
      ""name"": ""memberId"",
      ""outputs"": [
        {
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": true,
      ""inputs"": [],
      ""name"": ""numProposals"",
      ""outputs"": [
        {
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": true,
      ""inputs"": [
        {
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""members"",
      ""outputs"": [
        {
          ""name"": ""member"",
          ""type"": ""address""
        },
        {
          ""name"": ""name"",
          ""type"": ""string""
        },
        {
          ""name"": ""memberSince"",
          ""type"": ""uint256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": true,
      ""inputs"": [],
      ""name"": ""debatingPeriodInMinutes"",
      ""outputs"": [
        {
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": true,
      ""inputs"": [],
      ""name"": ""minimumQuorum"",
      ""outputs"": [
        {
          ""name"": """",
          ""type"": ""uint256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": true,
      ""inputs"": [],
      ""name"": ""owner"",
      ""outputs"": [
        {
          ""name"": """",
          ""type"": ""address""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""multiplier"",
          ""type"": ""int256""
        }
      ],
      ""name"": ""test"",
      ""outputs"": [],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": true,
      ""inputs"": [],
      ""name"": ""majorityMargin"",
      ""outputs"": [
        {
          ""name"": """",
          ""type"": ""int256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""targetMember"",
          ""type"": ""address""
        },
        {
          ""name"": ""memberName"",
          ""type"": ""string""
        }
      ],
      ""name"": ""addMember"",
      ""outputs"": [],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""proposalNumber"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""supportsProposal"",
          ""type"": ""bool""
        },
        {
          ""name"": ""justificationText"",
          ""type"": ""string""
        }
      ],
      ""name"": ""vote"",
      ""outputs"": [
        {
          ""name"": ""voteID"",
          ""type"": ""uint256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""beneficiary"",
          ""type"": ""address""
        },
        {
          ""name"": ""weiAmount"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""jobDescription"",
          ""type"": ""string""
        }
      ],
      ""name"": ""newProposalTest2"",
      ""outputs"": [
        {
          ""name"": ""d"",
          ""type"": ""string""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""beneficiary"",
          ""type"": ""address""
        },
        {
          ""name"": ""weiAmount"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""jobDescription"",
          ""type"": ""string""
        }
      ],
      ""name"": ""newProposal"",
      ""outputs"": [
        {
          ""name"": ""proposalID"",
          ""type"": ""uint256""
        }
      ],
      ""payable"": true,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""jobDescription"",
          ""type"": ""string""
        }
      ],
      ""name"": ""newProposalTest3"",
      ""outputs"": [
        {
          ""name"": ""d"",
          ""type"": ""string""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""payable"": true,
      ""type"": ""constructor""
    }
  ]";

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
