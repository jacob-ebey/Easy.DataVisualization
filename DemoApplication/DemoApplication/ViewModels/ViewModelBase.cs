using Easy.DataVisualization.MVVM;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemoApplication.ViewModels
{
    class ViewModelBase : INotifyPropertyChanged
    {
        private MessageType _messageType;
        public MessageType MessageType
        {
            get { return _messageType; }
            set
            {
                _messageType = value;
                OnPropertyChanged();
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
