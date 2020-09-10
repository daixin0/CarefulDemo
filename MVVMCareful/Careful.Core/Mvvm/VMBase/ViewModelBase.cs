using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
//using WPFCommonLib.Messaging;

// ReSharper disable RedundantUsingDirective
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

////using GalaSoft.Utilities.Attributes;

namespace Careful.Core.Mvvm.VMBase
{
    /// <summary>
    /// A base class for the ViewModel classes in the Mvvm pattern.
    /// </summary>
    //// [ClassInfo(typeof(ViewModelBase),
    ////  VersionString = "5.3.18",
    ////  DateString = "201604212130",
    ////  Description = "A base class for the ViewModel classes in the Mvvm pattern.",
    ////  UrlContacts = "http://www.galasoft.ch/contact_en.html",
    ////  Email = "laurent@galasoft.ch")]
    [SuppressMessage(
        "Microsoft.Design",
        "CA1012",
        Justification = "Constructors should remain public to allow serialization.")]
    public abstract class ViewModelBase : NotifyPropertyChanged, ICleanup
    {
        /// <summary>
        /// Unregisters this instance from the Messenger class.
        /// <para>To cleanup additional resources, override this method, clean
        /// up and then call base.Cleanup().</para>
        /// </summary>
        public virtual void Cleanup()
        {
            //MessengerInstance.Unregister(this);
        }

    }
}
