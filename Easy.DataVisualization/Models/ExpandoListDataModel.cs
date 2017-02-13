using System.Collections.Generic;

namespace Easy.DataVisualization.Models
{
    public class ExpandoListDataModel : ExpandoDataModel
    {
        public IEnumerable<IDictionary<string, object>> Items
        {
            get { return this[nameof(Items)] as IEnumerable<IDictionary<string, object>>; }
            set
            {
                this[nameof(Items)] = value;
            }
        }
    }
}
