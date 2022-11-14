﻿using Careful.Controls.CarefulDockControl.Layout;
using System;

namespace Careful.Controls.CarefulDockControl
{
	/// <summary>
	/// Implements an event that can be raised to inform the client application about a
	/// document that been closed and removed its content (viewmodel) from the docking framework.
	/// </summary>
	public class DocumentClosedEventArgs : EventArgs
	{
		/// <summary>
		/// Class constructor from the documents layout model.
		/// </summary>
		/// <param name="document"></param>
		public DocumentClosedEventArgs(LayoutDocument document)
		{
			Document = document;
		}

		/// <summary>
		/// Gets the model of the document that has been closed.
		/// </summary>
		public LayoutDocument Document { get; private set; }
	}
}