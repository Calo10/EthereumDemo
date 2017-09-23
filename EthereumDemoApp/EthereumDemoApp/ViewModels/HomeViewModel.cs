using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EthereumDemoApp.Views;

namespace EthereumDemoApp.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public HomeViewModel()
        {


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
