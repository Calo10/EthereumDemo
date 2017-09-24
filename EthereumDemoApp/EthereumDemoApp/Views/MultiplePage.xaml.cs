using System;
using System.Collections.Generic;
using EthereumDemoApp.ViewModels;
using Xamarin.Forms;

namespace EthereumDemoApp.Views
{
    public partial class MultiplePage : ContentPage
    {
        public MultiplePage()
        {
            InitializeComponent();

            BindingContext = ProposalViewModel.GetInstance(null);
        }

       
    }
}
