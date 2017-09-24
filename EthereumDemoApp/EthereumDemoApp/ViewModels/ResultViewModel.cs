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
    public class ResultViewModel : INotifyPropertyChanged
    {

        #region Singleton
        private static ResultViewModel instance = null;

        private ResultViewModel()
        {


        }

        public static ResultViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new ResultViewModel();
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
         

        private ObservableCollection<Proposal> _lstResults = new ObservableCollection<Proposal>();
        public ObservableCollection<Proposal> lstResults
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
