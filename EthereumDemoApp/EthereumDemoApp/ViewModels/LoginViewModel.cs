using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using EthereumDemoApp.Views;
using EthereumDemoApp.Models;
using Acr.UserDialogs;

namespace EthereumDemoApp.ViewModels
{

    public class LoginViewModel : INotifyPropertyChanged
    {

        private string _email { get; set; }

        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                NotifyPropertyChanged("email");
            }
        }

        private string _pass { get; set; }

        public string pass
        {
            get
            {
                return _pass;
            }
            set
            {
                _pass = value;
                NotifyPropertyChanged("pass");
            }
        }


        public ICommand LoginCommand { get; set; }
        public ICommand EnterTakePictureCommand { get; set; }

        Login loginM = new Login();
        
        public LoginViewModel()
        {
            InitComands();
        }

        public void Login()
        {
            
            Member member = loginM.ExecuteLogin(email,pass);

            if(member != null){

                NavigationPage navigation = new NavigationPage(new HomeViewDetail());
                navigation.Popped += Popped;
                navigation.PoppedToRoot += PoppedToRoot;

                App.Current.MainPage = new MasterDetailPage
                {
                    Master = new HomeViewMaster(),
                    Detail = navigation
                };


            }
            else
            {

                UserDialogs.Instance.Alert("No se encontro el Usuario", "Mensaje", "Aceptar");
            }
        }

        private void PoppedToRoot(object sender, NavigationEventArgs e)
        {
            ControlAplication();
        }

        private void Popped(object sender, NavigationEventArgs e)
        {
            if (((NavigationPage)((MasterDetailPage)App.Current.MainPage).Detail).CurrentPage.GetType() == typeof(HomeViewDetail) || ((NavigationPage)((MasterDetailPage)App.Current.MainPage).Detail).CurrentPage.GetType() == typeof(VotesSelectionPage))
            {
                ControlAplication();
            }
        }

        private void ControlAplication()
        {
          ProposalViewModel.DeleteInstance();
        }

        private void InitComands()
        {

            LoginCommand = new Command(Login);
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
