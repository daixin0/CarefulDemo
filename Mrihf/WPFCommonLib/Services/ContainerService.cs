using CommonLib;
using CommonLib.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFCommonLib.Helpers;
using WPFCommonLib.Views.WindowContainerControl;

namespace WPFCommonLib.Services
{
    public class ContainerService: IContainerService
    {
        public WindowContainer ContainerContent { get; set; }

        public void ChangeContent(string contentFullqualifiedName, bool isAnimation = true, object obj = null)
        {
            var controlType = ApplicationHelper.GetTargetType(contentFullqualifiedName);
            if (controlType == null)
            {
                ContainerContent.Content = null;
                return;
            }
            var control = Activator.CreateInstance(controlType) as UserControl;
            if (control == null)
            {
                return;
            }
            if (control.DataContext != null && control.DataContext is IContainerParameter)
            {
                (control.DataContext as IContainerParameter).ContainerContent = ContainerContent;
                (control.DataContext as IContainerParameter).ParameterItem = obj;
            }

            ContainerContent.IsStartAnimation = isAnimation;
                ContainerContent.Content = control;
            
           
        }
        
        public void ChangeContent(string contentFullqualifiedName, WindowContainer containerContent)
        {
            if (containerContent != null)
                ContainerContent = containerContent;
            var controlType = ApplicationHelper.GetTargetType(contentFullqualifiedName);
            if (controlType == null)
            {
                ContainerContent.Content = null;
                return;
            }

            var control = Activator.CreateInstance(controlType) as UserControl;
            if (control == null)
            {
                return;
            }
            if (control.DataContext != null && control.DataContext is IContainerParameter)
            {
                (control.DataContext as IContainerParameter).ContainerContent = ContainerContent;
                
            }
            ContainerContent.Content = control;
        }
        
        public void ChangeContent(string contentFullqualifiedName, WindowContainer containerContent = null, object obj = null, Func<bool> closeFunc = null)
        {
            if (containerContent != null)
                ContainerContent = containerContent;
            var controlType = ApplicationHelper.GetTargetType(contentFullqualifiedName);
            if (controlType == null)
            {
                ContainerContent.Content = null;
                return;
            }

            var control = Activator.CreateInstance(controlType) as UserControl;
            if (control == null)
            {
                return;
            }
            if (control.DataContext != null && control.DataContext is IContainerParameter)
            {
                (control.DataContext as IContainerParameter).ContainerContent = ContainerContent;
                (control.DataContext as IContainerParameter).ParameterItem = obj;
            }
            if (control.DataContext != null && control.DataContext is IViewModelCloseParameter)
            {
                (control.DataContext as IViewModelCloseParameter).ContainerContent = ContainerContent;
                (control.DataContext as IViewModelCloseParameter).ParameterItem = obj;
                (control.DataContext as IViewModelCloseParameter).CloseFunc = closeFunc;
            }
            //ContainerContent = new WindowContainer() { Content = control };
            ContainerContent.Content = control;
        }

    }
}
