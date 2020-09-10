// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PrismCommonLib.Mvvm;
#if NETFX_CORE
using Windows.UI.Xaml;
#endif
namespace PrismCommonLib.Mvvm
{
    /// <summary>
    /// This class defines the attached property and related change handler that calls the ViewModelLocator in Prism.Mvvm.
    /// </summary>
    public static class ViewModelLocator
    {
        #region Attached property with convention-or-mapping based approach

        /// <summary>
        /// The AutoWireViewModel attached property.
        /// </summary>
        public static DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator),
            new PropertyMetadata(false, AutoWireViewModelChanged));

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as IView;

            if (view == null)
            {
                // TODO: Improve exception
                throw new Exception("Your views must implement IView");
            }

            ViewModelLocationProvider.AutoWireViewModelChanged(view);
        }

        /// <summary>
        /// Gets the value of the AutoWireViewModel attached property.
        /// </summary>
        /// <param name="obj">The dependency object that has this attached property.</param>
        /// <returns><c>True</c> if view model autowiring is enabled; otherwise, <c>false</c>.</returns>
        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            if (obj != null)
            {
                return (bool)obj.GetValue(AutoWireViewModelProperty);
            }
            return false;
        }

        /// <summary>
        /// Sets the value of the AutoWireViewModel attached property.
        /// </summary>
        /// <param name="obj">The dependency object that has this attached property.</param>
        /// <param name="value">if set to <c>true</c> the view model wiring will be performed.</param>
        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            if (obj != null)
            {
                obj.SetValue(AutoWireViewModelProperty, value);
            }
        }

        #endregion
    }
}
