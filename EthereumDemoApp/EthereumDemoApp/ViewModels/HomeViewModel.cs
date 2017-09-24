using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EthereumDemoApp.Views;
using EthereumDemoApp.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace EthereumDemoApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        #region  Instances
                
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }

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

        #endregion

        #region Commands

        public ICommand FilterProposalsCommand { get; private set; }


        #endregion

        public HomeViewModel()
        {
            initClass();
            initCommands();
        }

        #region Private Methods

        private void initClass()
        {
   
        }

        private void initCommands()
        {
            FilterProposalsCommand = new Command(FilterProposals);  
        }

        private void FilterProposals(object opt)
        {
            ((MasterDetailPage)App.Current.MainPage).Detail.Navigation.PushAsync(new VotesSelectionPage(Int32.Parse(opt.ToString())));
        }

       

        #endregion

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
