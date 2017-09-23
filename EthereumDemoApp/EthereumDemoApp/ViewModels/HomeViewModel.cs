using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EthereumDemoApp.Views;
using EthereumDemoApp.Models;
using System.Windows.Input;

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
                OnPropertyChanged("IsBusy");
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
                OnPropertyChanged("lstProposals");
            }
        }

        #endregion

        #region Commands

        public ICommand FilterByBallotCommand { get; private set; }

        public ICommand FilterByReferendumCommand { get; private set; }

        public ICommand FilterByMultipleOptionCommand { get; private set; }

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
            FilterByBallotCommand = new Command(FilterByBallot); 
            FilterByReferendumCommand = new Command(FilterByReferendum); 
            FilterByMultipleOptionCommand = new Command(FilterByMultipleOption);   
        }

        private void FilterByBallot()
        {

        }
        
        private void FilterByReferendum()
        {
 
        }

        private void FilterByMultipleOption()
        {

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
