﻿/************************************************************************
   Careful.Controls.CarefulDockControl

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at https://opensource.org/licenses/MS-PL
 ************************************************************************/

using System;

namespace Careful.Controls.CarefulDockControl.ControlFunction
{
	internal class WindowActivateEventArgs : EventArgs
	{
		public WindowActivateEventArgs(IntPtr hwndActivating)
		{
			HwndActivating = hwndActivating;
		}

		public IntPtr HwndActivating { get; private set; }
	}
}