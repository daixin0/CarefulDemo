using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Careful.Core.Mvvm.BindingExtension
{
    [MarkupExtensionReturnType(typeof(object))]
    public class BindingMarkupExtensionBase : MarkupExtension
    {
        private Binding binding = new Binding();

        #region properties

        [Browsable(false)]
        public Binding Binding
        {
            get { return binding; }
            set { binding = value; }
        }


        [DefaultValue(null)]
        public object AsyncState
        {
            get { return binding.AsyncState; }
            set { binding.AsyncState = value; }
        }

        [DefaultValue(false)]
        public bool BindsDirectlyToSource
        {
            get { return binding.BindsDirectlyToSource; }
            set { binding.BindsDirectlyToSource = value; }
        }

        [DefaultValue(null)]
        public IValueConverter Converter
        {
            get { return binding.Converter; }
            set { binding.Converter = value; }
        }

        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter)), DefaultValue(null)]
        public CultureInfo ConverterCulture
        {
            get { return binding.ConverterCulture; }
            set { binding.ConverterCulture = value; }
        }

        [DefaultValue(null)]
        public object ConverterParameter
        {
            get { return binding.ConverterParameter; }
            set { binding.ConverterParameter = value; }
        }

        [DefaultValue(null)]
        public string ElementName
        {
            get { return binding.ElementName; }
            set { binding.ElementName = value; }
        }

        [DefaultValue(null)]
        public object FallbackValue
        {
            get { return binding.FallbackValue; }
            set { binding.FallbackValue = value; }
        }

        [DefaultValue(false)]
        public bool IsAsync
        {
            get { return binding.IsAsync; }
            set { binding.IsAsync = value; }
        }

        [DefaultValue(BindingMode.Default)]
        public BindingMode Mode
        {
            get { return binding.Mode; }
            set { binding.Mode = value; }
        }

        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated
        {
            get { return binding.NotifyOnSourceUpdated; }
            set { binding.NotifyOnSourceUpdated = value; }
        }

        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated
        {
            get { return binding.NotifyOnTargetUpdated; }
            set { binding.NotifyOnTargetUpdated = value; }
        }

        [DefaultValue(false)]
        public bool NotifyOnValidationError
        {
            get { return binding.NotifyOnValidationError; }
            set { binding.NotifyOnValidationError = value; }
        }

        [DefaultValue(null)]
        public PropertyPath Path
        {
            get { return binding.Path; }
            set { binding.Path = value; }
        }

        [DefaultValue(null)]
        public RelativeSource RelativeSource
        {
            get { return binding.RelativeSource; }
            set { binding.RelativeSource = value; }
        }

        [DefaultValue(null)]
        public object Source
        {
            get { return binding.Source; }
            set { binding.Source = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter
        {
            get { return binding.UpdateSourceExceptionFilter; }
            set { binding.UpdateSourceExceptionFilter = value; }
        }

        [DefaultValue(UpdateSourceTrigger.Default)]
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get { return binding.UpdateSourceTrigger; }
            set { binding.UpdateSourceTrigger = value; }
        }

        [DefaultValue(false)]
        public bool ValidatesOnDataErrors
        {
            get { return binding.ValidatesOnDataErrors; }
            set { binding.ValidatesOnDataErrors = value; }
        }

        [DefaultValue(false)]
        public bool ValidatesOnExceptions
        {
            get { return binding.ValidatesOnExceptions; }
            set { binding.ValidatesOnExceptions = value; }
        }

        [DefaultValue(null)]
        public string XPath
        {
            get { return binding.XPath; }
            set { binding.XPath = value; }
        }

        [DefaultValue(null)]
        public Collection<ValidationRule> ValidationRules
        {
            get { return binding.ValidationRules; }
        }


        #endregion

        public override object ProvideValue(IServiceProvider provider)
        {
            return binding.ProvideValue(provider);
        }
    }
}
