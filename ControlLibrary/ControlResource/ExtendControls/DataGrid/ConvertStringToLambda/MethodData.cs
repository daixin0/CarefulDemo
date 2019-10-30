using System.Linq.Expressions;
using System.Reflection;

namespace ControlResource.ExtendControlStyle.DataGrid.ConvertStringToLambda
{
    internal class MethodData
    {
        public MethodBase MethodBase { get; set; }
        public ParameterInfo[] Parameters { get; set; }
        public Expression[] Args { get; set; }
    }
}
