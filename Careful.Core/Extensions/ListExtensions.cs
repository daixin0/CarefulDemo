using Careful.Core.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Careful.Core.Extensions
{
    public static class ListExtensions
    {
        public static ObservableCollection<T> DeepCopy<T>(this IEnumerable<T> list) where T : ICloneable
        {
            return new ObservableCollection<T>(list.Select(x => x.Clone()).Cast<T>());
        }

        public static bool HasItems(this IList list)
        {
            return list != null && list.Count > 0;
        }

        public static bool HasItems<T>(this T list) where T : IList
        {
            return list != null && list.Count > 0;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list)
        {
            return new ObservableCollection<T>(list);
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                var isDescription = prop.Attributes.Cast<Attribute>().Any(m => m is DescriptionAttribute);
                if (!isDescription)
                {
                    continue;
                }

                var description = prop.Description;
                if (string.IsNullOrWhiteSpace(description))
                {
                    description = prop.Name;
                }

                var dataColumn = new DataColumn
                {
                    ColumnName = prop.Name,
                    DataType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType,
                    Caption = description
                };

                dt.Columns.Add(dataColumn);
            }

            foreach (T item in data)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyDescriptor pdt in properties)
                {
                    var isDescription = pdt.Attributes.Cast<Attribute>().Any(m => m is DescriptionAttribute);
                    if (!isDescription)
                    {
                        continue;
                    }

                    var value = pdt.GetValue(item) ?? DBNull.Value;

                    var propertyFormatAttribute = pdt.Attributes.Cast<Attribute>().FirstOrDefault(m => m is PropertyFormatOutputAttribute) as PropertyFormatOutputAttribute;
                    if (propertyFormatAttribute != null)
                    {
                        List<string> tmpValues = new List<string>();
                        foreach (var p in propertyFormatAttribute.PropertyNames)
                        {
                            string pValue = string.Empty;
                            var tmpProp = typeof(T).GetProperty(p);
                            if (tmpProp != null)
                            {
                                var tmpValue = tmpProp.GetValue(item);
                                if (tmpValue != null)
                                {
                                    pValue = tmpValue.ToString();
                                }
                            }

                            tmpValues.Add(pValue);
                        }

                        value = string.Format(propertyFormatAttribute.FormatString, tmpValues.ToArray());
                    }
                    row[pdt.Name] = value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}