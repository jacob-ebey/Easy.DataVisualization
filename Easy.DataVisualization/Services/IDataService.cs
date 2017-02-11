using System.Threading.Tasks;

namespace Easy.DataVisualization.Services
{
    public interface IDataService
    {
        // TODO: Expose a result type to allow passing error messages from the service to the VM layer.
        Task<string> GetDataAsync(object source);
    }
}
