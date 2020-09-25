using Careful.Core.Ioc;
using Careful.Core.Mvvm;
using Careful.Core.Mvvm.ViewModel;
using System;

namespace Careful.Core.Mvvm
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
        public static ICarefulIoc RegisterTypeForNavigation(this ICarefulIoc container, Type type, string name)
        {
            return container.RegisterType(typeof(object), type, name);
        }
        public static ICarefulIoc RegisterType(this ICarefulIoc container, Type from, Type to, string name)
        {
            return null;
        }
        /// <summary>
        /// Gets the <see cref="IUnityContainer" /> from the <see cref="IContainerProvider" />
        /// </summary>
        /// <param name="containerProvider">The current <see cref="IContainerProvider" /></param>
        /// <returns>The underlying <see cref="IUnityContainer" /></returns>
        public static ICarefulIoc GetContainer(this IContainerProvider containerProvider)
        {
            return ((IContainerExtension<ICarefulIoc>)containerProvider).Instance;
        }

        /// <summary>
        /// Gets the <see cref="IUnityContainer" /> from the <see cref="IContainerProvider" />
        /// </summary>
        /// <param name="containerRegistry">The current <see cref="IContainerRegistry" /></param>
        /// <returns>The underlying <see cref="IUnityContainer" /></returns>
        public static ICarefulIoc GetContainer(this IContainerRegistry containerRegistry)
        {
            return ((IContainerExtension<ICarefulIoc>)containerRegistry).Instance;
        }
        /// <summary>
        /// Registers an object for navigation.
        /// </summary>
        /// <typeparam name="T">The Type of the object to register as the view</typeparam>
        /// <param name="container"></param>
        /// <param name="name">The unique name to register with the object.</param>
        public static ICarefulIoc RegisterTypeForNavigation<T>(this ICarefulIoc container, string name = null)
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
        public static ICarefulIoc RegisterTypeForNavigation<TView, TViewModel>(this ICarefulIoc container, string name = null)
        {
            return container.RegisterTypeForNavigationWithViewModel<TViewModel>(typeof(TView), name);
        }

        private static ICarefulIoc RegisterTypeForNavigationWithViewModel<TViewModel>(this ICarefulIoc container, Type viewType, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                name = viewType.Name;

            ViewModelLocationProvider.Register(viewType.ToString(), typeof(TViewModel));

            return container.RegisterTypeForNavigation(viewType, name);
        }
    }
}
