using Easy.DataVisualization.Models;
using Easy.DataVisualization.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DemoApplication.Services
{
    class DemoDataService : IDataService
    {
        // This isn't needed in the client app, it's just to make creating mock data easy.
        public class DemoDataModel : DataModel
        {
            public DemoDataModel()
            {
                DataType = "DemoDataModel";
            }

            public string Property1 { get; set; }

            public string Property2 { get; set; }
        }

        public Task<string> GetDataAsync(object source)
        {
            return Task.Run(() =>
            {
                string stringSource = source as string;

                DataPageModel model = new DataPageModel
                {
                    Data = new DataModel[]
                    {
                            new DemoDataModel
                            {
                                Property1 = "Data from the service...",
                                Property2 = "More data from the service..."
                            },
                            new ListDataModel
                            {
                                DataType = "DemoListDataModel",
                                Items = new object[]
                                {
                                    new { Label = "Item 1" },
                                    new { Label = "Item 2" },
                                    new { Label = "Etc..." },
                                    null
                                }
                            },
                            new DemoDataModel
                            {
                                Property1 = "Data from the service...",
                                Property2 = "More data from the service..."
                            },
                            new ListDataModel
                            {
                                DataType = "DemoListDataModel",
                                Items = new object[]
                                {
                                    new { Label = "Item 1" },
                                    new { Label = "Item 2" },
                                    new { Label = "Etc..." },
                                    null
                                }
                            }
                    }
                };

                return JsonConvert.SerializeObject(model);
            });
        }
    }
}
