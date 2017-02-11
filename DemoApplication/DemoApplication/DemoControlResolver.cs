using DemoApplication.Views;
using Easy.DataVisualization.Controls;

namespace DemoApplication
{
    class DemoControlResolver : ControlResolver
    {
        public DemoControlResolver()
        {
            Register<DemoDataView>("DemoDataModel");
            Register<DemoListDataView>("DemoListDataModel");
        }
    }
}
