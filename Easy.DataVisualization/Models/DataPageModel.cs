using System.Collections.Generic;

namespace Easy.DataVisualization.Models
{
    /// <summary>
    /// Serve an instance of this from the serer.
    /// </summary>
    public class DataPageModel : DataModel
    {
        /// <summary>
        /// The data to display.
        /// </summary>
        public IEnumerable<IDataModel> Data { get; set; }
    }
}
