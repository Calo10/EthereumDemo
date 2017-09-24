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
    public class BallotViewModel : INotifyPropertyChanged
    {

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

        #region Singleton
        private static BallotViewModel instance = null;

        private BallotViewModel()
        {

           
        }

        public static BallotViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new BallotViewModel();
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
