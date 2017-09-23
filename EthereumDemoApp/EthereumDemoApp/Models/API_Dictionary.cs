using System;

using Xamarin.Forms;

namespace EthereumDemoApp.Models
{
    public class API_Dictionary : ContentPage
    {
        public API_Dictionary()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

