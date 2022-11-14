﻿/************************************************************************
   Careful.Controls.CarefulDockControl

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at https://opensource.org/licenses/MS-PL
 ************************************************************************/

namespace Careful.Controls.CarefulDockControl.Layout
{
	/// <summary>Interface definition for a <see cref="ILayoutElement"/> that supports a visibility property.</summary>
	public interface ILayoutPanelElement : ILayoutElement
	{
		/// <summary>Gets whether the <see cref="ILayoutElement"/> is currently visible or not.</summary>
		bool IsVisible { get; }
	}
}