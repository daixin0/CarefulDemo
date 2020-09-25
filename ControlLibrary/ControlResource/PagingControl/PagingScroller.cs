using Careful.Core.Mvvm;
using Careful.Core.Mvvm.Command;
using Careful.Core.ObservableExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Careful.Controls.PagingControl
{
    public class PagingScroller: ItemsControl
    {
        static PagingScroller()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(PagingScroller),
                new FrameworkPropertyMetadata(typeof(PagingScroller)));

            FocusableProperty.OverrideMetadata(
                typeof(PagingScroller),
                new FrameworkPropertyMetadata(false));
        }

        public PagingScroller()
        {
            m_firstCommand = new DelegateCommand(First, CanFirst);
            m_previousCommand = new DelegateCommand(Previous, CanPrevious);
            m_nextCommand = new DelegateCommand(Next, CanNext);
            m_lastCommand = new DelegateCommand(Last, CanLast);
            m_goToPageCommand = new DelegateCommand(GoToPage, CanGoToPage);
        }

        public ICommand FirstCommand { get { return m_firstCommand; } }

        public ICommand PreviousCommand { get { return m_previousCommand; } }

        public ICommand GoToPageCommand { get { return m_goToPageCommand; } }

        public ICommand NextCommand { get { return m_nextCommand; } }

        public ICommand LastCommand { get { return m_lastCommand; } }

        public ReadOnlyObservableCollection<PagingCommandItem> Commands
        {
            get
            {
                return m_commandItems.ReadOnly;
            }
        }

        public static readonly DependencyProperty CommandItemTemplateProperty =
            DependencyProperty.Register("CommandItemTemplate", typeof(DataTemplate), typeof(PagingScroller));

        public DataTemplate CommandItemTemplate
        {
            get { return (DataTemplate)GetValue(CommandItemTemplateProperty); }
            set { SetValue(CommandItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty CommandItemTemplateSelectorProperty =
            DependencyProperty.Register("CommandItemTemplateSelector", typeof(DataTemplateSelector), typeof(PagingScroller));

        public DataTemplateSelector CommandItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(CommandItemTemplateSelectorProperty); }
            set { SetValue(CommandItemTemplateSelectorProperty, value); }
        }

        public int GoToPageIndex
        {
            get { return (int)GetValue(GoToPageIndexProperty); }
            set { SetValue(GoToPageIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GoToPageIndexProperty =
            DependencyProperty.Register("GoToPageIndex", typeof(int), typeof(PagingScroller),new PropertyMetadata(1));


        public int TotalNumber
        {
            get { return (int)GetValue(TotalNumberProperty); }
            set { SetValue(TotalNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalNumberProperty =
            DependencyProperty.Register("TotalNumber", typeof(int), typeof(PagingScroller));



        public int PageNumber
        {
            get { return (int)GetValue(PageNumberProperty); }
            set { SetValue(PageNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageNumberProperty =
            DependencyProperty.Register("PageNumber", typeof(int), typeof(PagingScroller),
                 new PropertyMetadata(new PropertyChangedCallback(pageNumber_changed)));



        public static readonly DependencyProperty CurrentItemIndexProperty =
            DependencyProperty.Register("CurrentItemIndex", typeof(int), typeof(PagingScroller),
            new PropertyMetadata(new PropertyChangedCallback(currentItemIndex_changed)));

        public int CurrentItemIndex
        {
            get { return (int)GetValue(CurrentItemIndexProperty); }
            set { SetValue(CurrentItemIndexProperty, value); }
        }


        public void First()
        {
            if (CanFirst())
            {
                CurrentItemIndex = 0;
            }
        }

        public void Previous()
        {
            if (CanPrevious())
            {
                CurrentItemIndex--;
            }
        }
        public void GoToPage()
        {
            if (CanGoToPage())
            {
                if (CurrentItemIndex == GoToPageIndex - 1)
                    return;
                if (GoToPageIndex > PageNumber || GoToPageIndex <= 0)
                    return;
                CurrentItemIndex = GoToPageIndex - 1;
            }
        }
        public void Next()
        {
            if (CanNext())
            {
                CurrentItemIndex++;
            }
        }

        public void Last()
        {
            if (CanLast())
            {
                CurrentItemIndex = (PageNumber - 1);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            FindZapDecorator();

            return base.MeasureOverride(availableSize);
        }

        public static readonly RoutedEvent CurrentItemIndexChangedEvent =
             EventManager.RegisterRoutedEvent("CurrentItemIndexChanged", RoutingStrategy.Bubble,
             typeof(RoutedPropertyChangedEventHandler<int>), typeof(PagingScroller));

        public event RoutedPropertyChangedEventHandler<int> CurrentItemChanged
        {
            add { base.AddHandler(CurrentItemIndexChangedEvent, value); }
            remove { base.RemoveHandler(CurrentItemIndexChangedEvent, value); }
        }



        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(PagingScroller));


        protected virtual void OnCurrentItemIndexChanged(int oldValue, int newValue)
        {
            ResetCommands();
            RoutedPropertyChangedEventArgs<int> args = new RoutedPropertyChangedEventArgs<int>(oldValue, newValue);
            args.RoutedEvent = CurrentItemIndexChangedEvent;
            base.RaiseEvent(args);
        }

        protected virtual void PageNumberChanged(int oldValue, int newValue)
        {
            ResetProperties();
        }

        #region Implementation

        private static void currentItemIndex_changed(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            PagingScroller zapScroller = (PagingScroller)element;
            zapScroller.OnCurrentItemIndexChanged((int)e.OldValue, (int)e.NewValue);
        }
        private static void pageNumber_changed(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            PagingScroller zapScroller = (PagingScroller)element;
            zapScroller.PageNumberChanged((int)e.OldValue, (int)e.NewValue);
        }

        private void ResetEdgeCommands()
        {
            m_firstCommand.RaiseCanExecuteChanged();
            m_lastCommand.RaiseCanExecuteChanged();
            m_nextCommand.RaiseCanExecuteChanged();
            m_previousCommand.RaiseCanExecuteChanged();
            m_goToPageCommand.RaiseCanExecuteChanged();
        }

        private void ResetCommands()
        {
            ResetEdgeCommands();

            int parentItemsCount = this.PageNumber;
            m_commandItems.Clear();

            if (parentItemsCount <= 9)
            {
                for (int i = 0; i < parentItemsCount; i++)
                {
                    m_commandItems.Add(new PagingCommandItem(this, i, (i + 1).ToString(),i));
                }
            }
            else
            {
                if (CurrentItemIndex < 4)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (i == 6)
                        {
                            m_commandItems.Add(new PagingCommandItem(this, i, "...", i));
                        }
                        else if (i == 7)
                        {
                            m_commandItems.Add(new PagingCommandItem(this, parentItemsCount - 1, parentItemsCount.ToString(),i));
                        }
                        else
                        {
                            m_commandItems.Add(new PagingCommandItem(this, i, (i + 1).ToString(),i));
                        }

                    }
                }
                else if (CurrentItemIndex > parentItemsCount - 5)
                {
                    int buttonIndex = 0;
                    for (int i = 0; i < parentItemsCount; i++)
                    {
                        if (i == 1)
                        {
                            m_commandItems.Add(new PagingCommandItem(this, i, "...", buttonIndex,true));
                            buttonIndex++;
                        }
                        else if (i > parentItemsCount - 7 || i == 0)
                        {
                            m_commandItems.Add(new PagingCommandItem(this, i, (i + 1).ToString(), buttonIndex));
                            buttonIndex++;
                        }

                    }
                }
                else
                {
                    int buttonIndex = 0;
                    for (int i = 0; i < parentItemsCount; i++)
                    {
                        if (i == 1)
                        {
                            m_commandItems.Add(new PagingCommandItem(this, i, "...", buttonIndex, true));
                            buttonIndex++;
                        }
                        else if (i == parentItemsCount - 2)
                        {
                            m_commandItems.Add(new PagingCommandItem(this, i, "...", buttonIndex));
                            buttonIndex++;
                        }
                        else if (i == parentItemsCount - 1 || i == 0)
                        {
                            m_commandItems.Add(new PagingCommandItem(this, i, (i + 1).ToString(), buttonIndex));
                            buttonIndex++;
                        }
                        else if (i - 2 <= CurrentItemIndex && i + 2 >= CurrentItemIndex)
                        {
                            m_commandItems.Add(new PagingCommandItem(this, i, (i + 1).ToString(), buttonIndex));
                            buttonIndex++;
                        }

                    }
                }
                
            }
            //if (m_commandItems.Count > 0)
            //{
            //    if (SelectedIndex < m_commandItems.Count && SelectedIndex >= 0)
            //        m_commandItems[SelectedIndex].Content = Items[CurrentItemIndex];
            //    else
            //    {
            //        if (SelectedIndex >= m_commandItems.Count)
            //        {
            //            m_commandItems[m_commandItems.Count - 1].Content = Items[CurrentItemIndex];
            //        }
            //        if (SelectedIndex < 0)
            //        {
            //            m_commandItems[0].Content = Items[CurrentItemIndex];
            //        }
            //    }
            //}
        }

        private void FindZapDecorator()
        {
            if (this.Template != null)
            {
                PagingDecorator temp = this.Template.FindName(PART_ZapDecorator, this) as PagingDecorator;
                if (m_zapDecorator != temp)
                {
                    m_zapDecorator = temp;
                    if (m_zapDecorator != null)
                    {
                        Binding binding = new Binding("CurrentItemIndex");
                        binding.Source = this;
                        m_zapDecorator.SetBinding(PagingDecorator.TargetIndexProperty, binding);
                    }
                }
                else
                {
                    Debug.WriteLine("No element with name '" + PART_ZapDecorator + "' in the template.");
                }
            }
            else
            {
                Debug.WriteLine("No template defined for ZapScroller.");
            }
        }

        private void ResetProperties()
        {
            //if (m_internalItemCollection.Count != PageNumber)
            //{
            //    SetValue(PageNumberProperty, m_internalItemCollection.Count);
            //}
            if (CurrentItemIndex >= PageNumber)
            {
                CurrentItemIndex = (PageNumber - 1);
            }
            else if (CurrentItemIndex == -1 && PageNumber > 0)
            {
                CurrentItemIndex = 0;
            }

            ResetCommands();
        }

        private bool CanFirst()
        {
            return (PageNumber > 1) && (CurrentItemIndex > 0);
        }

        private bool CanNext()
        {
            return (CurrentItemIndex >= 0) && CurrentItemIndex < (PageNumber - 1);
        }

        private bool CanPrevious()
        {
            return CurrentItemIndex > 0;
        }
        private bool CanGoToPage()
        {

            return (CurrentItemIndex >= 0) && CurrentItemIndex < (PageNumber - 1);
        }
        private bool CanLast()
        {
            return (PageNumber > 1) && (CurrentItemIndex < (PageNumber - 1));
        }

        //private ItemCollection m_internalItemCollection;

        private PagingDecorator m_zapDecorator;

        private readonly DelegateCommand m_firstCommand, m_previousCommand, m_nextCommand, m_lastCommand,m_goToPageCommand;

        private readonly ObservableCollectionSynchronize<PagingCommandItem> m_commandItems = new ObservableCollectionSynchronize<PagingCommandItem>();

        #endregion

        public const string PART_ZapDecorator = "PART_ZapDecorator";
    }
}
