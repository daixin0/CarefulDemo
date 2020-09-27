using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.ServiceLocation;

namespace Careful.Core.Ioc
{
    public interface ICarefulIoc : IServiceLocator
    {
        bool ContainsCreated<TClass>();

        bool ContainsCreated<TClass>(string key);
        bool IsRegistered(Type type);
        bool IsRegistered<T>();

        bool IsRegistered<T>(string key);

        void Register<TInterface, TClass>()
            where TClass : class 
            where TInterface : class;

        void Register<TInterface, TClass>(bool createInstanceImmediately)
            where TClass : class
            where TInterface : class;
        void RegisterInstance<TInterface>(object obj);
        void Register<TClass>() 
            where TClass : class;

        void Register<TClass>(bool createInstanceImmediately)
            where TClass : class;

        void Register<TClass>(Func<TClass> factory)
            where TClass : class;

        void Register<TClass>(Func<TClass> factory, bool createInstanceImmediately)
            where TClass : class;

        void Register<TClass>(Func<TClass> factory, string key)
            where TClass : class;

        void Register<TClass>(
            Func<TClass> factory,
            string key,
            bool createInstanceImmediately)
            where TClass : class;

        void Reset();

        void Unregister<TClass>() 
            where TClass : class;

        void Unregister<TClass>(TClass instance) 
            where TClass : class;

        void Unregister<TClass>(string key) 
            where TClass : class;

    }
}