﻿using Careful.Controls.CarefulDockControl.Layout;
using System.ComponentModel;

namespace Careful.Controls.CarefulDockControl
{
	/// <summary>
	/// Implements a Cancelable event that can be raised to ask the client application whether closing this document
	/// and removing its content (viewmodel) is OK or not.
	/// </summary>
	public class DocumentClosingEventArgs : CancelEventArgs
	{
		/// <summary>
		/// Class constructor from the documents layout model.
		/// </summary>
		/// <param name="document"></param>
		public DocumentClosingEventArgs(LayoutDocument document)
		{
			Document = document;
		}

		/// <summary>
		/// Gets the model of the document that is about to be closed.
		/// </summary>
		public LayoutDocument Document { get; private set; }
	}
}