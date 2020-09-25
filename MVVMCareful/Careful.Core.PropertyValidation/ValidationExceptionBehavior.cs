using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Careful.Core.PropertyValidation
{
    public class ValidationExceptionBehavior : Behavior<FrameworkElement>
    {
        public string RegionName
        {
            get { return (string)GetValue(RegionNameProperty); }
            set { SetValue(RegionNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RegionName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RegionNameProperty =
            DependencyProperty.Register("RegionName", typeof(string), typeof(ValidationExceptionBehavior));



        public Action<string, int> ValidationRegionResult
        {
            get { return (Action<string, int>)GetValue(ValidationRegionResultProperty); }
            set { SetValue(ValidationRegionResultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValidationRegionResult.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationRegionResultProperty =
            DependencyProperty.Register("ValidationRegionResult", typeof(Action<string, int>), typeof(ValidationExceptionBehavior));



        public int ErrorCount
        {
            get { return (int)GetValue(ErrorCountProperty); }
            set { SetValue(ErrorCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ErrorCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ErrorCountProperty =
            DependencyProperty.Register("ErrorCount", typeof(int), typeof(ValidationExceptionBehavior),new PropertyMetadata(0));

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if(e.Property.Name== "ErrorCount")
            {
                if (ValidationRegionResult != null)
                    ValidationRegionResult(RegionName, ErrorCount);
            }
        }

        protected override void OnAttached()
        {
            this.AssociatedObject.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(OnValidationError));
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            BindingExpression bindingExpression = e.Error.BindingInError as BindingExpression;
            ErrorCount = bindingExpression.ValidationErrors == null ? 0 : bindingExpression.ValidationErrors.Count;
        }
    }
}
