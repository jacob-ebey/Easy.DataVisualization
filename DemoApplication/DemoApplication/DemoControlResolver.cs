using DemoApplication.Views;
using Easy.DataVisualization.Controls;

namespace DemoApplication
{
    class DemoControlResolver : ControlResolver
    {
        public DemoControlResolver()
        {
            Register<UserView>("UserModel");
            Register<RepositoriesView>("ReposModel");
        }
    }
}
