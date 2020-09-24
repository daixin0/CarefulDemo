using Careful.Controls.DataGridControl.ConvertStringToLambda;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Careful.Controls.DataGridControl.Enums;

namespace Careful.Controls.DataGridControl
{

    /// <summary>
    /// FilterDataWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FilterDataWindow : Window
    {
        public FilterDataWindow()
        {
            InitializeComponent();
        }

        #region 支持所有筛选功能，但不支持加括号（屏蔽）
        //private Predicate<object> GenerateFilterPredicate(ObservableCollection<FilterModel> filterModels)
        //{
        //    Predicate<object> filterPredicate = null;

        //    for (int i = 0; i < filterModels.Count; i++)
        //    {
        //        Predicate<object> temp = GenerateFilterPredicate(filterModels[i].ColumnsNameSelected.ColumnField,
        //            filterModels[i].ColumnValue,
        //            filterModels[i].ColumnsNameSelected.ColumnsType,
        //            filterModels[i].FilterOperationsSelected);
        //        if (filterPredicate == null)
        //            filterPredicate = temp;
        //        else
        //        {
        //            if (i - 1 >= 0)
        //            {
        //                if (GridFilterColumn[i - 1].LogicBracketsSelected == "And")
        //                {
        //                    filterPredicate = filterPredicate.And(temp);
        //                }
        //                else if (GridFilterColumn[i - 1].LogicBracketsSelected == "Or")
        //                {
        //                    filterPredicate = filterPredicate.Or(temp);
        //                }
        //            }
        //        }
        //    }
        //    return filterPredicate;
        //}

        //protected Predicate<object> GenerateFilterPredicate(string propertyName, string filterValue, Type propType, FilterOperationItem filterItem)
        //{
        //    ParameterExpression objParam = System.Linq.Expressions.Expression.Parameter(typeof(object), "x");
        //    UnaryExpression param = System.Linq.Expressions.Expression.TypeAs(objParam, Grid.FilterType);
        //    var prop = System.Linq.Expressions.Expression.Property(param, propertyName);

        //    var val = System.Linq.Expressions.Expression.Constant(filterValue);

        //    switch (filterItem.FilterOption)
        //    {
        //        case Enums.FilterOperation.Contains:
        //            return ExpressionHelper.GenerateGeneric(prop, val, propType, objParam, "Contains");
        //        case Enums.FilterOperation.EndsWith:
        //            return ExpressionHelper.GenerateGeneric(prop, val, propType, objParam, "EndsWith");
        //        case Enums.FilterOperation.StartsWith:
        //            return ExpressionHelper.GenerateGeneric(prop, val, propType, objParam, "StartsWith");
        //        case Enums.FilterOperation.Equals:
        //            DateTime dtEquals = new DateTime();
        //            if (DateTime.TryParse(filterValue, out dtEquals))
        //            {
        //                return ExpressionHelper.GenerateEquals(prop, dtEquals, propType, objParam);
        //            }
        //            else
        //            {
        //                return ExpressionHelper.GenerateEquals(prop, filterValue, propType, objParam);
        //            }
        //        case Enums.FilterOperation.NotEquals:
        //            DateTime dtNotEquals = new DateTime();
        //            if (DateTime.TryParse(filterValue, out dtNotEquals))
        //            {
        //                return ExpressionHelper.GenerateNotEquals(prop, dtNotEquals, propType, objParam);
        //            }
        //            else
        //            {
        //                return ExpressionHelper.GenerateNotEquals(prop, filterValue, propType, objParam);
        //            }
        //        case Enums.FilterOperation.GreaterThanEqual:
        //            DateTime dtGreaterThanEqual = new DateTime();
        //            if (DateTime.TryParse(filterValue, out dtGreaterThanEqual))
        //            {
        //                return ExpressionHelper.GenerateGreaterThanEqual(prop, dtGreaterThanEqual, propType, objParam);
        //            }
        //            else
        //            {
        //                return ExpressionHelper.GenerateGreaterThanEqual(prop, filterValue, propType, objParam);
        //            }
        //        case Enums.FilterOperation.LessThanEqual:
        //            DateTime dtLessThanEqual = new DateTime();
        //            if (DateTime.TryParse(filterValue, out dtLessThanEqual))
        //            {
        //                return ExpressionHelper.GenerateLessThanEqual(prop, dtLessThanEqual, propType, objParam);
        //            }
        //            else
        //            {
        //                return ExpressionHelper.GenerateLessThanEqual(prop, filterValue, propType, objParam);
        //            }
        //        case Enums.FilterOperation.GreaterThan:
        //            DateTime dtGreaterThan = new DateTime();
        //            if (DateTime.TryParse(filterValue, out dtGreaterThan))
        //            {
        //                return ExpressionHelper.GenerateGreaterThan(prop, dtGreaterThan, propType, objParam);
        //            }
        //            else
        //            {
        //                return ExpressionHelper.GenerateGreaterThan(prop, filterValue, propType, objParam);
        //            }
        //        case Enums.FilterOperation.LessThan:
        //            DateTime dtLessThan = new DateTime();
        //            if (DateTime.TryParse(filterValue, out dtLessThan))
        //            {
        //                return ExpressionHelper.GenerateLessThan(prop, dtLessThan, propType, objParam);
        //            }
        //            else
        //            {
        //                return ExpressionHelper.GenerateLessThan(prop, filterValue, propType, objParam);
        //            }
        //        default:
        //            throw new ArgumentException("Could not decode Search Mode.  Did you add a new value to the enum, or send in Unknown?");
        //    }

        //}
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
