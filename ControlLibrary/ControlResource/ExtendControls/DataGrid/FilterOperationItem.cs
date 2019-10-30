using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlResource.ExtendControlStyle.DataGrid
{
    public class FilterOperationItem
    {
        public Enums.FilterOperation FilterOption { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public FilterOperationItem(Enums.FilterOperation operation, string description, string imagePath)
        {
            FilterOption = operation;
            Description = description;
            ImagePath = imagePath;
        }
        public override string ToString()
        {
            return Description;
        }
    }
}
