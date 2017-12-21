var HDWalletProvider = require("truffle-hdwallet-provider");
var mnemonic = "segment comfort coconut bone jeans pudding orchard leopard better daring atom still";
module.exports = {
  networks: {
    development: {
      provider: function() {
        return new HDWalletProvider(mnemonic, "https://ropsten.infura.io/<INFURA_Access_Token>")
      },
      network_id: 3,
      gas: 4700000
    } 
  }
};