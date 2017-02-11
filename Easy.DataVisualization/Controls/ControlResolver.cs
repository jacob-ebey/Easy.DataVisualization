using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Easy.DataVisualization.Controls
{
    public interface IControlResolver
    {
        void Register<T>(string dataType) where T : View;

        View ResolveControl(string dataType);
    }

    public class ControlResolver : IControlResolver
    {
        private Dictionary<string, Func<View>> _cache = new Dictionary<string, Func<View>>();

        public void Register<T>(string dataType) where T : View
        {
            _cache[dataType] = () => Activator.CreateInstance<T>();
        }

        public View ResolveControl(string dataType)
        {
            Func<View> factory = null;

            _cache.TryGetValue(dataType, out factory);

            return factory?.Invoke();
        }
    }
}
