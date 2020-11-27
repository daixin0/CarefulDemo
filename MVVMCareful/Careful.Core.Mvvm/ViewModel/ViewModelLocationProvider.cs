using Careful.Core.Mvvm.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Careful.Core.Mvvm.ViewModel
{
    public static class ViewModelLocationProvider
    {
        static Dictionary<string, Func<object>> _factories = new Dictionary<string, Func<object>>();

        static Dictionary<string, Type> _typeFactories = new Dictionary<string, Type>();

        static Func<Type, object> _defaultViewModelFactory = type => Activator.CreateInstance(type);

        static Func<object, Type, bool, object> _defaultViewModelFactoryWithViewParameter;

        static Func<Type, Type> _defaultViewTypeToViewModelTypeResolver =
            viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName.Replace("Views", "ViewModels");
                var suffix = viewName.EndsWith("View") ? "Model" : "ViewModel";
                var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", viewName, suffix, viewAssemblyName);
                return Type.GetType(viewModelName);
            };

        public static void SetDefaultViewModelFactory(Func<Type, object> viewModelFactory)
        {
            _defaultViewModelFactory = viewModelFactory;
        }

        public static void SetDefaultViewModelFactory(Func<object, Type, bool, object> viewModelFactory)
        {
            _defaultViewModelFactoryWithViewParameter = viewModelFactory;
        }

        public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
        {
            _defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
        }


        public static void AutoWireViewModelChanged(object view, Action<object, object> setDataContextCallback)
        {
            object viewModel = GetViewModelForView(view);

            if (viewModel == null)
            {
                var viewModelType = GetViewModelTypeForView(view.GetType());

                if (viewModelType == null)
                    viewModelType = _defaultViewTypeToViewModelTypeResolver(view.GetType());

                if (viewModelType == null)
                    return;

                bool isSingle = false;
                if (view is IView window)
                {
                    isSingle = window.IsSingle;
                }

                viewModel = _defaultViewModelFactoryWithViewParameter != null ? _defaultViewModelFactoryWithViewParameter(view, viewModelType, isSingle) : _defaultViewModelFactory(viewModelType);
            }


            setDataContextCallback(view, viewModel);
        }

        private static object GetViewModelForView(object view)
        {
            var viewKey = view.GetType().ToString();

            if (_factories.ContainsKey(viewKey))
                return _factories[viewKey]();

            return null;
        }

        private static Type GetViewModelTypeForView(Type view)
        {
            var viewKey = view.ToString();

            if (_typeFactories.ContainsKey(viewKey))
                return _typeFactories[viewKey];

            return null;
        }

        public static void Register<T>(Func<object> factory)
        {
            Register(typeof(T).ToString(), factory);
        }

        public static void Register(string viewTypeName, Func<object> factory)
        {
            _factories[viewTypeName] = factory;
        }

        public static void Register<T, VM>()
        {
            var viewType = typeof(T);
            var viewModelType = typeof(VM);

            Register(viewType.ToString(), viewModelType);
        }

        public static void Register(string viewTypeName, Type viewModelType)
        {
            _typeFactories[viewTypeName] = viewModelType;
        }
    }
}
