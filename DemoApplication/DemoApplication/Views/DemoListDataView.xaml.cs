using DemoApplication.ViewModels;
using Xamarin.Forms;

namespace DemoApplication.Views
{
    public partial class DemoListDataView : ListView
    {
        public DemoListDataView()
        {
            BindingContext = new DemoListDataViewViewModel();

            InitializeComponent();
        }

        public void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (BindingContext as DemoListDataViewViewModel).NavigateCommand.Execute(null);
            SelectedItem = null;
        }
    }
}
