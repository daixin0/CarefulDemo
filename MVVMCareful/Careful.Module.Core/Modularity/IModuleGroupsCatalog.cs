using System.Collections.ObjectModel;

namespace Careful.Module.Core.Modularity
{
    public interface IModuleGroupsCatalog
    {
        Collection<IModuleCatalogItem> Items { get; }
    }
}
