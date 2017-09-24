using System;
using System.Collections.Generic;
using EthereumDemoApp.ViewModels;

using Xamarin.Forms;

namespace EthereumDemoApp.Views
{
    public partial class ResultPage : ContentPage
    {
        public ResultPage()
        {
            InitializeComponent();

            BindingContext = ProposalViewModel.GetInstance(null);
        }
    }
}
