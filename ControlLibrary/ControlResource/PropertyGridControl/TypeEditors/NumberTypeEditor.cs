using System.Windows;

namespace Careful.Controls.PropertyGridControl.TypeEditors
{
    public class NumberTypeEditor<T> : IntegerTypeEditor
    {

        public string Typ { get; set; }

        static NumberTypeEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberTypeEditor<T>), new FrameworkPropertyMetadata(typeof(IntegerTypeEditor)));
        }                                                                                                               

        public NumberTypeEditor()
            : base()
        {

            //_step = 2;
            //InitializeCommands();
        }
     
    }
}
