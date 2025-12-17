using Serjbal.Core;

namespace Serjbal
{
    using System;
    using UnityEngine;

    public class AppEnter : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _services;
        [SerializeField] private AppSettings _settings;

        private void Awake()
        {
            RegisterSettingsModel();
            RegisterServices();
            RegisterEventBus();
            InitializeServices();
        }

        private void InitializeServices()
        {
            foreach (Type serviceType in DI.GetAllServices())
            {
                (DI.GetService(serviceType) as IInitializable)?.Init();
            }
        }

        private void RegisterEventBus()
        {
            DI.AddService<IEventBus<InventoryEvent>>(new AppEventBus<InventoryEvent>());
        }

        private void RegisterServices()
        {
            foreach (var service in _services)
            {
                if (service == null) continue;
                DI.AddService(service);
            }
        }

        private void RegisterSettingsModel()
        {
            DI.AddService<AppSettingsModel>(_settings.model);
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
