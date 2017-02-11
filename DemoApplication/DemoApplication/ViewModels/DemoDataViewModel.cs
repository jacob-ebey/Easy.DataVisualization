using Easy.DataVisualization.Controls;
using Easy.DataVisualization.MVVM;
using Xamarin.Forms;

namespace DemoApplication.ViewModels
{
    class DemoDataViewViewModel : DataViewModel
    {
        public DemoDataViewViewModel()
        {
            NavigateCommand = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new DataPage());
            });

            ManipulateDataCommand = new Command(() =>
            {
                if (Data?.DataType == "DemoDataModel")
                {
                    Data["Property1"] = "Some new data :D";
                }
            });
        }

        public Command NavigateCommand { get; }

        public Command ManipulateDataCommand { get; }
    }
}
