using System;
using EthereumDemoApp.Views;

using Xamarin.Forms;

namespace EthereumDemoApp
{
    public partial class App : Application
    {
        
        public App()
        {
            
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
