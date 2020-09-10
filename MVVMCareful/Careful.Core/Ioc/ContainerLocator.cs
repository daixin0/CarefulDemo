using System;
using System.ComponentModel;

namespace Careful.Core.Ioc
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ContainerLocator
    {
        private static Lazy<IContainerExtension> _lazyContainer;

        private static IContainerExtension _current;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IContainerExtension Current =>
            _current ?? (_current = _lazyContainer?.Value);

        public static IContainerProvider Container =>
            Current;

        public static void SetContainerExtension(Func<IContainerExtension> factory) =>
            _lazyContainer = new Lazy<IContainerExtension>(factory);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void ResetContainer()
        {
            _current = null;
            _lazyContainer = null;
        }
    }
}
