using Easy.DataVisualization.Models;
using Easy.DataVisualization.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DemoApplication.Services
{
    class GithubUserService : IDataService
    {
        public async Task<string> GetDataAsync(object source)
        {
            string stringSource = source as string;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Easy.DataVisualization.Demo");

            var userResult = await client.GetStringAsync($"https://api.github.com/users/{stringSource}");
            var userModel = new ExpandoDataModel
            {
                DataType = "UserModel"
            };
            userModel.Add("User", JsonConvert.DeserializeObject<ExpandoObject>(userResult));

            var repoResult = await client.GetStringAsync($"https://api.github.com/users/{stringSource}/repos");
            var repos = JsonConvert.DeserializeObject<IEnumerable<ExpandoObject>>(repoResult);

            DataPageModel model = new DataPageModel
            {
                Data = new IDataModel[]
                {
                    userModel,
                    new ListDataModel
                    {
                        DataType = "ReposModel",
                        Items = repos
                    }
                }
            };

            return JsonConvert.SerializeObject(model);
        }
    }
}
