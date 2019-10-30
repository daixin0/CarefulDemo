using System;
using System.Linq.Expressions;

namespace ControlResource.ExtendControlStyle.DataGrid.ConvertStringToLambda
{
    internal static class ParameterExpressionHelper
    {
        public static ParameterExpression CreateParameterExpression(Type type, string name)
        {
            return Expression.Parameter(type, name);
        }
    }
}
