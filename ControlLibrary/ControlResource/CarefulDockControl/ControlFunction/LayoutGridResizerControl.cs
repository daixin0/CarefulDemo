﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Careful.Controls.CarefulDockControl.ControlFunction
{
	/// <inheritdoc />
	/// <summary>Implements a control like <see cref="System.Windows.Controls.GridSplitter"/> that can be used to resize areas
	/// horizontally or vertically (only one of these but never both) in a grid layout.</summary>
	/// <seealso cref="Thumb"/>
	public class LayoutGridResizerControl : Thumb
	{
		#region Constructors

		public LayoutGridResizerControl()
        {
			ResourceDictionary style1 = new ResourceDictionary();
			style1.Source = new Uri("Careful.Controls;component/CarefulDockControl/Style/generic.xaml", UriKind.Relative);
			this.Style = (System.Windows.Style)style1["LayoutGridResizerControlStyle"];

		}
        static LayoutGridResizerControl()
        {
            //This OverrideMetadata call tells the system that this element wants to provide a style that is different than its base class.
            //This style is defined in themes\generic.xaml
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(typeof(LayoutGridResizerControl)));
            HorizontalAlignmentProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(HorizontalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
            VerticalAlignmentProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(VerticalAlignment.Stretch, FrameworkPropertyMetadataOptions.AffectsParentMeasure));
            BackgroundProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(Brushes.Transparent));
            IsHitTestVisibleProperty.OverrideMetadata(typeof(LayoutGridResizerControl), new FrameworkPropertyMetadata(true, null));
        }

        #endregion Constructors

        #region Properties

        #region BackgroundWhileDragging

        /// <summary><see cref="BackgroundWhileDragging"/> dependency property.</summary>
        public static readonly DependencyProperty BackgroundWhileDraggingProperty = DependencyProperty.Register(nameof(BackgroundWhileDragging), typeof(Brush), typeof(LayoutGridResizerControl),
				new FrameworkPropertyMetadata(Brushes.Black));

		/// <summary>Gets/sets the background brush of the control being dragged.</summary>
		[Bindable(true), Description("Gets/sets the background brush of the control being dragged."), Category("Other")]
		public Brush BackgroundWhileDragging
		{
			get => (Brush)GetValue(BackgroundWhileDraggingProperty);
			set => SetValue(BackgroundWhileDraggingProperty, value);
		}

		#endregion BackgroundWhileDragging

		#region OpacityWhileDragging

		/// <summary><see cref="OpacityWhileDragging"/> dependency property.</summary>
		public static readonly DependencyProperty OpacityWhileDraggingProperty = DependencyProperty.Register(nameof(OpacityWhileDragging), typeof(double), typeof(LayoutGridResizerControl),
				new FrameworkPropertyMetadata(0.5));

		/// <summary>Gets/sets the opacity while the control is being dragged.</summary>
		[Bindable(true), Description("Gets/sets the opacity while the control is being dragged."), Category("Other")]
		public double OpacityWhileDragging
		{
			get => (double)GetValue(OpacityWhileDraggingProperty);
			set => SetValue(OpacityWhileDraggingProperty, value);
		}

		#endregion OpacityWhileDragging

		#endregion Properties
	}
}