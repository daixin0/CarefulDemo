using Careful.Core.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Careful.Core.Mvvm.ViewInvokeCommand
{
    public class ContainerExtension 
    {
        public static Type GetWindowObject(string assemblyName)
        {
            IContainerProvider ContainerProvider = ServiceLocator.Current.GetInstance<IContainerProvider>();
            ICarefulIoc unityContainer = ContainerProvider.GetContainer();
            
            //foreach (var item in unityContainer.Registrations.ToList())
            //{
            //    if (item.RegisteredType.Name == assemblyName)
            //    {
            //        return item.RegisteredType;
            //    }
            //}
            return null;
        }
    }
}
