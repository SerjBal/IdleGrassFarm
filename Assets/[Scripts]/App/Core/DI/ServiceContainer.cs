using System;
using System.Collections.Generic;
using System.Linq;
using Serjbal.Utils;

namespace Serjbal.Core
{
    public sealed class ServiceContainer : IServiceContainer
    {
        private readonly ObservableDictionary<Type, object> _services =
            new ObservableDictionary<Type, object>();

        private readonly Dictionary<Type, ServiceContainerCallback> _callbackServices =
            new Dictionary<Type, ServiceContainerCallback>();

        public event EventHandler<NotifyDictionaryChangedEventArgs<Type, object>> DictionaryChanged
        {
            add => _services.DictionaryChanged += value;
            remove => _services.DictionaryChanged -= value;
        }

        public void AddService(Type serviceType, ServiceContainerCallback callback)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            if (!typeof(IService).IsAssignableFrom(serviceType))
                throw new ArgumentException($"{serviceType} does not implement IService");

            _callbackServices[serviceType] = callback;
            _services[serviceType] = null;          // null -  маркер наличия сервиса
        }

        public void AddService(Type serviceType, object serviceInstance)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (!serviceType.IsInstanceOfType(serviceInstance))
                throw new ArgumentException($"Service instance of type {serviceInstance.GetType()} is not assignable to {serviceType}");
            if (!typeof(IService).IsAssignableFrom(serviceInstance.GetType()))
                throw new ArgumentException($"{serviceInstance.GetType()} does not implement IService");

            if (_callbackServices.ContainsKey(serviceType))
                _callbackServices.Remove(serviceType);

            _services[serviceType] = serviceInstance;
        }

        public void AddService(object serviceInstance)
        {
            if (serviceInstance == null)
                throw new ArgumentNullException(nameof(serviceInstance));

            var serviceType = serviceInstance.GetType();
            AddService(serviceType, serviceInstance);
        }

        public void AddService<T>(ServiceContainerCallback callback) where T : class =>
            AddService(typeof(T), callback);

        public void AddService<T>(T serviceInstance) where T : class =>
            AddService(typeof(T), serviceInstance);

        public ICollection<Type> GetAllServices()
        {
            return _services.Keys.ToList(); // Возвращаем копию для безопасности
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));

            if (_callbackServices.TryGetValue(serviceType, out var callback))
            {
                var instance = callback;
                if (instance != null && !serviceType.IsInstanceOfType(instance))
                    throw new InvalidOperationException($"Callback for service {serviceType} returned instance of wrong type {instance.GetType()}");

                return instance;
            }

            if (_services.TryGetValue(serviceType, out var service))
            {
                return service;
            }

            return null;
        }

        public T GetService<T>() where T : class =>
            (T)GetService(typeof(T));

        public void RemoveService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            
            if (_callbackServices.ContainsKey(serviceType))
            {
                _callbackServices.Remove(serviceType);
            }
            _services.Remove(serviceType);
        }

        public void Clear()
        {
            _callbackServices.Clear();
            _services.Clear();
        }

        public void RemoveService<T>() where T : class =>
            RemoveService(typeof(T));
    }
}