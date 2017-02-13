using System.Collections.Generic;
using System.Threading.Tasks;

namespace Easy.DataVisualization.Services
{
    public interface IDataService
    {
        Task<IDictionary<string, object>> GetDataAsync(object source);
    }
}
