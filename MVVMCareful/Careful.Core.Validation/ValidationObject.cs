using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Careful.Core.PropertyValidation
{
    public class ValidationObject: DependencyObject
    {
        public object ValidationModel
        {
            get { return (object)GetValue(ValidationModelProperty); }
            set { SetValue(ValidationModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValidationModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationModelProperty =
            DependencyProperty.Register("ValidationModel", typeof(object), typeof(ValidationObject));


        public string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register("PropertyName", typeof(string), typeof(ValidationObject));




        public Func<object, bool> CustomValidationAction
        {
            get { return (Func<object, bool>)GetValue(CustomValidationActionProperty); }
            set { SetValue(CustomValidationActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomValidationAction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomValidationActionProperty =
            DependencyProperty.Register("CustomValidationAction", typeof(Func<object, bool>), typeof(ValidationObject));


    }
}
