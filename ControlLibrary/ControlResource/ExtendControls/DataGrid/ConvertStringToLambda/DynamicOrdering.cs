using System.Linq.Expressions;

namespace ControlResource.ExtendControlStyle.DataGrid.ConvertStringToLambda
{
    internal class DynamicOrdering
    {
        public Expression Selector;
        public bool Ascending;
        public string MethodName;
    }
}