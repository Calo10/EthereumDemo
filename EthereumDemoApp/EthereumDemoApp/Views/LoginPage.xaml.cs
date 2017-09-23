using System;
using System.Collections.Generic;
using EthereumDemoApp.ViewModels;

using Xamarin.Forms;

namespace EthereumDemoApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = new LoginViewModel();
        }
    }
}
