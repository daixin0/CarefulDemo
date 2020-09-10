using System.ComponentModel;
using System.Windows;

namespace Careful.Core.Mvvm
{
    public partial class ObservableObject<T> : FrameworkElement, INotifyPropertyChanged
    {
        /// <summary>
        /// Identifies the Value property of the ObservableObject
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "This is the pattern for WPF dependency properties")]
        public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(T), typeof(ObservableObject<T>), new PropertyMetadata(null, ValueChangedCallback));

        /// <summary>
        /// Event that gets invoked when the Value property changes. 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The value that's wrapped inside the ObservableObject.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public T Value
        {
            get { return (T)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ObservableObject<T> thisInstance = ((ObservableObject<T>)d);
            PropertyChangedEventHandler eventHandler = thisInstance.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(thisInstance, new PropertyChangedEventArgs("Value"));
            }
        }
    }
}