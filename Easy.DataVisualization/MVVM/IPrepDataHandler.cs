using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easy.DataVisualization.MVVM
{
    /// <summary>
    /// An interface exposing the service to prep data from a VM.
    /// </summary>
    public interface IPrepDataHandler
    {
        /// <summary>
        /// Prepare the binding by adding properties / manipulating data.
        /// </summary>
        /// <param name="expandoHelper">The data to manipulate.</param>
        void PrepBinding(ExpandoHelper data);

        /// <summary>
        /// Where the data that is retrieved gets stored.
        /// </summary>
        ExpandoHelper Data { get; set; }
    }
}
