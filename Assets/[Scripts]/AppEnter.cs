using System.Linq;
using Serjbal.Core;

namespace Serjbal
{
    using System;
    using UnityEngine;

    public class AppEnter : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _services;
        //IEventBus<AppEvent> _eventBus;

        private void Awake()
        {
            RegisterServices();
            InitializeServices();
        }

        private void InitializeServices()
        {
            foreach (Type serviceType in DI.GetAllServices())
            {
                (DI.GetService(serviceType) as IInitializable)?.Init();
            }
        }

        private void RegisterServices()
        {
            foreach (var service in _services)
            {
                if (service == null) continue;
                DI.AddService(service);
            }
        }

        private void OnDestroy()
        {
            Despose();
        }

        private void Despose()
        {
            foreach (Type serviceType in DI.GetAllServices())
            {
                (DI.GetService(serviceType) as IDisposable)?.Dispose();
            }
        }
    }
}
