using Xamarin.Forms;

namespace Easy.DataVisualization.Controls
{
    public class StackLayoutManeger : ILayoutManeger
    {
        public View RootView { get; } = new StackLayout();

        public StackOrientation Orientation
        {
            get { return (RootView as StackLayout).Orientation; }
            set { (RootView as StackLayout).Orientation = value; }
        }

        public void AddChild(View v)
        {
            (RootView as StackLayout).Children.Add(v);
        }

        public void ClearChildren()
        {
            (RootView as StackLayout).Children.Clear();
        }
    }
}
