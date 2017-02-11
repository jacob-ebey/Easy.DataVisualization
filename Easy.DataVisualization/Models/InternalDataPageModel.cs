using System.Collections.Generic;
using System.Dynamic;

namespace Easy.DataVisualization.Models
{
    internal class InternalDataPageModel
    {
        // TODO: Expose page template selection from DataType (this is a future thing).
        public string DataType { get; set; }

        /// <summary>
        /// The data to display. These should be served an insance of one of the supported subclasses of
        /// <see cref="DataModel"/>.
        /// </summary>
        public IEnumerable<ExpandoObject> Data { get; set; }
    }
}
