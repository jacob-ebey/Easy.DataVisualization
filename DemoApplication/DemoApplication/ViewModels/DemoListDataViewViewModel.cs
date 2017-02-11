using Easy.DataVisualization.Controls;
using Easy.DataVisualization.MVVM;
using Xamarin.Forms;

namespace DemoApplication.ViewModels
{
    class DemoListDataViewViewModel : DataViewModel, IPrepListData
    {
        public DemoListDataViewViewModel()
        {
            NavigateCommand = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new DataPage());
            });
        }

        public Command NavigateCommand { get; }

        public void PrepItemBinding(ExpandoHelper data)
        {
            string label = data["Label"] as string;

            if (label != null)
            {
                label += " :D";
            }
            else
            {
                label = "No data...";
            }

            data["Label"] = label;
        }
    }
}
