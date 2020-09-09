using Careful.Core.Mvvm;
using System;

namespace Careful.Core.Ioc
{
    /// <summary>
    /// <see cref="IUnityContainer"/> extensions.
    /// </summary>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Registers an object for navigation
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type">The type of object to register</param>
        /// <param name="name">The unique name to register with the obect.</param>
        /// <returns><see cref="IUnityContainer"/></returns>
        public static ISimpleIoc RegisterTypeForNavigation(this ISimpleIoc container, Type type, string name)
        {
            return container.RegisterType(typeof(object), type, name);
        }
        public static ISimpleIoc RegisterType(this ISimpleIoc container, Type from, Type to, string name)
        {
            return null;
        }
        /// <summary>
        /// Gets the <see cref="IUnityContainer" /> from the <see cref="IContainerProvider" />
        /// </summary>
        /// <param name="containerProvider">The current <see cref="IContainerProvider" /></param>
        /// <returns>The underlying <see cref="IUnityContainer" /></returns>
        public static ISimpleIoc GetContainer(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<ISimpleIoc>)containerProvider).Instance;
        }

        /// <summary>
        /// Gets the <see cref="IUnityContainer" /> from the <see cref="IContainerProvider" />
        /// </summary>
        /// <param name="containerRegistry">The current <see cref="IContainerRegistry" /></param>
        /// <returns>The underlying <see cref="IUnityContainer" /></returns>
        public static ISimpleIoc GetContainer(this IContainerRegistry containerRegistry)
        {
            return ((IContainerExtension<ISimpleIoc>)containerRegistry).Instance;
        }
        /// <summary>
        /// Registers an object for navigation.
        /// </summary>
        /// <typeparam name="T">The Type of the object to register as the view</typeparam>
        /// <param name="container"></param>
        /// <param name="name">The unique name to register with the object.</param>
        public static ISimpleIoc RegisterTypeForNavigation<T>(this ISimpleIoc container, string name = null)
        {
            Type type = typeof(T);
            string viewName = string.IsNullOrWhiteSpace(name) ? type.Name : name;
            return container.RegisterTypeForNavigation(type, viewName);
        }

        /// <summary>
        /// Registers an object for navigation.
        /// </summary>
        /// <typeparam name="TView">The Type of object to register as the view</typeparam>
        /// <typeparam name="TViewModel">The ViewModel to use as the DataContext for the view</typeparam>
        /// <param name="name">The unique name to register with the view</param>
        /// <param name="container"></param>
        public static ISimpleIoc RegisterTypeForNavigation<TView, TViewModel>(this ISimpleIoc container, string name = null)
        {
            return container.RegisterTypeForNavigationWithViewModel<TViewModel>(typeof(TView), name);
        }

        private static ISimpleIoc RegisterTypeForNavigationWithViewModel<TViewModel>(this ISimpleIoc container, Type viewType, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = viewType.Name;

            ViewModelLocationProvider.Register(viewType.ToString(), typeof(TViewModel));

            return container.RegisterTypeForNavigation(viewType, name);
        }
    }
}
