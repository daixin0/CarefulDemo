using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace Careful.Core.Ioc
{
    public class CarefulIoc : ICarefulIoc
    {
        private readonly Dictionary<Type, ConstructorInfo> _constructorInfos
           = new Dictionary<Type, ConstructorInfo>();

        private readonly string _defaultKey = Guid.NewGuid().ToString();

        private readonly object[] _emptyArguments = new object[0];

        private readonly Dictionary<Type, Dictionary<string, Delegate>> _factories
            = new Dictionary<Type, Dictionary<string, Delegate>>();

        private readonly Dictionary<Type, Dictionary<string, object>> _instancesRegistry
            = new Dictionary<Type, Dictionary<string, object>>();

        private readonly Dictionary<Type, Type> _interfaceToClassMap
            = new Dictionary<Type, Type>();

        private readonly object _syncLock = new object();

        private static CarefulIoc _default;

        public static CarefulIoc Default
        {
            get
            {
                return _default ?? (_default = new CarefulIoc());
            }
        }

        public bool ContainsCreated<TClass>()
        {
            return ContainsCreated<TClass>(null);
        }

        public bool ContainsCreated<TClass>(string key)
        {
            var classType = typeof(TClass);

            if (!_instancesRegistry.ContainsKey(classType))
            {
                return false;
            }

            if (string.IsNullOrEmpty(key))
            {
                return _instancesRegistry[classType].Count > 0;
            }

            return _instancesRegistry[classType].ContainsKey(key);
        }

        public bool IsRegistered<T>()
        {
            var classType = typeof(T);
            return _interfaceToClassMap.ContainsKey(classType);
        }

        public bool IsRegistered<T>(string key)
        {
            var classType = typeof(T);

            if (!_interfaceToClassMap.ContainsKey(classType)
                || !_factories.ContainsKey(classType))
            {
                return false;
            }

            return _factories[classType].ContainsKey(key);
        }

        public void Register<TInterface, TClass>()
            where TClass : class
            where TInterface : class
        {
            Register<TInterface, TClass>(false);
        }

        public void Register<TInterface, TClass>(bool createInstanceImmediately)
            where TClass : class
            where TInterface : class
        {
            lock (_syncLock)
            {
                var interfaceType = typeof(TInterface);
                var classType = typeof(TClass);

                if (_interfaceToClassMap.ContainsKey(interfaceType))
                {
                    if (_interfaceToClassMap[interfaceType] != classType)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "There is already a class registered for {0}.",
                                interfaceType.FullName));
                    }
                }
                else
                {
                    _interfaceToClassMap.Add(interfaceType, classType);
                    _constructorInfos.Add(classType, GetConstructorInfo(classType));
                }

                Func<TInterface> factory = MakeInstance<TInterface>;
                DoRegister(interfaceType, factory, _defaultKey);

                if (createInstanceImmediately)
                {
                    GetInstance<TInterface>();
                }
            }
        }
        public void RegisterInstance<TInterface>(object obj)
        {
            lock (_syncLock)
            {
                var interfaceType = typeof(TInterface);
                var classType = obj.GetType();

                if (_interfaceToClassMap.ContainsKey(interfaceType))
                {
                    if (_interfaceToClassMap[interfaceType] != classType)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "There is already a class registered for {0}.",
                                interfaceType.FullName));
                    }
                }
                else
                {
                    _interfaceToClassMap.Add(interfaceType, classType);
                    _constructorInfos.Add(classType, GetConstructorInfo(classType));
                }

                Func<TInterface> factory = MakeInstance<TInterface>;
                DoRegister(interfaceType, factory, _defaultKey);

            }
        }
        public bool IsRegistered(Type type)
        {
            return _interfaceToClassMap.ContainsKey(type);
        }
        public void Register<TClass>()
            where TClass : class
        {
            Register<TClass>(false);
        }

        public void Register<TClass>(bool createInstanceImmediately)
            where TClass : class
        {
            var classType = typeof(TClass);
            if (classType.IsInterface)
            {
                throw new ArgumentException("An interface cannot be registered alone.");
            }

            lock (_syncLock)
            {
                if (_factories.ContainsKey(classType)
                    && _factories[classType].ContainsKey(_defaultKey))
                {
                    if (!_constructorInfos.ContainsKey(classType))
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "Class {0} is already registered.",
                                classType));
                    }

                    return;
                }

                if (!_interfaceToClassMap.ContainsKey(classType))
                {
                    _interfaceToClassMap.Add(classType, null);
                }

                _constructorInfos.Add(classType, GetConstructorInfo(classType));
                Func<TClass> factory = MakeInstance<TClass>;
                DoRegister(classType, factory, _defaultKey);

                if (createInstanceImmediately)
                {
                    GetInstance<TClass>();
                }
            }
        }

        public void Register<TClass>(Func<TClass> factory)
            where TClass : class
        {
            Register(factory, false);
        }

        public void Register<TClass>(Func<TClass> factory, bool createInstanceImmediately)
            where TClass : class
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            lock (_syncLock)
            {
                var classType = typeof(TClass);

                if (_factories.ContainsKey(classType)
                    && _factories[classType].ContainsKey(_defaultKey))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "There is already a factory registered for {0}.",
                            classType.FullName));
                }

                if (!_interfaceToClassMap.ContainsKey(classType))
                {
                    _interfaceToClassMap.Add(classType, null);
                }

                DoRegister(classType, factory, _defaultKey);

                if (createInstanceImmediately)
                {
                    GetInstance<TClass>();
                }
            }
        }

        public void Register<TClass>(Func<TClass> factory, string key)
            where TClass : class
        {
            Register(factory, key, false);
        }

        public void Register<TClass>(
            Func<TClass> factory,
            string key,
            bool createInstanceImmediately)
            where TClass : class
        {
            lock (_syncLock)
            {
                var classType = typeof(TClass);

                if (_factories.ContainsKey(classType)
                    && _factories[classType].ContainsKey(key))
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "There is already a factory registered for {0} with key {1}.",
                            classType.FullName,
                            key));
                }

                if (!_interfaceToClassMap.ContainsKey(classType))
                {
                    _interfaceToClassMap.Add(classType, null);
                }

                DoRegister(classType, factory, key);

                if (createInstanceImmediately)
                {
                    GetInstance<TClass>(key);
                }
            }
        }

        public void Reset()
        {
            _interfaceToClassMap.Clear();
            _instancesRegistry.Clear();
            _constructorInfos.Clear();
            _factories.Clear();
        }

        public void Unregister<TClass>()
            where TClass : class
        {
            lock (_syncLock)
            {
                var serviceType = typeof(TClass);
                Type resolveTo;

                if (_interfaceToClassMap.ContainsKey(serviceType))
                {
                    resolveTo = _interfaceToClassMap[serviceType] ?? serviceType;
                }
                else
                {
                    resolveTo = serviceType;
                }

                if (_instancesRegistry.ContainsKey(serviceType))
                {
                    _instancesRegistry.Remove(serviceType);
                }

                if (_interfaceToClassMap.ContainsKey(serviceType))
                {
                    _interfaceToClassMap.Remove(serviceType);
                }

                if (_factories.ContainsKey(serviceType))
                {
                    _factories.Remove(serviceType);
                }

                if (_constructorInfos.ContainsKey(resolveTo))
                {
                    _constructorInfos.Remove(resolveTo);
                }
            }
        }

        public void Unregister<TClass>(TClass instance)
            where TClass : class
        {
            lock (_syncLock)
            {
                var classType = typeof(TClass);

                if (_instancesRegistry.ContainsKey(classType))
                {
                    var list = _instancesRegistry[classType];

                    var pairs = list.Where(pair => pair.Value == instance).ToList();
                    for (var index = 0; index < pairs.Count(); index++)
                    {
                        var key = pairs[index].Key;

                        list.Remove(key);
                    }
                }
            }
        }

        public void Unregister<TClass>(string key)
            where TClass : class
        {
            lock (_syncLock)
            {
                var classType = typeof(TClass);

                if (_instancesRegistry.ContainsKey(classType))
                {
                    var list = _instancesRegistry[classType];

                    var pairs = list.Where(pair => pair.Key == key).ToList();
                    for (var index = 0; index < pairs.Count(); index++)
                    {
                        list.Remove(pairs[index].Key);
                    }
                }

                if (_factories.ContainsKey(classType))
                {
                    if (_factories[classType].ContainsKey(key))
                    {
                        _factories[classType].Remove(key);
                    }
                }
            }
        }

        private object DoGetService(Type serviceType, string key, bool cache = true)
        {
            lock (_syncLock)
            {
                if (string.IsNullOrEmpty(key))
                {
                    key = _defaultKey;
                }

                Dictionary<string, object> instances = null;

                if (!_instancesRegistry.ContainsKey(serviceType))
                {
                    if (!_interfaceToClassMap.ContainsKey(serviceType))
                    {
                        throw new ActivationException(
                            string.Format(
                                CultureInfo.InvariantCulture,
                                "Type not found in cache: {0}.",
                                serviceType.FullName));
                    }

                    if (cache)
                    {
                        instances = new Dictionary<string, object>();
                        _instancesRegistry.Add(serviceType, instances);
                    }
                }
                else
                {
                    instances = _instancesRegistry[serviceType];
                }

                if (instances != null
                    && instances.ContainsKey(key))
                {
                    return instances[key];
                }

                object instance = null;

                if (_factories.ContainsKey(serviceType))
                {
                    if (_factories[serviceType].ContainsKey(key))
                    {
                        instance = _factories[serviceType][key].DynamicInvoke(null);
                    }
                    else
                    {
                        if (_factories[serviceType].ContainsKey(_defaultKey))
                        {
                            instance = _factories[serviceType][_defaultKey].DynamicInvoke(null);
                        }
                        else
                        {
                            throw new ActivationException(
                                string.Format(
                                    CultureInfo.InvariantCulture,
                                    "Type not found in cache without a key: {0}",
                                    serviceType.FullName));
                        }
                    }
                }

                if (cache
                    && instances != null)
                {
                    instances.Add(key, instance);
                }

                return instance;
            }
        }

        private void DoRegister<TClass>(Type classType, Func<TClass> factory, string key)
        {
            if (_factories.ContainsKey(classType))
            {
                if (_factories[classType].ContainsKey(key))
                {
                    // The class is already registered, ignore and continue.
                    return;
                }

                _factories[classType].Add(key, factory);
            }
            else
            {
                var list = new Dictionary<string, Delegate>
                {
                    {
                        key,
                        factory
                    }
                };

                _factories.Add(classType, list);
            }
        }

        private ConstructorInfo GetConstructorInfo(Type serviceType)
        {
            Type resolveTo;

            if (_interfaceToClassMap.ContainsKey(serviceType))
            {
                resolveTo = _interfaceToClassMap[serviceType] ?? serviceType;
            }
            else
            {
                resolveTo = serviceType;
            }

            var constructorInfos = resolveTo.GetConstructors();
            if (constructorInfos.Length > 1)
            {
                if (constructorInfos.Length > 2)
                {
                    return GetPreferredConstructorInfo(constructorInfos, resolveTo);
                }

                if (constructorInfos.FirstOrDefault(i => i.Name == ".cctor") == null)
                {
                    return GetPreferredConstructorInfo(constructorInfos, resolveTo);
                }

                var first = constructorInfos.FirstOrDefault(i => i.Name != ".cctor");

                if (first == null
                    || !first.IsPublic)
                {
                    throw new ActivationException(
                        string.Format(
                            CultureInfo.InvariantCulture,
                            "Cannot register: No public constructor found in {0}.",
                            resolveTo.Name));
                }

                return first;
            }

            if (constructorInfos.Length == 0
                || (constructorInfos.Length == 1
                    && !constructorInfos[0].IsPublic))
            {
                throw new ActivationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Cannot register: No public constructor found in {0}.",
                        resolveTo.Name));
            }

            return constructorInfos[0];
        }

        private static ConstructorInfo GetPreferredConstructorInfo(IEnumerable<ConstructorInfo> constructorInfos, Type resolveTo)
        {
            var preferredConstructorInfo
                = (from t in constructorInfos
                   let attribute = Attribute.GetCustomAttribute(t, typeof(PreferredConstructorAttribute))
                   where attribute != null
                   select t).FirstOrDefault();

            if (preferredConstructorInfo == null)
            {
                throw new ActivationException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Cannot register: Multiple constructors found in {0} but none marked with PreferredConstructor.",
                        resolveTo.Name));
            }

            return preferredConstructorInfo;
        }

        private TClass MakeInstance<TClass>()
        {
            var serviceType = typeof(TClass);

            var constructor = _constructorInfos.ContainsKey(serviceType)
                                  ? _constructorInfos[serviceType]
                                  : GetConstructorInfo(serviceType);

            var parameterInfos = constructor.GetParameters();

            if (parameterInfos.Length == 0)
            {
                return (TClass)constructor.Invoke(_emptyArguments);
            }

            var parameters = new object[parameterInfos.Length];

            foreach (var parameterInfo in parameterInfos)
            {
                parameters[parameterInfo.Position] = GetService(parameterInfo.ParameterType);
            }

            return (TClass)constructor.Invoke(parameters);
        }

        public IEnumerable<object> GetAllCreatedInstances(Type serviceType)
        {
            if (_instancesRegistry.ContainsKey(serviceType))
            {
                return _instancesRegistry[serviceType].Values;
            }

            return new List<object>();
        }

        public IEnumerable<TService> GetAllCreatedInstances<TService>()
        {
            var serviceType = typeof(TService);
            return GetAllCreatedInstances(serviceType)
                .Select(instance => (TService)instance);
        }

        #region Implementation of IServiceProvider

        public object GetService(Type serviceType)
        {
            return DoGetService(serviceType, _defaultKey);
        }

        #endregion

        #region Implementation of IServiceLocator


        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            lock (_factories)
            {
                if (_factories.ContainsKey(serviceType))
                {
                    foreach (var factory in _factories[serviceType])
                    {
                        GetInstance(serviceType, factory.Key);
                    }
                }
            }

            if (_instancesRegistry.ContainsKey(serviceType))
            {
                return _instancesRegistry[serviceType].Values;
            }


            return new List<object>();
        }


        public IEnumerable<TService> GetAllInstances<TService>()
        {
            var serviceType = typeof(TService);
            return GetAllInstances(serviceType)
                .Select(instance => (TService)instance);
        }

        public object GetInstance(Type serviceType)
        {
            return DoGetService(serviceType, _defaultKey);
        }

        public object GetInstanceWithoutCaching(Type serviceType)
        {
            return DoGetService(serviceType, _defaultKey, false);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return DoGetService(serviceType, key);
        }

        public object GetInstanceWithoutCaching(Type serviceType, string key)
        {
            return DoGetService(serviceType, key, false);
        }

        public TService GetInstance<TService>()
        {
            return (TService)DoGetService(typeof(TService), _defaultKey);
        }

        public TService GetInstanceWithoutCaching<TService>()
        {
            return (TService)DoGetService(typeof(TService), _defaultKey, false);
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService)DoGetService(typeof(TService), key);
        }

        public TService GetInstanceWithoutCaching<TService>(string key)
        {
            return (TService)DoGetService(typeof(TService), key, false);
        }

        #endregion
    }
}