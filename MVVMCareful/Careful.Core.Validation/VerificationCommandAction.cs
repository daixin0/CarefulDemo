using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Careful.Core.PropertyValidation
{
    public class VerificationCommandAction : TriggerAction<UIElement>
    {
        public ICommand CommandAction
        {
            get { return (ICommand)GetValue(CommandActionProperty); }
            set { SetValue(CommandActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandAction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandActionProperty =
            DependencyProperty.Register("CommandAction", typeof(ICommand), typeof(VerificationCommandAction));


        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(VerificationCommandAction));




        public object VerifivationModel
        {
            get { return (object)GetValue(VerifivationModelProperty); }
            set { SetValue(VerifivationModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VerifivationModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerifivationModelProperty =
            DependencyProperty.Register("VerifivationModel", typeof(object), typeof(VerificationCommandAction));


        public ICommand VerifivationResultCommand
        {
            get { return (ICommand)GetValue(VerifivationResultCommandProperty); }
            set { SetValue(VerifivationResultCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VerifivationResultCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VerifivationResultCommandProperty =
            DependencyProperty.Register("VerifivationResultCommand", typeof(ICommand), typeof(VerificationCommandAction));


        public Func<bool> CustomValidationAction
        {
            get { return (Func<bool>)GetValue(CustomValidationActionProperty); }
            set { SetValue(CustomValidationActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CustomValidationAction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomValidationActionProperty =
            DependencyProperty.Register("CustomValidationAction", typeof(Func<bool>), typeof(VerificationCommandAction));


        public int ValidationErrorCount
        {
            get { return (int)GetValue(ValidationErrorCountProperty); }
            set { SetValue(ValidationErrorCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValidationErrorCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValidationErrorCountProperty =
            DependencyProperty.Register("ValidationErrorCount", typeof(int), typeof(VerificationCommandAction));



        protected override void Invoke(object parameter)
        {
            if (VerifivationModel != null)
                if (VerificationHelper.VerifivationObject(VerifivationModel, new Action<ValidationResult>(p =>
                {
                    if (VerifivationResultCommand != null)
                        VerifivationResultCommand.Execute(p);
                })) == false) return;

            if (CustomValidationAction != null && CustomValidationAction.Invoke() == false)
                return;
            if (ValidationErrorCount > 0)
                return;
            CommandAction.Execute(CommandParameter);

        }
    }
}
