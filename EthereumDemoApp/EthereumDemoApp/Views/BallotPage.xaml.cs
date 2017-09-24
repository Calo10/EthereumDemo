using System;
using System.Collections.Generic;
using EthereumDemoApp.ViewModels;
using Xamarin.Forms;
using EthereumDemoApp.Models;
using System.Collections.ObjectModel;

namespace EthereumDemoApp.Views
{
    public partial class BallotPage : ContentPage
    {
        public BallotPage()
        {
            InitializeComponent();

            BindingContext = ProposalViewModel.GetInstance(null);
        }

     
    }
}
