using DemoApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DemoApplication.Views
{
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            BindingContext = new LandingPageViewModel();

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
