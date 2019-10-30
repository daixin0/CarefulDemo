using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ControlResource.ExtendControlStyle.MultiComboBox
{
    public partial class MultiComboBox : ComboBox
    {
        /// <summary>
        /// 获取选择项集合
        /// </summary>
        public IList SelectedItems
        {
            get { return this._ListBox.SelectedItems; }
        }


        //public IList SelectedItems
        //{
        //    get { return (IList)GetValue(SelectedItemsProperty); }
        //    set { SetValue(SelectedItemsProperty, this._ListBox.SelectedItems); }
        //}

        //// Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SelectedItemsProperty =
        //    DependencyProperty.Register("SelectedItems", typeof(IList), typeof(MultiComboBox));


        /// <summary>
        /// 设置或获取选择项
        /// </summary>
        public new int SelectedIndex
        {
            get { return this._ListBox.SelectedIndex; }
            set { this._ListBox.SelectedIndex = value; }
        }


        public static readonly RoutedEvent SelectedCheckEvent = EventManager.RegisterRoutedEvent
                     ("SelectedCheck", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(MultiComboBox));

        public event RoutedEventHandler SelectedCheck
        {
            add { this.AddHandler(SelectedCheckEvent, value); }
            remove { this.RemoveHandler(SelectedCheckEvent, value); }
        }


        public static readonly RoutedEvent ButtonCheckEvent = EventManager.RegisterRoutedEvent
                     ("ButtonCheck", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(MultiComboBox));

        public event RoutedEventHandler ButtonCheck
        {
            add { this.AddHandler(ButtonCheckEvent, value); }
            remove { this.RemoveHandler(ButtonCheckEvent, value); }
        }

        private ListBox _ListBox;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //OverridesDefaultStyleProperty.OverrideMetadata(typeof(MultiComboBox), new FrameworkPropertyMetadata(typeof(MultiComboBox)));

            this._ListBox = Template.FindName("PART_ListBox", this) as ListBox;
            this._ListBox.SelectionChanged -= _ListBox_SelectionChanged;
            this._ListBox.SelectionChanged += _ListBox_SelectionChanged;
            
            

            ToggleButton btnDown = Template.FindName("PART_DropDownToggle", this) as ToggleButton;
            btnDown.Checked += BtnDown_Checked;
        }
        

        private void BtnDown_Checked(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(MultiComboBox.ButtonCheckEvent, sender);
            this.RaiseEvent(routedEventArgs);
        }

        void _ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.OriginalSource is ListBox)
            {
                ListBox lb = e.OriginalSource as ListBox;
                if (lb.Items.Count <= 0)
                    return;
            }
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(MultiComboBox.SelectedCheckEvent, sender);
            this.RaiseEvent(routedEventArgs);

        }

        public MultiComboBox()
        {
            this.DefaultStyleKey = typeof(MultiComboBox);
        }

        public void SelectAll()
        {
            this._ListBox.SelectAll();

        }

        public void UnselectAll()
        {
            this._ListBox.UnselectAll();
        }
    }
}
