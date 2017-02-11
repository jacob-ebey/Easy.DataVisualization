using DemoApplication.Views;
using Easy.DataVisualization.Controls;

using Xamarin.Forms;

namespace DemoApplication
{
    public partial class App : Application
    {
        public App()
        {
            ControlResolver.Register<DemoDataView>("DemoDataModel");
            ControlResolver.Register<DemoListDataView>("DemoListDataModel");

            InitializeComponent();

            MainPage = new NavigationPage(new DataPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
