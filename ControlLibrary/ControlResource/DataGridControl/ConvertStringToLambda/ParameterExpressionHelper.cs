using System;
using System.Linq.Expressions;

namespace Careful.Controls.DataGridControl.ConvertStringToLambda
{
    internal static class ParameterExpressionHelper
    {
        public static ParameterExpression CreateParameterExpression(Type type, string name)
        {
            return Expression.Parameter(type, name);
        }
    }
}
