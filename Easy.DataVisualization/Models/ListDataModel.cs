using System.Collections.Generic;

namespace Easy.DataVisualization.Models
{
    public class ListDataModel : DataModel
    {
        public IEnumerable<object> Items { get; set; }
    }
}
