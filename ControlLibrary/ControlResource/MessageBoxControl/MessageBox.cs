using Careful.Core.DialogServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Careful.Controls.MessageBoxControl
{
    public class MessageBox : Control
    {

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MessageBox));


        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBox));



        public double Button1Width
        {
            get { return (double)GetValue(Button1WidthProperty); }
            set { SetValue(Button1WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Button1Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Button1WidthProperty =
            DependencyProperty.Register("Button1Width", typeof(double), typeof(MessageBox), new PropertyMetadata(60.0));



        public double Button2Width
        {
            get { return (double)GetValue(Button2WidthProperty); }
            set { SetValue(Button2WidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Button2Width.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Button2WidthProperty =
            DependencyProperty.Register("Button2Width", typeof(double), typeof(MessageBox), new PropertyMetadata(60.0));




        public ButtonResult MessageResult
        {
            get { return (ButtonResult)GetValue(MessageResultProperty); }
            set { SetValue(MessageResultProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageResule.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageResultProperty =
            DependencyProperty.Register("MessageResult", typeof(ButtonResult), typeof(MessageBox));



        public MessageButtonType MessageButtonTextType
        {
            get { return (MessageButtonType)GetValue(MessageButtonTextTypeProperty); }
            set { SetValue(MessageButtonTextTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageButtonTextType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageButtonTextTypeProperty =
            DependencyProperty.Register("MessageButtonTextType", typeof(MessageButtonType), typeof(MessageBox));



        public MessageBoxType MessageType
        {
            get { return (MessageBoxType)GetValue(MessageTypeProperty); }
            set { SetValue(MessageTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageTypeProperty =
            DependencyProperty.Register("MessageType", typeof(MessageBoxType), typeof(MessageBox));


        public MessageBoxButton MessageButtonType
        {
            get { return (MessageBoxButton)GetValue(MessageButtonTypeProperty); }
            set { SetValue(MessageButtonTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageButtonType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageButtonTypeProperty =
            DependencyProperty.Register("MessageButtonType", typeof(MessageBoxButton), typeof(MessageBox));



        public Geometry MessagePath
        {
            get { return (PathGeometry)GetValue(MessagePathProperty); }
            set { SetValue(MessagePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessagePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessagePathProperty =
            DependencyProperty.Register("MessagePath", typeof(Geometry), typeof(MessageBox));



        public PathGeometry TitlePath
        {
            get { return (PathGeometry)GetValue(TitlePathProperty); }
            set { SetValue(TitlePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TitlePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitlePathProperty =
            DependencyProperty.Register("TitlePath", typeof(PathGeometry), typeof(MessageBox));



        public string DetermineText
        {
            get { return (string)GetValue(DetermineTextProperty); }
            set { SetValue(DetermineTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DetermineText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DetermineTextProperty =
            DependencyProperty.Register("DetermineText", typeof(string), typeof(MessageBox));



        public string CancelText
        {
            get { return (string)GetValue(CancelTextProperty); }
            set { SetValue(CancelTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelTextProperty =
            DependencyProperty.Register("CancelText", typeof(string), typeof(MessageBox));



        public Visibility DetermineVisiable
        {
            get { return (Visibility)GetValue(DetermineVisiableProperty); }
            set { SetValue(DetermineVisiableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DetermineVisiable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DetermineVisiableProperty =
            DependencyProperty.Register("DetermineVisiable", typeof(Visibility), typeof(MessageBox));


        public Visibility CancelVisiable
        {
            get { return (Visibility)GetValue(CancelVisiableProperty); }
            set { SetValue(CancelVisiableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelVisiable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelVisiableProperty =
            DependencyProperty.Register("CancelVisiable", typeof(Visibility), typeof(MessageBox));


        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler Close
        {
            add { AddHandler(CloseEvent, value); }
            remove { RemoveHandler(CloseEvent, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedEvent CloseEvent = EventManager.RegisterRoutedEvent(
            "Close", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MessageBox));



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            DetermineVisiable = Visibility.Collapsed;
            CancelVisiable = Visibility.Collapsed;
            switch (MessageType)
            {
                case MessageBoxType.Information:
                    MessageButtonType = MessageBoxButton.Ok;
                    DetermineVisiable = Visibility.Visible;
                    DetermineText = "确定";
                    MessagePath = this.Template.Resources["pathInformation"] as StreamGeometry;
                    break;
                case MessageBoxType.Question:
                    MessageButtonType = MessageBoxButton.YesNo;
                    DetermineVisiable = Visibility.Visible;
                    CancelVisiable = Visibility.Visible;
                    switch (MessageButtonTextType)
                    {
                        case Core.DialogServices.MessageButtonType.Default:
                            DetermineText = "是";
                            CancelText = "否";
                            break;
                        case Core.DialogServices.MessageButtonType.Custom:
                            break;
                    }

                    MessagePath = this.Template.Resources["pathQuestion"] as StreamGeometry;
                    break;
                case MessageBoxType.Confirm:
                    MessageButtonType = MessageBoxButton.OkCancel;
                    DetermineVisiable = Visibility.Visible;
                    CancelVisiable = Visibility.Visible;
                    switch (MessageButtonTextType)
                    {
                        case Core.DialogServices.MessageButtonType.Default:
                            DetermineText = "确定";
                            CancelText = "取消";
                            break;
                        case Core.DialogServices.MessageButtonType.Custom:
                            break;
                    }

                    MessagePath = this.Template.Resources["pathInformation"] as StreamGeometry;
                    break;
            }

            Button okButton = this.Template.FindName("btnDetermine", this) as Button;
            Button cancelButton = this.Template.FindName("btnCancel", this) as Button;

            okButton.Click -= OkButton_Click;
            cancelButton.Click -= CancelButton_Click;
            okButton.Click += OkButton_Click;
            cancelButton.Click += CancelButton_Click;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageButtonType == MessageBoxButton.OkCancel)
                MessageResult = ButtonResult.Cancel;
            else if (MessageButtonType == MessageBoxButton.YesNo)
                MessageResult = ButtonResult.No;

            MessageBoxEventArgs messageBoxEventArgs = new MessageBoxEventArgs(MessageBox.CloseEvent, MessageResult);
            this.RaiseEvent(messageBoxEventArgs);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageButtonType == MessageBoxButton.OkCancel || MessageButtonType == MessageBoxButton.Ok)
                MessageResult = ButtonResult.OK;
            else if (MessageButtonType == MessageBoxButton.YesNo)
                MessageResult = ButtonResult.Yes;
            MessageBoxEventArgs messageBoxEventArgs = new MessageBoxEventArgs(MessageBox.CloseEvent, MessageResult);
            this.RaiseEvent(messageBoxEventArgs);
        }
    }
}
