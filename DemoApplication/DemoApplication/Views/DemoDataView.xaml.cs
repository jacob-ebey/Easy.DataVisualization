using DemoApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DemoApplication.Views
{
    public partial class DemoDataView : ContentView
    {
        public DemoDataView()
        {
            BindingContext = new DemoDataViewViewModel();

            InitializeComponent();
        }
    }
}
