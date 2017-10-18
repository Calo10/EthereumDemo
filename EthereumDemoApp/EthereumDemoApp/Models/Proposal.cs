using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace EthereumDemoApp.Models
{
    public class Proposal : INotifyPropertyChanged
    {
        private bool _Checked { get; set; }
        public bool Checked { get { return _Checked; } set { _Checked = value; NotifyPropertyChanged("Checked"); } }

        public string ContracEthereumProposal { get; set; }
        public string ProposalName { get; set; }
        public string VotingOptions { get; set; }
        public int SecurityType { get; set; }
        public int QuestionType { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public int AdvancedSearch { get; set; }
        public string Description { get; set; }
        public ObservableCollection<Option> Options { get; set; }

        private string _status;
        public string Status 
        {
            get
            {
                return _status;
            }

            set 
            {
                if(InitialDate > DateTime.Now)
                {
                    _status = "Votacion aun no ha abierto.";
                }

                if(FinalDate < DateTime.Now)
                {
                    _status = "Votacion cerrada.";
                }
            } 
        
        }



        public ObservableCollection<Proposal>  SearhProposalByUser(string email,int type)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(API_Dictionary.ApiSearchProposalByUser);
                var json = JsonConvert.SerializeObject(new
                {
                    email = email,
                    QuestionType = type
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(uri, content).Result;
                string ans = response.Content.ReadAsStringAsync().Result;
                ObservableCollection<Proposal>  req = JsonConvert.DeserializeObject<ObservableCollection<Proposal>>(ans);
                return req;
            }
        }

        public bool ToVoted(string email, string contract, ObservableCollection<Option> lstoptions)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(API_Dictionary.ApiToVoted);
                var json = JsonConvert.SerializeObject(new
                {
                    Contract = Member.GetContract(User.GetInstance().Email),
                    email = User.GetInstance().Email,
                    ContracEthereumProposal = contract,
                    Options = lstoptions
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(uri, content).Result;
                string ans = response.Content.ReadAsStringAsync().Result;
                bool req = JsonConvert.DeserializeObject<bool>(ans);
                return req;
            }
        }

        public ObservableCollection<Option>  PartialResult(string contract)
        {
            using (HttpClient client = new HttpClient())
            {
                var uri = new Uri(API_Dictionary.ApiConsultPartialResult);
                var json = JsonConvert.SerializeObject(new
                {
                    ContracEthereumProposal = contract
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(uri, content).Result;
                string ans = response.Content.ReadAsStringAsync().Result;
                ObservableCollection<Option>  req = JsonConvert.DeserializeObject<ObservableCollection<Option>>(ans);
                return req;
            }
        }



        #region  Property Changes

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }
}

