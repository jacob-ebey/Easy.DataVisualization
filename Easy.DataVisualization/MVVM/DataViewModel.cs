using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Easy.DataVisualization.MVVM
{
    // TODO: Move Data down to another class called DataViewModel that implements IDataViewModel then inherit here.
    public abstract class DataViewModel : IPrepDataHandler, INotifyPropertyChanged
    {
        ExpandoHelper _data;
        public ExpandoHelper Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public virtual void PrepBinding(ExpandoHelper expandoHelper) { }
    }
}
