using Xamarin.Forms;

namespace Easy.DataVisualization.Controls
{
    public interface ILayoutManeger
    {
        View RootView { get; }

        void AddChild(View v);

        void ClearChildren();
    }
}
