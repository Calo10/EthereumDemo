using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using EthereumDemoHost;
using EthereumDemoHost.Controllers;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using Nethereum.Web3;

namespace EthereumDemoHost.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = (ViewResult)controller.Index();

            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            var expectedVersion = mvcName.Version.Major + "." + mvcName.Version.Minor;
            var expectedRuntime = isMono ? "Mono" : ".NET";

            // Assert
            Assert.AreEqual(expectedVersion, result.ViewData["Version"]);
            Assert.AreEqual(expectedRuntime, result.ViewData["Runtime"]);
        }

        [Test]
        public async Task TestContractConection()
        {
       
            var senderAddress = "";
            var password = "";
            var abi = "";
            var byteCode = "";

            var multiplier = 8;

            var web3 = new Web3.Web3();
            var ulockAccountResult = await web3.Personal.UloackAccount.SendRequestAsync(senderAddress,password,multiplier);

            Assert.True((ulockAccountResult));



        }
    }
}
