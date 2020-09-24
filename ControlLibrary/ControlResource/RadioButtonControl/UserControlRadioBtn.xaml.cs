using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Careful.Controls.RadioButtonControl
{
    /// <summary>
    /// UserControlRadioBtn.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlRadioBtn : UserControl
    {
        #region 属性
        private bool? isCheck;
        /// <summary>
        /// RadioBtn是否选中
        /// </summary>
        public bool? IsCheck
        {
            get { return isCheck; }
            set { isCheck = value; }
        }
        private bool hasCheck;
        /// <summary>
        /// 是否已经选中标志
        /// </summary>
        public bool HasCheck
        {
            get { return hasCheck; }
            set { hasCheck = value; }
        }

        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(UserControlRadioBtn));

        

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(UserControlRadioBtn));
        

        public string TextContent
        {
            get { return (string)GetValue(TextContentProperty); }
            set { SetValue(TextContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextContentProperty =
            DependencyProperty.Register("TextContent", typeof(string), typeof(UserControlRadioBtn));


        #endregion
        
        public UserControlRadioBtn()
        {
            InitializeComponent();
            
            this.DataContext = this;
          
        }
        public void SetCheck(bool? isCheck)
        {
            this.isCheck = isCheck;
            if (isCheck != null)
            {
                if (isCheck == true)
                {
                    this.chkState.IsChecked = true;
                    this.hasCheck = true;
                }
                else
                {
                    this.chkState.IsChecked = false;
                    this.hasCheck = false;
                }
            }
            else
            {
                this.chkState.IsChecked = false;
            }
        }
        private void chkState_Click(object sender, RoutedEventArgs e)
        {
            if (this.HasCheck == false)
            {
                this.HasCheck = true;
                this.IsChecked = true;
            }
            else
            {
                this.HasCheck = false;
                this.IsChecked = false;
            }
        }
        private void chkState_Unchecked(object sender, RoutedEventArgs e)
        {
            this.HasCheck = false;
        }
    }
}
