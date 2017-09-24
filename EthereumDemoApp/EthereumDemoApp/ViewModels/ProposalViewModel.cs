using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EthereumDemoApp.Views;
using EthereumDemoApp.Models;
using Acr.UserDialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace EthereumDemoApp.ViewModels
{
    public class ProposalViewModel : INotifyPropertyChanged
    {

        #region Singleton
        private static ProposalViewModel instance = null;

        private ProposalViewModel(int? type)
        {

           
                InitCommand();
                //filtrar
                Proposal proposalM = new Proposal();

                ObservableCollection<Proposal> proposal = proposalM.SearhProposalByUser("raykel18@gmail.com", Int32.Parse(type.ToString()));
                lstProposals = proposal;

                //setOptions();


        }

        public static ProposalViewModel GetInstance(int? type)
        {
            if (instance == null)
            {
                instance = new ProposalViewModel(type);
            }
           
            return instance;
        }

        public static void DeleteInstance()
        {
            if (instance != null)
            {
                instance = null;
            }
        }
        #endregion
         
        private ObservableCollection<Proposal> _lstProposals = new ObservableCollection<Proposal>();
        public ObservableCollection<Proposal> lstProposals
        {
            get
            {
                return _lstProposals;
            }
            set
            {
                _lstProposals = value;
                NotifyPropertyChanged("lstProposals");
            }
        }

        private ObservableCollection<Option> _lstProposalsOptions = new ObservableCollection<Option>();
        public ObservableCollection<Option> lstProposalsOptions
        {
            get
            {
                return _lstProposalsOptions;
            }
            set
            {
                _lstProposalsOptions = value;
                NotifyPropertyChanged("lstProposalsOptions");
            }
        }

        private ObservableCollection<Option> _lstResults = new ObservableCollection<Option>();
        public ObservableCollection<Option> lstResults
        {
            get
            {
                return _lstResults;
            }
            set
            {
                _lstResults = value;
                NotifyPropertyChanged("lstResults");
            }
        }

        private string _RefSelected { get; set; }

        public string RefSelected
        {
            get
            {
                return _RefSelected;
            }
            set
            {
                _RefSelected = value;
                NotifyPropertyChanged("RefSelected");
            }
        }


        public ICommand SelectProposalCommand { get; set; }
        public ICommand SendVotationCommand { get; set; }
        public ICommand SetReferendumCommand { get; set; }
        public ICommand SelectBallotCommand { get; set; }

        private void setOptions()
        {

            foreach (Proposal item in lstProposals.Where(x => x.Checked == true).ToList())
            {
                foreach (var item2 in item.Options)
                {
                    lstProposalsOptions.Add(item2);
                }

            }
        }


        private void SelectProposal(object opc)
        {
            
            lstProposalsOptions = new ObservableCollection<Option>();

            Proposal prop = (Proposal)opc;

            foreach (var item in lstProposals.Where(x=> x.ContracEthereumProposal != prop.ContracEthereumProposal))
            {
                item.Checked = false;
            }

            lstProposals.Where(x => x.ContracEthereumProposal == prop.ContracEthereumProposal).FirstOrDefault().Checked = true;

            setOptions();

            FilterProposalsOptions(prop.TypeVoting);

        }

        private void SendVotation(object type)
        {

            Proposal proposalM = new Proposal();
            bool res = false;
            ObservableCollection<Option> lstOpt = new ObservableCollection<Option>();
            Proposal prop = lstProposals.Where(x => x.Checked == true).FirstOrDefault();

            switch (Int32.Parse(type.ToString()))
            {

                case 1:
         
                    foreach (Option item in lstProposalsOptions.Where(x => x.Checked == true))
                    {
                        lstOpt.Add(item);
                    }

                    res = proposalM.ToVoted("raykel18@gmail.com", prop.ContracEthereumProposal, lstOpt);

                    if (res)
                    {
                        //UserDialogs.Instance.Alert("Voto Registrado Correctamente", "Mensaje", "Aceptar");

                        if(prop.AdvancedSearch == 1)
                        {
                            lstResults = proposalM.PartialResult(prop.ContracEthereumProposal);

                            ((MasterDetailPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ResultPage());

                        }
                        else
                        {
                            UserDialogs.Instance.Alert("Voto Registrado", "Mensaje", "Aceptar");
                        }

                    }
                    else
                    {
                        UserDialogs.Instance.Alert("ERROR al votar!", "Mensaje", "Aceptar");
                    }

                    break;

                case 2:

                    if (RefSelected == "SI")
                    {
                        lstProposalsOptions.Where(x=> x.Description == "SI").FirstOrDefault().Checked = true;
                    }
                    else
                    {
                        lstProposalsOptions.Where(x => x.Description == "NO").FirstOrDefault().Checked = true;
                    }


                    foreach (Option item in lstProposalsOptions.Where(x => x.Checked == true))
                    {
                        lstOpt.Add(item);
                    }


                    res = proposalM.ToVoted("raykel18@gmail.com", prop.ContracEthereumProposal, lstOpt);

                    if(res)
                    {
                        if (prop.AdvancedSearch == 1)
                        {
                            lstResults = proposalM.PartialResult(prop.ContracEthereumProposal);

                            ((MasterDetailPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ResultPage());

                        }
                        else
                        {
                            UserDialogs.Instance.Alert("Voto Registrado", "Mensaje", "Aceptar");
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("ERROR al votar!", "Mensaje", "Aceptar");
                    }
                        

                    break;

                case 3:

                    foreach (Option item in lstProposalsOptions.Where(x => x.Checked == true))
                    {
                        lstOpt.Add(item);
                    }

                    res = proposalM.ToVoted("raykel18@gmail.com", prop.ContracEthereumProposal, lstOpt);

                    if (res)
                    {
                        if (prop.AdvancedSearch == 1)
                        {
                            lstResults = proposalM.PartialResult(prop.ContracEthereumProposal);

                            ((MasterDetailPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ResultPage());

                        }
                        else
                        {
                            UserDialogs.Instance.Alert("Voto Registrado", "Mensaje", "Aceptar");
                        }

                    }
                    else
                    {
                        UserDialogs.Instance.Alert("ERROR al votar!", "Mensaje", "Aceptar");
                    }

                    break;

                default:
                    break;
            }

        }

        private void SetReferendum(object opc)
        {
            RefSelected = opc.ToString();

        }

        private void SelectBallot(Object opc)
        {
            foreach (var item in lstProposalsOptions.Where(x=> x.Checked == true))
            {
                item.Checked = false;
            }

            lstProposalsOptions.Where(x => x.IdOption == Int32.Parse(opc.ToString())).FirstOrDefault().Checked = true;

        }
        private void InitCommand()
        {
            SelectProposalCommand = new Command(SelectProposal);
            SendVotationCommand = new Command(SendVotation);
            SetReferendumCommand = new Command(SetReferendum);
            SelectBallotCommand = new Command(SelectBallot);
        }

        private void FilterProposalsOptions(object opt)
        {

            switch (Int32.Parse(opt.ToString()))
            {
                case 1:

                    ((MasterDetailPage)App.Current.MainPage).Detail.Navigation.PushAsync(new BallotPage());
                    break;

                case 2:

                    ((MasterDetailPage)App.Current.MainPage).Detail.Navigation.PushAsync(new ReferendumPage());
                    break;

                case 3:

                    ((MasterDetailPage)App.Current.MainPage).Detail.Navigation.PushAsync(new MultiplePage());
                    break;


                default:
                    break;
            }

        }

        #region  Property Change

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

