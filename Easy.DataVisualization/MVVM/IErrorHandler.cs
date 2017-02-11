using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.DataVisualization.MVVM
{
    public enum ErrorType
    {
        /// <summary>
        /// No data was found in the response. This is either because the response was null 
        /// or the Data property of the response was null or empty.
        /// </summary>
        NoData,

        /// <summary>
        /// No control was avaliable for the requested type.
        /// </summary>
        NoControlForType,

        /// <summary>
        /// No datatype could not be determined from the result from the data service.
        /// </summary>
        NoDataType
    }

    public interface IErrorHandler
    {
        Task HandleErrorAsync(ErrorType error);

        Task HandleExceptionAsync(Exception e);
    }
}
