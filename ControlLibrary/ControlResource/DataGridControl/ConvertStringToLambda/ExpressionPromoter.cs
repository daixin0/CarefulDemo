using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Careful.Controls.DataGridControl.ConvertStringToLambda
{
    internal static class ExpressionPromoter
    {
        public static Expression Promote(Expression expr, Type type, bool exact, bool convertExpr)
        {
            if (expr.Type == type)
            {
                return expr;
            }

            var ce = expr as ConstantExpression;

            if (ce != null)
            {
                if (ce == Constants.NullLiteral || ce.Value == null)
                {
                    if (!type.GetTypeInfo().IsValueType || TypeHelper.IsNullableType(type))
                    {
                        return Expression.Constant(null, type);
                    }
                }
                else
                {
                    if (ConstantExpressionHelper.TryGetText(ce, out string text))
                    {
                        Type target = TypeHelper.GetNonNullableType(type);
                        object value = null;
                        switch (Type.GetTypeCode(ce.Type))
                        {
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                                value = TypeHelper.ParseNumber(text, target);

                                // Make sure an enum value stays an enum value
                                if (target.IsEnum)
                                {
                                    value = Enum.ToObject(target, value);
                                }
                                break;

                            case TypeCode.Double:
                                if (target == typeof(decimal)) value = TypeHelper.ParseNumber(text, target);
                                break;

                            case TypeCode.String:
                                value = TypeHelper.ParseEnum(text, target);
                                break;
                        }
                        if (value != null)
                        {
                            return Expression.Constant(value, type);
                        }
                    }
                }
            }

            if (TypeHelper.IsCompatibleWith(expr.Type, type))
            {
                if (type.GetTypeInfo().IsValueType || exact || expr.Type.GetTypeInfo().IsValueType && convertExpr)
                {
                    return Expression.Convert(expr, type);
                }

                return expr;
            }

            return null;
        }
    }
}
