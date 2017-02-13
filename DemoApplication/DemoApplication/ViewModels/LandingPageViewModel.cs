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
                var page = new DataPage()
                {
                    Source = UserName,
                    BindingContext = new ResultPageViewModel()
                };
                (page.LayoutManeger as StackLayoutManeger).Orientation = StackOrientation.Horizontal;
                Application.Current.MainPage.Navigation.PushAsync(page);
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
