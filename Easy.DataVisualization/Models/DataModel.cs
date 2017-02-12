﻿using Easy.DataVisualization.MVVM;

namespace Easy.DataVisualization.Models
{
    public interface IDataModel
    {
        /// <summary>
        /// The data model identifier.
        /// </summary>
        string DataType { get; set; }
    }

    /// <summary>
    /// A data model that gets a control registered to display for it.
    /// </summary>
    public class DataModel : IDataModel
    {
        /// <summary>
        /// The data model identifier.
        /// </summary>
        public string DataType { get; set; }
    }

    public class ExpandoDataModel : ExpandoHelper, IDataModel
    {
        public ExpandoDataModel() 
            : base(new System.Dynamic.ExpandoObject())
        {
        }
        /// <summary>
        /// The data model identifier.
        /// </summary>
        public string DataType
        {
            get { return this[nameof(DataType)] as string; }
            set
            {
                this[nameof(DataType)] = value;
            }
        }
    }
}
