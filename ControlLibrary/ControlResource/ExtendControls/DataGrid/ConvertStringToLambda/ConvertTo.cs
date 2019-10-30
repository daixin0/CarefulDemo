using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ControlResource.ExtendControlStyle.DataGrid.ConvertStringToLambda
{
    public static class ConvertTo
    {
        private static readonly TraceSource TraceSource = new TraceSource(typeof(ConvertTo).Name);
        private static Expression OptimizeExpression(Expression expression)
        {
            if (ExtensibilityPoint.QueryOptimizer != null)
            {
                var optimized = ExtensibilityPoint.QueryOptimizer(expression);
                
                if (optimized != expression)
                {
                    TraceSource.TraceEvent(TraceEventType.Verbose, 0, "Expression before : {0}", expression);
                    TraceSource.TraceEvent(TraceEventType.Verbose, 0, "Expression after  : {0}", optimized);
                }
                return optimized;
            }

            return expression;
        }

        #region Where
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A <see cref="IQueryable{TSource}"/> to filter.</param>
        /// <param name="config">The <see cref="ParsingConfig"/>.</param>
        /// <param name="predicate">An expression string to test each element for a condition.</param>
        /// <param name="args">An object array that contains zero or more objects to insert into the predicate as parameters. Similar to the way String.Format formats strings.</param>
        /// <returns>A <see cref="IQueryable{TSource}"/> that contains elements from the input sequence that satisfy the condition specified by predicate.</returns>
        /// <example>
        /// <code language="cs">
        /// var result1 = queryable.Where("NumberProperty = 1");
        /// var result2 = queryable.Where("NumberProperty = @0", 1);
        /// var result3 = queryable.Where("StringProperty = null");
        /// var result4 = queryable.Where("StringProperty = \"abc\"");
        /// var result5 = queryable.Where("StringProperty = @0", "abc");
        /// </code>
        /// </example>
        public static IQueryable<TSource> Where<TSource>([NotNull] this IQueryable<TSource> source, [CanBeNull] ParsingConfig config, [NotNull] string predicate, params object[] args)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            return (IQueryable<TSource>)Where((IQueryable)source, config, predicate, args);
        }

        /// <inheritdoc cref="DynamicQueryableExtensions.Where{TSource}(IQueryable{TSource}, ParsingConfig, string, object[])"/>
        public static IQueryable<TSource> Where<TSource>([NotNull] this IQueryable<TSource> source, [NotNull] string predicate, params object[] args)
        {
            return Where(source, null, predicate, args);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="source">A <see cref="IQueryable"/> to filter.</param>
        /// <param name="config">The <see cref="ParsingConfig"/>.</param>
        /// <param name="predicate">An expression string to test each element for a condition.</param>
        /// <param name="args">An object array that contains zero or more objects to insert into the predicate as parameters. Similar to the way String.Format formats strings.</param>
        /// <returns>A <see cref="IQueryable"/> that contains elements from the input sequence that satisfy the condition specified by predicate.</returns>
        /// <example>
        /// <code>
        /// var result1 = queryable.Where("NumberProperty = 1");
        /// var result2 = queryable.Where("NumberProperty = @0", 1);
        /// var result3 = queryable.Where("StringProperty = null");
        /// var result4 = queryable.Where("StringProperty = \"abc\"");
        /// var result5 = queryable.Where("StringProperty = @0", "abc");
        /// </code>
        /// </example>
        public static IQueryable Where([NotNull] this IQueryable source, [CanBeNull] ParsingConfig config, [NotNull] string predicate, params object[] args)
        {
            Check.NotNull(source, nameof(source));
            Check.NotEmpty(predicate, nameof(predicate));

            bool createParameterCtor = source.IsLinqToObjects();
            LambdaExpression lambda = DynamicExpressionParser.ParseLambda(config, createParameterCtor, source.ElementType, null, predicate, args);

            var optimized = OptimizeExpression(Expression.Call(typeof(Queryable), nameof(Queryable.Where), new[] { source.ElementType }, source.Expression, Expression.Quote(lambda)));
            return source.Provider.CreateQuery(optimized);
        }
        public static Predicate<object> ConvertStringToLambda(Type sourceType, Expression sourceExoression, [CanBeNull] ParsingConfig config, [NotNull] string predicate, params object[] args)
        {
            Check.NotEmpty(predicate, nameof(predicate));
            return DynamicExpressionParser.ParsePredicate(config,sourceType, null, predicate, args);
        }
        /// <inheritdoc cref = "DynamicQueryableExtensions.Where(IQueryable, ParsingConfig, string, object[])" />
        public static IQueryable Where([NotNull] this IQueryable source, [NotNull] string predicate, params object[] args)
        {
            return Where(source, null, predicate, args);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="source">A <see cref="IQueryable"/> to filter.</param>
        /// <param name="lambda">A cached Lambda Expression.</param>
        public static IQueryable Where([NotNull] this IQueryable source, [NotNull] LambdaExpression lambda)
        {
            Check.NotNull(source, nameof(source));
            Check.NotNull(lambda, nameof(lambda));

            var optimized = OptimizeExpression(Expression.Call(typeof(Queryable), nameof(Queryable.Where), new[] { source.ElementType }, source.Expression, Expression.Quote(lambda)));
            return source.Provider.CreateQuery(optimized);
        }
        #endregion
    }
}
