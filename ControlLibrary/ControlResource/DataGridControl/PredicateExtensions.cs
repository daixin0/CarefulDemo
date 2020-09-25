using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Careful.Controls.DataGridControl
{
    public static class PredicateExtensions
    {
        public static Predicate<T> And<T>(this Predicate<T> original, Predicate<T> newPredicate)
        {
            return t => original(t) && newPredicate(t);
        }

        public static Predicate<T> Or<T>(this Predicate<T> original, Predicate<T> newPredicate)
        {
            return t => original(t) || newPredicate(t);
        }
    }
}
