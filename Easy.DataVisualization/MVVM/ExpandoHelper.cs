using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace Easy.DataVisualization.MVVM
{
    /// <summary>
    /// This wrapps the ExpandoObject as an explicit IDictionary to expose bindings
    /// to Xamarin.Forms.
    /// </summary>
    public class ExpandoHelper : IDictionary<string, object>, INotifyPropertyChanged
    {
        private IDictionary<string, object> _dict;

        public ExpandoHelper(IDictionary<string, object> o)
        {
            _dict = o;
        }

        public object GetRecursive(string path, object lastResult = null)
        {
            var helper = lastResult as ExpandoHelper;

            if (string.IsNullOrWhiteSpace(path) || helper == null)
            {
                return lastResult == this ? null : lastResult;
            }

            var split = path.Split(new string[] { "__" }, 2, System.StringSplitOptions.RemoveEmptyEntries);
            
            return GetRecursive(split.Length == 2 ? split[1] : null, helper[split[0]]);
        }

        public void SetRecursive(string path, object value, ExpandoHelper helper)
        {
            if (path == null)
            {
                return;
            }

            if (!path.Contains("__"))
            {
                helper[path] = value;
            }

            var split = path.Split(new string[] { "__" }, 2, System.StringSplitOptions.RemoveEmptyEntries);

            ExpandoHelper newHelper = null;
            object itemAtKey = helper[split[0]];

            if (itemAtKey != null && (itemAtKey as ExpandoHelper) == null)
            {
                return;
            }
            else if (itemAtKey == null)
            {
                helper[split[0]] = newHelper = new ExpandoHelper(new ExpandoObject());
            }
            else
            {
                newHelper = itemAtKey as ExpandoHelper;
            }

            SetRecursive(split.Length == 2 ? split[1] : null, value, newHelper);
        }

        public object this[string key]
        {
            get
            {
                object result = null;
                if (!key.Contains("__"))
                {
                    if (_dict?.TryGetValue(key, out result) ?? false)
                    {
                        if (result as IDictionary<string, object> != null && result as ExpandoHelper == null)
                        {
                            _dict[key] = result = new ExpandoHelper(result as IDictionary<string, object>);
                        }
                    }
                }
                else
                {
                    result = GetRecursive(key, this);
                }
                return result;
            }
            set
            {
                if (_dict == null)
                {
                    _dict = new ExpandoObject();
                }

                if (!key.Contains("__"))
                {
                    _dict[key] = value;
                }
                else
                {

                }

                OnPropertyChanged($"Item[{key}]");
            }
        }

        public int Count
        {
            get
            {
                return _dict.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _dict.IsReadOnly;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return _dict.Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return _dict.Values;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void Add(KeyValuePair<string, object> item)
        {
            if (_dict == null)
            {
                _dict = new ExpandoObject();
            }

            _dict.Add(item);
        }

        public void Add(string key, object value)
        {
            if (_dict == null)
            {
                _dict = new ExpandoObject();
            }
            _dict.Add(key, value);
        }

        public void Clear()
        {
            _dict?.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dict?.Contains(item) ?? false;
        }

        public bool ContainsKey(string key)
        {
            return _dict?.ContainsKey(key) ?? false;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _dict?.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dict?.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dict?.Remove(item) ?? false;
        }

        public bool Remove(string key)
        {
            return _dict?.Remove(key) ?? false;
        }

        public bool TryGetValue(string key, out object value)
        {
            bool result = false;

            if (_dict != null)
            {
                _dict.TryGetValue(key, out value);
            }
            else
            {
                value = null;
            }

            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_dict as IEnumerable)?.GetEnumerator();
        }
    }
}
