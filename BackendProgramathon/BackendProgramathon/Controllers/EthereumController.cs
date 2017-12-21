using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BackendProgramathon.Entities;

using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using Nethereum.Web3.TransactionReceipts;
using System.Threading;
using Nethereum.RPC.Eth.DTOs;
using System.Numerics;
using Nethereum.Geth;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.KeyStore;
using Nethereum.Hex.HexConvertors.Extensions;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendProgramathon.Controllers
{
    public class EthereumController : Controller
    {
        private readonly MyOptions _appSettings;

        public EthereumController(IOptions<MyOptions> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public List<Vote> getVotes(int proposalId)
        {
            List<Vote> votes = new List<Vote>();

            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);

            var FuncLenth = contract.GetFunction("proposalsVotes");
            var Func = contract.GetFunction("getVoteinfo");

            var result = FuncLenth.CallAsync<int>(proposalId).Result;

            for (int i = 0; i < result; i++)
            {
                var myVote = Func.CallDeserializingToObjectAsync<Vote>(proposalId, i).Result;
                votes.Add(myVote);


                if (myVote.OptionId == 300)
                {
                    i = result; //out
                    votes.Clear();
                }

            }

            return votes;
        }

        [HttpGet]
        public List<Option> getResults(int proposalId)
        {
            List<Option> options = new List<Option>();

            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);

            var FuncLenth = contract.GetFunction("proposalsoptionsCount");
            var Func = contract.GetFunction("getOptioninfo");

            var result = FuncLenth.CallAsync<int>(proposalId).Result;

            for (int i = 0; i < result; i++)
            {
                var opt = Func.CallDeserializingToObjectAsync<Option>(proposalId, i).Result;
                //   options.Add(opt);

                if (!string.IsNullOrEmpty(opt.Name))
                {

                    options.Add(opt);
                    //i = result; //out
                    //options.Clear();
                }

            }

            return options;
        }

        [HttpGet]
        public string ProposalsCount()
        {
            //var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            //var password = "password";


            var senderAddress = _appSettings.AccountNumber;
            var password = _appSettings.Password;

            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);


            var Func = contract.GetFunction("proposalsCount");
            var result = Func.CallAsync<int>().Result;

            return result.ToString();
        }

        [HttpGet]
        public string ProposalsVoutingCount(int proprosalId)
        {
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);


            var Func = contract.GetFunction("proposalsVotes");
            var result = Func.CallAsync<int>(0).Result;

            return result.ToString();
        }


        [HttpGet]
        public string ProposalsOptionsCount(int proprosalId)
        {
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);


            var Func = contract.GetFunction("proposalsoptionsCount");
            var result = Func.CallAsync<int>(proprosalId).Result;

            return result.ToString();
        }





        [HttpGet]
        public string InsertProposal(string proposalName, int startTime, int finishTime, int voteType, int votationType, int advances, int minimumQuantitySelected, int maximumQuantitySelected)
        {
            voteType -= 1;
            advances -= 1;
            votationType -= 1;

            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);

            var Func = contract.GetFunction("newProposal");
            var Event = contract.GetEvent("ProposalAdded");
            var filterAll = Event.CreateFilterAsync().Result;

            var unlockResult = web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, 60).Result;

            web3.TransactionManager.DefaultGas = BigInteger.Parse("290000");
            web3.TransactionManager.DefaultGasPrice = BigInteger.Parse("290000");

            var resultPro = Func.SendTransactionAsync(senderAddress, proposalName, startTime, finishTime, voteType, votationType, advances, minimumQuantitySelected, 10).Result;
            var resultTran = MineAndGetReceiptAsync(web3, resultPro).Result;


            var log = Event.GetFilterChanges<ProposalAddedEvent>(filterAll).Result;


            return log[0].Event.proposalID.ToString();
        }


        [HttpGet]
        public string UpdateProposal(int proposalId, string proposalName, int startTime, int finishTime, int votationType, int advances, int minimumQuantitySelected, int maximumQuantitySelected)
        {
            advances -= 1;
            votationType -= 1;


            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);

            var Func = contract.GetFunction("updateProposal");
            var Event = contract.GetEvent("ProposalAdded");
            var filterAll = Event.CreateFilterAsync().Result;

            var unlockResult = web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, 60).Result;

            web3.TransactionManager.DefaultGas = BigInteger.Parse("290000");
            web3.TransactionManager.DefaultGasPrice = BigInteger.Parse("290000");

            var resultPro = Func.SendTransactionAsync(senderAddress, proposalId, proposalName, startTime, finishTime, votationType, advances, minimumQuantitySelected, maximumQuantitySelected).Result;
            var resultTran = MineAndGetReceiptAsync(web3, resultPro).Result;


            var log = Event.GetFilterChanges<ProposalAddedEvent>(filterAll).Result;


            return log[0].Event.proposalID.ToString();
        }



        [HttpGet]
        public string InsertOptionProposal(int proposalId, string optionName)
        {
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);

            var Func = contract.GetFunction("addOptionToProposal");
            var Event = contract.GetEvent("Voted");
            var filterAll = Event.CreateFilterAsync().Result;

            var unlockResult = web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, 60).Result;

            web3.TransactionManager.DefaultGas = BigInteger.Parse("290000");
            web3.TransactionManager.DefaultGasPrice = BigInteger.Parse("290000");

            var resultPro = Func.SendTransactionAsync(senderAddress, proposalId, optionName).Result;
            var resultTran = MineAndGetReceiptAsync(web3, resultPro).Result;


            var log = Event.GetFilterChanges<VotedEvent>(filterAll).Result;



            return log[0].Event.Result.ToString();
        }

        [HttpGet]
        public string InsertMemberProposal(int proposalId, string userSenderAddress, string email)
        {
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);

            var Func = contract.GetFunction("addMemberToProposal");
            var Event = contract.GetEvent("Voted");
            var filterAll = Event.CreateFilterAsync().Result;

            var unlockResult = web3.Personal.UnlockAccount.SendRequestAsync(senderAddress, password, 60).Result;

            web3.TransactionManager.DefaultGas = BigInteger.Parse("290000");
            web3.TransactionManager.DefaultGasPrice = BigInteger.Parse("290000");

            var resultPro = Func.SendTransactionAsync(senderAddress, proposalId, userSenderAddress, email).Result;
            var resultTran = MineAndGetReceiptAsync(web3, resultPro).Result;


            var log = Event.GetFilterChanges<VotedEvent>(filterAll).Result;



            return log[0].Event.Result.ToString();
        }


        [HttpGet]
        public string AddVoteToProposal(int proposalId, int optionId, string userSenderAddress)
        {
            //userSenderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(userSenderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);

            var Func = contract.GetFunction("addVoteToProposal");
            var Event = contract.GetEvent("Voted");
            var filterAll = Event.CreateFilterAsync().Result;

            var unlockResult = web3.Personal.UnlockAccount.SendRequestAsync(userSenderAddress, password, 60).Result;

            web3.TransactionManager.DefaultGas = BigInteger.Parse("4000000");
            web3.TransactionManager.DefaultGasPrice = BigInteger.Parse("4000000");

            var resultPro = Func.SendTransactionAsync(userSenderAddress, proposalId, optionId).Result;
            var resultTran = MineAndGetReceiptAsync(web3, resultPro).Result;


            var log = Event.GetFilterChanges<VotedEvent>(filterAll).Result;

            if (log.Count > 0)
            {
                string message = string.Empty;
                for (int i = 0; i < log.Count; i++)
                {
                    message += log[i].Event.Result.ToString();
                    message += " / ";
                }
                return message;

            }
            else
                return "No hay respuesta";
        }

        [HttpGet]
        public string AddVoteToProposalv2(int proposalId, int optionId, string userSenderAddress)
        {
            //userSenderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var password = "password";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(userSenderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);



            var contract = web3.Eth.GetContract(ABI, CONTRACTADDRESS);

            var Func = contract.GetFunction("addVoteToProposalv2");
            var Event = contract.GetEvent("Voted");
            var filterAll = Event.CreateFilterAsync().Result;

            var unlockResult = web3.Personal.UnlockAccount.SendRequestAsync(userSenderAddress, password, 60).Result;

            web3.TransactionManager.DefaultGas = BigInteger.Parse("4000000");
            web3.TransactionManager.DefaultGasPrice = BigInteger.Parse("4000000");

            var resultPro = Func.SendTransactionAsync(userSenderAddress, proposalId, optionId).Result;
            var resultTran = MineAndGetReceiptAsync(web3, resultPro).Result;


            var log = Event.GetFilterChanges<VotedEvent>(filterAll).Result;



            if (log.Count > 0)
            {
                string message = string.Empty;
                for (int i = 0; i < log.Count; i++)
                {
                    message += log[i].Event.Result.ToString();
                    message += " / ";
                }
                return message;
            }
            else
                return "No hay respuesta";
        }

        [HttpGet]
        public ContractDemo CreateAccount()
        {
            //To full the new account
            var password = "password";
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var web3 = new Nethereum.Web3.Web3(new ManagedAccount(senderAddress, password));
            var transactionPolling = new TransactionReceiptPollingService(web3);
            string path = @"C:\Programathon\Nethereum-master\testchain\devChain\keystore\";

            //Generate a private key pair using SecureRandom
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            //Get the public address (derivied from the public key)
            var newAddress = ecKey.GetPublicAddress();
            var privateKey = ecKey.GetPrivateKey();

            //Create a store service, to encrypt and save the file using the web3 standard
            var service = new KeyStoreService();
            var encryptedKey = service.EncryptAndGenerateDefaultKeyStoreAsJson(password, ecKey.GetPrivateKeyAsBytes(), newAddress);
            var fileName = service.GenerateUTCFileName(newAddress);
            //save the File

            using (var newfile = System.IO.File.CreateText(Path.Combine(path, fileName)))
            {
                newfile.Write(encryptedKey);
                newfile.Flush();
            }


            var web3Geth = new Web3Geth();
            var miningResult = web3Geth.Miner.Start.SendRequestAsync(6).Result;

            var currentBalance = web3.Eth.GetBalance.SendRequestAsync(newAddress).Result;
            //when sending a transaction using an Account, a raw transaction is signed and send using the private key

            var transactionReceipt = transactionPolling.SendRequestAsync(() =>
              web3.TransactionManager.SendTransactionAsync(senderAddress, newAddress, new HexBigInteger(4000000000000000000))
            ).Result;


            var newBalance = web3.Eth.GetBalance.SendRequestAsync(newAddress).Result;

            miningResult = web3Geth.Miner.Stop.SendRequestAsync().Result;


            ContractDemo d = new ContractDemo();
            d.Address = newAddress;
            return d;
        }
        public class ContractDemo
        {
            public string Address { set; get; }
        }
        public class ProposalAddedEvent
        {


            [Parameter("int", "proposalID", 1, true)]
            public int proposalID { get; set; }

            [Parameter("string", "description", 2, true)]
            public string description { get; set; }


        }

        public class VotedEvent
        {


            [Parameter("string", "result", 1, true)]
            public string Result { get; set; }



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

        public string CONTRACTADDRESS = "0xd0828aeb00e4db6813e2f330318ef94d2bba2f60";


        public string ABI = @"[
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
          ""name"": ""proposalID"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""owner"",
          ""type"": ""address""
        },
        {
          ""name"": ""description"",
          ""type"": ""string""
        },
        {
          ""name"": ""votingDeadlineStart"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""votingDeadlineFinish"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""voteType"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""votationType"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""advances"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""numberOfVotes"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""minimumQuantitySelected"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""maximumQuantitySelected"",
          ""type"": ""uint8""
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
          ""name"": ""proposalId"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""optionId"",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""addVoteToProposalv2"",
      ""outputs"": [
        {
          ""name"": ""voteID"",
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
          ""name"": ""proposalId"",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""proposalsoptionsCount"",
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
          ""name"": ""proposalID"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""optionName"",
          ""type"": ""string""
        }
      ],
      ""name"": ""addOptionToProposal"",
      ""outputs"": [
        {
          ""name"": ""optId"",
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
          ""name"": ""proposalId"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""optionId"",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""addVoteToProposal"",
      ""outputs"": [
        {
          ""name"": ""voteID"",
          ""type"": ""uint256""
        }
      ],
      ""payable"": true,
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
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""proposalId"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""optionId"",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""proposalsVotesByOption"",
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
          ""name"": ""proposalId"",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""proposalsVotes"",
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
          ""name"": ""proposalId"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""description"",
          ""type"": ""string""
        },
        {
          ""name"": ""startTime"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""finishTime"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""votationType"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""advances"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""minimumQuantitySelected"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""maximumQuantitySelected"",
          ""type"": ""uint8""
        }
      ],
      ""name"": ""updateProposal"",
      ""outputs"": [
        {
          ""name"": ""res"",
          ""type"": ""bool""
        }
      ],
      ""payable"": true,
      ""type"": ""function""
    },
    {
      ""constant"": false,
      ""inputs"": [
        {
          ""name"": ""proposalId"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""optionId"",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""getOptioninfo"",
      ""outputs"": [
        {
          ""name"": ""name"",
          ""type"": ""string""
        },
        {
          ""name"": ""numberOfVotes"",
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
          ""name"": ""description"",
          ""type"": ""string""
        },
        {
          ""name"": ""startTime"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""finishTime"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""voteType"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""votationType"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""advances"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""minimumQuantitySelected"",
          ""type"": ""uint8""
        },
        {
          ""name"": ""maximumQuantitySelected"",
          ""type"": ""uint8""
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
          ""name"": ""proposalID"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""memberAddress"",
          ""type"": ""address""
        }
      ],
      ""name"": ""addMemberToProposal"",
      ""outputs"": [
        {
          ""name"": ""memberId"",
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
          ""name"": ""v"",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""uintToString"",
      ""outputs"": [
        {
          ""name"": """",
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
          ""name"": ""proposalId"",
          ""type"": ""uint256""
        },
        {
          ""name"": ""voteId"",
          ""type"": ""uint256""
        }
      ],
      ""name"": ""getVoteinfo"",
      ""outputs"": [
        {
          ""name"": ""voter"",
          ""type"": ""address""
        },
        {
          ""name"": ""optionId"",
          ""type"": ""uint256""
        }
      ],
      ""payable"": false,
      ""type"": ""function""
    },
    {
      ""inputs"": [],
      ""payable"": true,
      ""type"": ""constructor""
    },
    {
      ""anonymous"": false,
      ""inputs"": [
        {
          ""indexed"": false,
          ""name"": ""proposalID"",
          ""type"": ""uint256""
        },
        {
          ""indexed"": false,
          ""name"": ""description"",
          ""type"": ""string""
        }
      ],
      ""name"": ""ProposalAdded"",
      ""type"": ""event""
    },
    {
      ""anonymous"": false,
      ""inputs"": [
        {
          ""indexed"": false,
          ""name"": ""result"",
          ""type"": ""string""
        }
      ],
      ""name"": ""Voted"",
      ""type"": ""event""
    }
  ]";


    }
}
