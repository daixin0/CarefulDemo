using Careful.Controls.MultiComboBoxControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Careful.Controls.DataGridControl
{
    public class OptionColumnInfo
    {
        public DataGridColumn Column { get; set; }
        public bool IsValid { get; set; }
        public string PropertyPath { get; set; }
        public IValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public System.Globalization.CultureInfo ConverterCultureInfo { get; set; }
        public Type PropertyType { get; set; }
        public Type PropertyConvertType { get; set; }

        public bool IsSpecialState { get; set; }
        public string StringPath { get; set; }
        public OptionColumnInfo(DataGridColumn column, DataGridExtend grid)
        {
            if (column == null)
                return;

            Column = column;
            if (column is DataGridTemplateColumn)
            {
                var boundColumn = column as DataGridTemplateColumn;
                if (boundColumn != null)
                {
                    DataTemplate dt = boundColumn.CellTemplate as DataTemplate;
                    if (dt != null)
                    {
                        if (dt.DataType != null)
                        {
                            string dataType = dt.DataType.ToString();
                            TemplateColumnType templateType = (TemplateColumnType)Enum.Parse(typeof(TemplateColumnType), dataType);
                            System.Windows.Data.Binding binding = new Binding();
                            switch (templateType)
                            {
                                case TemplateColumnType.HyperLink:
                                    StackPanel sp = dt.LoadContent() as StackPanel;
                                    TextBlock tb = sp.Children[3] as TextBlock;
                                    InlineUIContainer il = tb.Inlines.FirstInline as InlineUIContainer;
                                    Label lbl = il.Child as Label;
                                    Run r = lbl.Content as Run;
                                    binding = BindingOperations.GetBinding(r, Run.TextProperty);
                                    break;
                                case TemplateColumnType.TextBlock:
                                    TextBlock tb1 = dt.LoadContent() as TextBlock;
                                    binding = BindingOperations.GetBinding(tb1, TextBlock.TextProperty);
                                    break;
                                case TemplateColumnType.CheckBox:
                                    CheckBox cb = new CheckBox();
                                    if (dt.LoadContent() is CheckBox)
                                    {
                                        cb = dt.LoadContent() as CheckBox;
                                        
                                    }
                                    else if(dt.LoadContent() is Grid)
                                    {
                                        Grid gd = dt.LoadContent() as Grid;
                                        if (gd.Children.Count > 1)
                                        {
                                            cb = gd.Children[1] as CheckBox;
                                        }
                                    }
                                    if (cb.IsThreeState)
                                    {
                                        IsSpecialState = true;
                                        binding = BindingOperations.GetBinding(cb, CheckBox.ContentProperty);
                                    }
                                    else
                                    {
                                        binding = BindingOperations.GetBinding(cb, CheckBox.IsCheckedProperty);

                                    }
                                    break;
                                case TemplateColumnType.MultiComboBox:
                                    try
                                    {
                                        MultiComboBox multiCbb = dt.LoadContent() as MultiComboBox;
                                        binding = BindingOperations.GetBinding(multiCbb, MultiComboBox.TextProperty);
                                    }
                                    catch (Exception ex)
                                    {
                                        binding = null;
                                    }
                                    break;
                                case TemplateColumnType.ComboBox:
                                    Grid gd2 = dt.LoadContent() as Grid;
                                    ComboBox com = gd2.Children[0] as ComboBox;
                                    if (!string.IsNullOrWhiteSpace(com.DisplayMemberPath))
                                    {
                                        binding = BindingOperations.GetBinding(com, ComboBox.SelectedItemProperty);
                                        StringPath = com.DisplayMemberPath;
                                    }
                                    else
                                    {
                                        binding = BindingOperations.GetBinding(com, ComboBox.SelectedItemProperty);
                                    }

                                    break;
                            }
                            if (binding != null && binding.Path != null && !string.IsNullOrWhiteSpace(binding.Path.Path))
                            {
                                System.Reflection.PropertyInfo propInfo = null;
                                if (!string.IsNullOrWhiteSpace(StringPath))
                                {
                                    System.Reflection.PropertyInfo propInfo2 = null;
                                    if (grid.FilterType != null)
                                        propInfo2 = grid.FilterType.GetProperty(binding.Path.Path);
                                    propInfo = propInfo2.PropertyType.GetProperty(StringPath);
                                }
                                else
                                {
                                    if (grid.FilterType != null)
                                        propInfo = grid.FilterType.GetProperty(binding.Path.Path);
                                }
                                //if (propInfo != null)
                                //{
                                IsValid = propInfo != null ? true : false;
                                PropertyPath = binding.Path.Path;
                                Converter = binding.Converter;
                                PropertyType = propInfo != null ? propInfo.PropertyType : typeof(string);
                                if (Converter != null)
                                {
                                    PropertyConvertType = Converter.Convert(null, null, null, null).GetType();
                                }
                                ConverterCultureInfo = binding.ConverterCulture;
                                ConverterParameter = binding.ConverterParameter;
                                //}
                                //else
                                //{

                                //}
                            }
                        }

                    }

                }
                else if (column.SortMemberPath != null && column.SortMemberPath.Length > 0)
                {
                    PropertyPath = column.SortMemberPath;
                    PropertyType = grid.FilterType.GetProperty(column.SortMemberPath).PropertyType;
                }
            }
            else
            {
                var boundColumn = column as DataGridBoundColumn;
                if (boundColumn != null)
                {
                    System.Windows.Data.Binding binding = boundColumn.Binding as System.Windows.Data.Binding;
                    if (binding.Path == null)
                        return;
                    if (binding != null && !string.IsNullOrWhiteSpace(binding.Path.Path))
                    {
                        System.Reflection.PropertyInfo propInfo = null;
                        if (grid.FilterType != null)
                            propInfo = grid.FilterType.GetProperty(binding.Path.Path);

                        if (propInfo != null)
                        {
                            IsValid = true;
                            PropertyPath = binding.Path.Path;
                            Converter = binding.Converter;
                            PropertyType = propInfo != null ? propInfo.PropertyType : typeof(string);
                            if (Converter != null)
                            {
                                PropertyConvertType = Converter.Convert(null, null, null, null) == null ? null : Converter.Convert(null, null, null, null).GetType();
                            }
                            ConverterCultureInfo = binding.ConverterCulture;
                            ConverterParameter = binding.ConverterParameter;
                        }
                        else
                        {
                            //if (System.Diagnostics.Debugger.IsAttached && System.Diagnostics.Debugger.IsLogging())
                            //    System.Diagnostics.Debug.WriteLine("Jib.WPF.Controls.DataGrid.JibGrid: BindingExpression path error: '{0}' property not found on '{1}'", binding.Path.Path, boundObjectType.ToString());
                        }
                    }
                }
                else if (column.SortMemberPath != null && column.SortMemberPath.Length > 0)
                {
                    PropertyPath = column.SortMemberPath;
                    PropertyType = grid.FilterType.GetProperty(column.SortMemberPath).PropertyType;
                }
            }

        }

        public override string ToString()
        {
            if (PropertyPath != null)
                return PropertyPath;
            else
                return string.Empty;
        }
    }
}
