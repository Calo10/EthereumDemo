using System;
using System.Collections.Generic;
using EthereumDemoApp.ViewModels;
using Xamarin.Forms;

namespace EthereumDemoApp.Views
{
    public partial class ReferendumPage : ContentPage
    {
        public ReferendumPage()
        {
            InitializeComponent();

            BindingContext = ProposalViewModel.GetInstance(null);
        }

       
    }
}
