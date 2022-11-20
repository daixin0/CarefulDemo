using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Module.Test.Views
{
    /// <summary>
    /// MenuToolView.xaml 的交互逻辑
    /// </summary>
    public partial class MenuToolView : UserControl
    {
        public MenuToolView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler NewFlow
        {
            add { AddHandler(NewFlowEvent, value); }
            remove { RemoveHandler(NewFlowEvent, value); }
        }

        public static readonly RoutedEvent NewFlowEvent = EventManager.RegisterRoutedEvent(
            "NewFlow", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuToolView));


        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler SaveFlow
        {
            add { AddHandler(SaveFlowEvent, value); }
            remove { RemoveHandler(SaveFlowEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent SaveFlowEvent = EventManager.RegisterRoutedEvent(
            "SaveFlow", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuToolView));



        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler OpenFlow
        {
            add { AddHandler(OpenFlowEvent, value); }
            remove { RemoveHandler(OpenFlowEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent OpenFlowEvent = EventManager.RegisterRoutedEvent(
            "OpenFlow", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MenuToolView));



        public void NewFlowMethod()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(MenuToolView.NewFlowEvent, this);
            this.RaiseEvent(routedEventArgs);
        }
        public void SaveFlowMethod()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(MenuToolView.SaveFlowEvent, this);
            this.RaiseEvent(routedEventArgs);
        }
        public void OpenFlowMethod()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(MenuToolView.OpenFlowEvent, this);
            this.RaiseEvent(routedEventArgs);
        }

    }
}
