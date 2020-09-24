using System.Linq.Expressions;

namespace Careful.Controls.DataGridControl.ConvertStringToLambda
{
    internal class DynamicOrdering
    {
        public Expression Selector;
        public bool Ascending;
        public string MethodName;
    }
}