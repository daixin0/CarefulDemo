using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Careful.Controls.Themes
{
	public abstract class Theme : DependencyObject
	{
		/// <summary>Class constructor</summary>
		public Theme()
		{
		}

		/// <summary>Gets the <see cref="Uri"/> of the XAML that contains the definition for this AvalonDock theme.</summary>
		/// <returns><see cref="Uri"/> of the XAML that contains the definition for this custom AvalonDock theme</returns>
		public abstract Uri GetResourceUri();
	}
}
