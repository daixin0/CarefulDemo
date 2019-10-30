using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using WPFCommonLib.Misc;

namespace WPFCommonLib.Extensions
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

        public static ObservableCollectionEx<T> ToObservableCollectionEx<T>(this IEnumerable<T> list) where T : INotifyPropertyChanged
        {
            return new ObservableCollectionEx<T>(list);
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                var isIgnore = prop.Attributes.Cast<Attribute>().Any(m => m is IgnoreAttribute);
                if (isIgnore)
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

                var columeTypeAttribute = prop.Attributes.Cast<Attribute>().FirstOrDefault(m => m is ColumnDataTypeAttribute) as ColumnDataTypeAttribute;
                if (columeTypeAttribute != null)
                {
                    dataColumn.DataType = columeTypeAttribute.DataType;
                }

                dt.Columns.Add(dataColumn);
            }

            foreach (T item in data)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyDescriptor pdt in properties)
                {
                    var isIgnore = pdt.Attributes.Cast<Attribute>().Any(m => m is IgnoreAttribute);
                    if (isIgnore)
                    {
                        continue;
                    }

                    // 得到原始值
                    var value = pdt.GetValue(item) ?? DBNull.Value;

                    // 对 PropertyFormat Attribute 进行检查
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

                    // 对 ValueDescription Attribute 进行检查
                    var valueDescriptionAttribute = pdt.Attributes.Cast<Attribute>().FirstOrDefault(m => m is ValueDescriptionAttribute) as ValueDescriptionAttribute;
                    if (valueDescriptionAttribute != null && valueDescriptionAttribute.Values.Count > 0)
                    {
                        if (valueDescriptionAttribute.Values.ContainsKey(value.ToString()))
                        {
                            value = valueDescriptionAttribute.Values[value.ToString()];
                        }
                    }

                    row[pdt.Name] = value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}