using CommonLib;
using WPFCommonLib.Views.WindowContainerControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WPFCommonLib
{
    public interface IViewModelParameter
    {
        object ParameterItem { get; set; }
       

    }
    public interface IContainerParameter :  IViewModelParameter
    {
        WindowContainer ContainerContent { get; set; }
        
    }
    public interface IViewModelCloseParameter : IViewModelParameter
    {
        WindowContainer ContainerContent { get; set; }
        Func<bool> CloseFunc { get; set; }
    }
}
