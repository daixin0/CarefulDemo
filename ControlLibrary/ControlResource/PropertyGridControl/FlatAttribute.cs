using System;
using System.Collections.Generic;
using System.Text;

namespace Careful.Controls.PropertyGridControl
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class FlatAttribute : Attribute { }
}
