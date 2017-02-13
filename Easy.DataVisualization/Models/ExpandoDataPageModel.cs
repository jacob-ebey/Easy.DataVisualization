using System.Collections.Generic;

namespace Easy.DataVisualization.Models
{
    /// <summary>
    /// Serve an instance of this from the serer.
    /// </summary>
    public class ExpandoDataPageModel : ExpandoDataModel
    {
        /// <summary>
        /// The data to display.
        /// </summary>
        public IEnumerable<IDictionary<string, object>> Data
        {
            get { return this[nameof(Data)] as IEnumerable<IDictionary<string, object>>; }
            set
            {
                this[nameof(Data)] = value;
            }
        }
    }
}
