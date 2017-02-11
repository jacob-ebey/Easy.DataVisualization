﻿using Easy.DataVisualization.Models;
using Easy.DataVisualization.MVVM;
using Easy.DataVisualization.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Easy.DataVisualization.Controls
{
    // Tasks for DataPage.
    // TODO: - Abstract UI dispatching for unit testing.
    // TODO: - Expose IsLoading property for templates to show a loading overlay.
    // TODO: - Abstract out ControlResolver so it isn't static and is a property of DataPage

    /// <summary>
    /// The type of a message to show to a user.
    /// </summary>
    public enum MessageType { Information, Warning, Error }

    /// <summary>
    /// The page for presenting data visualizations served from the Microsoft.Forms.DataVisualization framework.
    /// </summary>
    public class DataPage : ContentPage
    {
        private StackLayout _layout;

        /// <summary>
        /// Creates a new instance of <see cref="DataPage"/>.
        /// </summary>
        public DataPage()
        {
            _layout = new StackLayout();

            Content = _layout;
        }

        /// <summary>
        /// Gets or sets the <see cref="IDataService"/> to use.
        /// </summary>
        public IDataService DataService
        {
            get { return (IDataService) GetValue(DataServiceProperty); }
            set { SetValue(DataServiceProperty, value); }
        }

        /// <summary>
        /// The source to fetch data from via the registred <see cref="IDataService"/>.
        /// </summary>
        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// The message to show.
        /// </summary>
        public string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// The type of the message if it is not null.
        /// </summary>
        public MessageType MessageType
        {
            get { return (MessageType) GetValue(MessageTypeProperty); }
            set { SetValue(MessageTypeProperty, value); }
        }

        /// <summary>
        /// True if <see cref="Message"/> is not null; otherwises false.
        /// </summary>
        public bool HasMessage
        {
            get { return Message != null; }
        }

        private async void OnDataServicePropertyChanged(IDataService oldValue, IDataService newValue)
        {
            await OnDataServicePropertyChangedAsync(oldValue, newValue);
        }

        internal async Task OnDataServicePropertyChangedAsync(IDataService oldValue, IDataService newValue)
        {
            if (oldValue == newValue)
            {
                return;
            }

            await DoRequestAsync(newValue, Source);
        }

        private async void OnSourcePropertyChanged(object oldValue, object newValue)
        {
            await OnSourcePropertyChangedAsync(oldValue, newValue);
        }

        // TODO: Test this method and abstract ui dispatching
        internal async Task OnSourcePropertyChangedAsync(object oldValue, object newValue)
        {
            await DoRequestAsync(DataService, newValue);
        }

        private async Task DoRequestAsync(IDataService dataService, object source)
        {
            if (dataService != null)
            {
                string tempData = null;
                try
                {
                    // TODO: Expose a result type to allow passing error messages from the service to the VM layer.
                    tempData = await dataService.GetDataAsync(source);
                }
                catch (Exception e)
                {
                    if (BindingContext as IErrorHandler != null)
                    {
                        await (BindingContext as IErrorHandler).HandleExceptionAsync(e);
                    }
                }

                InternalDataPageModel newData = null;
                try { newData = JsonConvert.DeserializeObject<InternalDataPageModel>(tempData); }
                catch { }

                Device.BeginInvokeOnMainThread(() =>
                {
                    PopulateFromData(newData);
                });
            }
        }

        internal void OnMessagePropertyChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(nameof(Message));
        }

        private void PopulateFromData(InternalDataPageModel data)
        {
            _layout.Children.Clear();

            if (data?.Data?.Any() ?? false)
            {
                foreach (var dataModel in data.Data)
                {
                    var dataModelHelper = new ExpandoHelper(dataModel);
                    var dataType = dataModelHelper["DataType"] as string;

                    if (dataType != null)
                    {
                        var control = ControlResolver.ResolveControl(dataType);

                        if (control != null)
                        {
                            bool setBindingContext = true;

                            if (control is ListView)
                            {
                                var listView = control as ListView;

                                IEnumerable<dynamic> items = dataModelHelper["Items"] as IEnumerable<dynamic>;

                                if (items?.Any() ?? false)
                                {
                                    List<ExpandoHelper> itemsSource = null;

                                    if (items != null)
                                    {
                                        itemsSource = new List<ExpandoHelper>();

                                        foreach (var i in items)
                                        {
                                            var helper = new ExpandoHelper(i);

                                            if (listView.BindingContext as IPrepListData != null)
                                            {
                                                (listView.BindingContext as IPrepListData).PrepItemBinding(helper);
                                            }

                                            itemsSource.Add(helper);
                                        }
                                    }

                                    listView.ItemsSource = itemsSource;
                                }
                            }

                            setBindingContext = control.BindingContext == null;

                            var binding = new ExpandoHelper(dataModel, dataType);
                            if (setBindingContext)
                            {
                                control.BindingContext = binding;
                            }
                            else
                            {
                                if (control.BindingContext as IPrepDataHandler != null)
                                {
                                    (control.BindingContext as IPrepDataHandler).PrepBinding(binding);
                                    (control.BindingContext as IPrepDataHandler).Data = binding;
                                }
                            }

                            _layout.Children.Add(control);
                        }
                        else
                        {
                            if (BindingContext as IErrorHandler != null)
                            {
                                (BindingContext as IErrorHandler).HandleErrorAsync(ErrorType.NoControlForType);
                            }
                        }
                    }
                    else
                    {
                        if (BindingContext as IErrorHandler != null)
                        {
                            (BindingContext as IErrorHandler).HandleErrorAsync(ErrorType.NoDataType);
                        }
                    }
                }
            }
            else
            {
                if (BindingContext as IErrorHandler != null)
                {
                    (BindingContext as IErrorHandler).HandleErrorAsync(ErrorType.NoData);
                }
            }
        }

        #region Static shit

        /// <summary>
        /// The backing field for the <see cref="DataService"/> property.
        /// </summary>
        public static readonly BindableProperty DataServiceProperty = BindableProperty.Create(
            nameof(DataService),
            typeof(IDataService),
            typeof(DataPage),
            propertyChanged: OnDataServicePropertyChanged);

        /// <summary>
        /// The backing field for the <see cref="Source"/> property.
        /// </summary>
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(
            nameof(Source),
            typeof(object),
            typeof(DataPage),
            propertyChanged: OnSourcePropertyChanged);

        /// <summary>
        /// The backing field for the <see cref="Message"/> property.
        /// </summary>
        public static readonly BindableProperty MessageProperty = BindableProperty.Create(
            nameof(Message),
            typeof(string),
            typeof(DataPage),
            propertyChanged: OnMessagePropertyChanged);

        /// <summary>
        /// The backing field for the <see cref="MessageType"/> property.
        /// </summary>
        public static readonly BindableProperty MessageTypeProperty = BindableProperty.Create(
            nameof(MessageType),
            typeof(MessageType),
            typeof(DataPage),
            MessageType.Information);

        private static void OnDataServicePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DataPage)?.OnDataServicePropertyChanged((IDataService) oldValue, (IDataService) newValue);
        }

        private static void OnSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DataPage)?.OnSourcePropertyChanged(oldValue, newValue);
        }

        private static void OnMessagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DataPage)?.OnMessagePropertyChanged((string) oldValue, (string) newValue);
        }

        #endregion
    }
}