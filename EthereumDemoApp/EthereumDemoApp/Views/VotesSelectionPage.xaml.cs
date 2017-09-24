using System;
using System.Collections.Generic;
using EthereumDemoApp.ViewModels;
using EthereumDemoApp.Models;

using Xamarin.Forms;

namespace EthereumDemoApp.Views
{
    public partial class VotesSelectionPage : ContentPage
    {
        public VotesSelectionPage(int type)
        {
            InitializeComponent();

            BindingContext = ProposalViewModel.GetInstance(type);

        }

        private void ProposalListViewTapped(object sender, ItemTappedEventArgs e)
        {
            Proposal currentProposal= (Proposal)e.Item;

            //Navigation.PushAsync(new BallotPage());
        }

       
    }
}
