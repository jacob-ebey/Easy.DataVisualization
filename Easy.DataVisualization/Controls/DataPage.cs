﻿using Easy.DataVisualization.MVVM;
using Easy.DataVisualization.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Easy.DataVisualization.Controls
{
    // Tasks for DataPage.
    // TODO: - Abstract UI dispatching for unit testing.

    /// <summary>
    /// The page for presenting data visualizations served from the Microsoft.Forms.DataVisualization framework.
    /// </summary>
    public class DataPage : ContentPage
    {
        /// <summary>
        /// Creates a new instance of <see cref="DataPage"/>.
        /// </summary>
        public DataPage()
        {
            LayoutManeger = new StackLayoutManeger();
        }

        /// <summary>
        /// The layout maneger that controls how child elements are added to the root view.
        /// </summary>
        /// <remarks>This defaults to an instance of <see cref="StackLayoutManeger"/>.</remarks>
        public ILayoutManeger LayoutManeger
        {
            get { return (ILayoutManeger)GetValue(LayoutManegerProperty); }
            set { SetValue(LayoutManegerProperty, value); }
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
        /// Gets or sets the <see cref="IControlResolver"/> to use.
        /// </summary>
        public IControlResolver ControlResolver
        {
            get { return (IControlResolver)GetValue(ControlResolverProperty); }
            set { SetValue(ControlResolverProperty, value); }
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

        int taskCount = 0;
        public bool IsLoading
        {
            get { return taskCount > 0; }
        }

        private async Task DoRequestAsync(IDataService dataService, object source)
        {
            if (dataService != null && ControlResolver != null && LayoutManeger != null)
            {
                try
                {
                    taskCount++;
                    OnPropertyChanged(nameof(IsLoading));
                    
                    IDictionary<string, object> tempData = await dataService.GetDataAsync(source);

                    ExpandoHelper newData = new ExpandoHelper(tempData);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        PopulateFromData(newData);
                    });
                }
                catch (Exception e)
                {
                    if (BindingContext as IErrorHandler != null)
                    {
                        await (BindingContext as IErrorHandler).HandleExceptionAsync(e);
                    }
                }
                finally
                {
                    taskCount--;
                    OnPropertyChanged(nameof(IsLoading));
                }             
            }
        }

        private void PopulateFromData(ExpandoHelper data)
        {
            LayoutManeger.ClearChildren();

            if ((data["Data"] as IEnumerable<IDictionary<string, object>>)?.Any() ?? false)
            {
                foreach (var dataModel in data["Data"] as IEnumerable<IDictionary<string, object>>)
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
                            
                            if (setBindingContext)
                            {
                                control.BindingContext = dataModelHelper;
                            }
                            else
                            {
                                if (control.BindingContext as IPrepDataHandler != null)
                                {
                                    (control.BindingContext as IPrepDataHandler).PrepBinding(dataModelHelper);
                                    (control.BindingContext as IPrepDataHandler).Data = dataModelHelper;
                                }
                            }

                            LayoutManeger.AddChild(control);
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

        #region Bindable on change handlers

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

        private async void OnLayoutManegerPropertyChanged(ILayoutManeger oldValue, ILayoutManeger newValue)
        {
            if (newValue != null)
            {
                Content = newValue.RootView;
            }
            await OnLayoutManegerPropertyChangedAsync(oldValue, newValue);
        }

        private async Task OnLayoutManegerPropertyChangedAsync(ILayoutManeger oldValue, ILayoutManeger newValue)
        {
            await DoRequestAsync(DataService, Source);
        }

        private async void OnControlResolverPropertyChanged(IControlResolver oldValue, IControlResolver newValue)
        {
            await OnControlResolverPropertyChangedAsync(oldValue, newValue);
        }

        // TODO: Test this method and abstract ui dispatching
        internal async Task OnControlResolverPropertyChangedAsync(object oldValue, object newValue)
        {
            await DoRequestAsync(DataService, Source);
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

        internal void OnMessagePropertyChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(nameof(HasMessage));
        }

        #endregion

        #region Static shit

        /// <summary>
        /// The backing field for the <see cref="DataService"/> property.
        /// </summary>
        public static readonly BindableProperty LayoutManegerProperty = BindableProperty.Create(
            nameof(LayoutManeger),
            typeof(ILayoutManeger),
            typeof(DataPage),
            propertyChanged: OnLayoutManegerPropertyChanged);
        
        /// <summary>
        /// The backing field for the <see cref="DataService"/> property.
        /// </summary>
        public static readonly BindableProperty DataServiceProperty = BindableProperty.Create(
            nameof(DataService),
            typeof(IDataService),
            typeof(DataPage),
            propertyChanged: OnDataServicePropertyChanged);

        /// <summary>
        /// The backing field for the <see cref="DataService"/> property.
        /// </summary>
        public static readonly BindableProperty ControlResolverProperty = BindableProperty.Create(
            nameof(ControlResolver),
            typeof(IControlResolver),
            typeof(DataPage),
            propertyChanged: OnControlResolverPropertyChanged);

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

        private static void OnLayoutManegerPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DataPage)?.OnLayoutManegerPropertyChanged((ILayoutManeger)oldValue, (ILayoutManeger)newValue);
        }

        private static void OnDataServicePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DataPage)?.OnDataServicePropertyChanged((IDataService) oldValue, (IDataService) newValue);
        }

        private static void OnControlResolverPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DataPage)?.OnControlResolverPropertyChanged((IControlResolver)oldValue, (IControlResolver)newValue);
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
