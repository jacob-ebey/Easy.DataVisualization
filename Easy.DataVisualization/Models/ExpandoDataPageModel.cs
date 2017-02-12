using System.Collections.Generic;
using System.Dynamic;

namespace Easy.DataVisualization.Models
{
    public class ExpandoDataPageModel
    {
        /// <summary>
        /// The data to display. These should be served an insance of one of the supported subclasses of
        /// <see cref="DataModel"/>.
        /// </summary>
        public IEnumerable<IDictionary<string, object>> Data { get; set; }
    }
}
