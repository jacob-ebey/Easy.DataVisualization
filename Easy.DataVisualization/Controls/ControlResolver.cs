using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Easy.DataVisualization.Controls
{
    public static class ControlResolver
    {
        private static Dictionary<string, Func<View>> _cache = new Dictionary<string, Func<View>>();

        public static void Register<T>(string dataType) where T : View
        {
            _cache[dataType] = () => Activator.CreateInstance<T>();
        }

        internal static View ResolveControl(string dataType)
        {
            Func<View> factory = null;

            _cache.TryGetValue(dataType, out factory);

            return factory?.Invoke();
        }
    }
}
