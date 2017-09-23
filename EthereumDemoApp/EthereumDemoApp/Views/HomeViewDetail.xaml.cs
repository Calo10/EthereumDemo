using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EthereumDemoApp.Views
{
    public partial class HomeViewDetail : ContentPage
    {
        public HomeViewDetail()
        {
            InitializeComponent();

            BindingContext = new HomeViewModel();
        }

        #region Private Methods

        private void Cell_OnTapped(object sender, EventArgs e)
        {
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.White;
            }
        }

        #endregion
    }
}
