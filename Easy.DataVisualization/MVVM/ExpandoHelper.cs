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

        public ExpandoHelper(ExpandoObject o, string dataType = null)
        {
            _dict = o;
            DataType = dataType;
        }

        public string DataType { get; }

        public object this[string key]
        {
            get
            {
                object result = null;
                if (_dict?.TryGetValue(key, out result) ?? false)
                {
                    if (result as ExpandoObject != null)
                    {
                        _dict[key] = result = new ExpandoHelper(result as ExpandoObject);
                    }
                }
                return result;
            }

            set
            {
                if (_dict == null)
                {
                    _dict = new ExpandoObject();
                }
                _dict[key] = value;
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
