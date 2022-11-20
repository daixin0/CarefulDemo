using Careful.Core.Ioc;
using Careful.Module.Core.Modularity;
using Careful.Module.Core.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Test.Views
{
    public class DesignerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            //regionManager.RegisterViewWithRegion("MenuToolView", typeof(MenuToolView));
            //regionManager.RegisterViewWithRegion("ActivityListView", typeof(ActivityListView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(DesignerMainView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.Register<ActivityListView>();
            //containerRegistry.Register<MenuToolView>();
        }
    }
}
