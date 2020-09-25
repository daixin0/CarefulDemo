using System.Linq.Expressions;
using System.Reflection;

namespace Careful.Controls.DataGridControl.ConvertStringToLambda
{
    internal class MethodData
    {
        public MethodBase MethodBase { get; set; }
        public ParameterInfo[] Parameters { get; set; }
        public Expression[] Args { get; set; }
    }
}
