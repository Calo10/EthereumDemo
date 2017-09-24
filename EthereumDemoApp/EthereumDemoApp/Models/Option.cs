using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace EthereumDemoApp.Models
{
    public class Option : INotifyPropertyChanged
    {
        public int IdOption { get; set; }
        public string Description { get; set; }
        public long NumberOfVotes { get; set; }

        private bool _Checked { get; set; }
        public bool Checked { get { return _Checked; } set { _Checked = value; NotifyPropertyChanged("Checked"); } }

        public Option()
        {
           
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
