using Easy.DataVisualization.Models;
using Easy.DataVisualization.MVVM;
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
        public async Task<IDictionary<string, object>> GetDataAsync(object source)
        {
            string stringSource = source as string;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Easy.DataVisualization.Demo");

            var userResult = await client.GetStringAsync($"https://api.github.com/users/{stringSource}");
            var userModel = new ExpandoDataModel
            {
                DataType = "UserModel"
            };
            userModel.Add("User", JsonConvert.DeserializeObject<ExpandoHelper>(userResult));

            var repoResult = await client.GetStringAsync($"https://api.github.com/users/{stringSource}/repos");
            var repos = JsonConvert.DeserializeObject<IEnumerable<ExpandoHelper>>(repoResult);

            ExpandoDataPageModel model = new ExpandoDataPageModel
            {
                Data = new IDictionary<string, object>[]
                {
                    userModel,
                    new ExpandoListDataModel
                    {
                        DataType = "ReposModel",
                        Items = repos
                    }
                }
            };

            return model;
        }
    }
}
