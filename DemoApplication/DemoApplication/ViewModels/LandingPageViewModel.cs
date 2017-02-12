using Easy.DataVisualization.Controls;
using Xamarin.Forms;

namespace DemoApplication.ViewModels
{
    class LandingPageViewModel : ViewModelBase
    {
        public LandingPageViewModel()
        {
            BrowseCommand = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new DataPage() { Source = UserName });
            });
        }

        private string _userName = "jacob-ebey";
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public Command BrowseCommand { get; }
    }
}
