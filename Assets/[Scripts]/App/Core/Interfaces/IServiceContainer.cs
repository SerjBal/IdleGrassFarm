using System;
using System.Collections.Generic;

namespace Serjbal.Core
{
	public delegate object ServiceContainerCallback(IServiceContainer container, Type serviceType);

	public interface IServiceContainer : INotifyDictionaryChanged<Type, object>
    {
		void AddService(Type serviceType, ServiceContainerCallback callback);

		void AddService(Type serviceType, object serviceInstance);

		void AddService(object serviceInstance);

		void AddService<T>(ServiceContainerCallback callback) where T : class;

		void AddService<T>(T serviceInstance) where T : class;

		void RemoveService(Type serviceType);

		object GetService(Type serviceType);

        T GetService<T>() where T : class;

		ICollection<Type> GetAllServices();
	}
}