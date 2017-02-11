namespace Easy.DataVisualization.MVVM
{
    public interface IPrepListData
    {
        /// <summary>
        /// Prepare the binding by adding properties / manipulating data.
        /// </summary>
        /// <param name="expandoHelper">The data to manipulate.</param>
        void PrepItemBinding(ExpandoHelper data);
    }
}
