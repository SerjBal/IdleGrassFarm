using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DI
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void AddService(Type serviceType, object serviceInstance)
    {
        if (serviceType == null)
            throw new ArgumentNullException(nameof(serviceType));
        if (!serviceType.IsInstanceOfType(serviceInstance))
            throw new ArgumentException($"Service instance of type {serviceInstance.GetType()} is not assignable to {serviceType}");
        if (serviceInstance is not IService)
            throw new ArgumentException($"{serviceInstance.GetType()} does not implement IService");

        _services[serviceType] = serviceInstance;
    }
    
    public static void AddService(MonoBehaviour serviceInstance)
    {
        if (serviceInstance is IService)
        {
            var type = serviceInstance.GetType();
            var iType = type.Interface();
            AddService(iType, serviceInstance);
        }
    }

    public static void AddService(object serviceInstance)
    {
        if (serviceInstance == null)
            throw new ArgumentNullException(nameof(serviceInstance));

        var serviceType = serviceInstance.GetType();
        AddService(serviceType, serviceInstance);
    }

    public static ICollection<Type> GetAllServices()
    {
        return _services.Keys.ToList();
    }

    public static object GetService(Type serviceType)
    {
        if (serviceType == null)
            throw new ArgumentNullException(nameof(serviceType));

        if (_services.TryGetValue(serviceType, out var service))
        {
            return service;
        }

        return null;
    }

    public static T GetService<T>() where T : class =>
        (T)GetService(typeof(T));

    public static Type Interface(this Type type)
    {
        var interfaces = type.GetInterfaces();
        var iParentType = typeof(IService);
        foreach (var i in interfaces)
        {
            if (i != iParentType && iParentType.IsAssignableFrom(i))
            {
                return i;
            }
        }

        return iParentType;
    }
    
    // public static void RegisterSingle<TService>(TService implementation) where TService : IService
    // {
    //     Implementation<TService>.ServiceInstance = implementation;
    // }
    //
    // public static TService Single<TService>() where TService : IService
    // {
    //     return Implementation<TService>.ServiceInstance;
    // }
    //
    // private class Implementation<TService> where TService : IService
    // {
    //     public static TService ServiceInstance;
    // }
}

public interface IService
{
}
