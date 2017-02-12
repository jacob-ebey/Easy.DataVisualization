using Easy.DataVisualization.MVVM;
using System;
using System.Threading.Tasks;
using Easy.DataVisualization.Controls;

namespace DemoApplication.ViewModels
{
    class ResultPageViewModel : ViewModelBase, IErrorHandler
    {
        public Task HandleErrorAsync(ErrorType error)
        {
            return Task.Run(() =>
            {
                MessageType = MessageType.Warning;
                Message = "Could not find that profile for you :(";
            });
        }

        public Task HandleExceptionAsync(Exception e)
        {
            return HandleErrorAsync(ErrorType.NoData);
        }
    }
}
