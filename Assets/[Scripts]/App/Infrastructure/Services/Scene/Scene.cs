using System;
using UnityEngine;
using Serjbal.Core;
using Serjbal.Infrastructure.Services;

namespace Serjbal
{
    public interface IScene : IService, IInitializable
    {
    }

    public class Scene : MonoBehaviour, IScene
    {
        private ICustomer customer;
        private IUpgrader[] upgraders;
        private IZone[] zones;

        public void Init()
        {
            customer = GetComponentInChildren<ICustomer>();
            upgraders = GetComponentsInChildren<IUpgrader>();
            zones = GetComponentsInChildren<IZone>();

            var player = DI.GetService<IPlayer>();
            player.OnSell += customer.Buy;
            foreach (var upgrader in upgraders)
                player.OnUpgrade += upgrader.Upgrade;
            foreach (var zone in zones)
                player.OnMow += zone.Mow;
            
            Debug.Log("Scene Initialized");
        }
    }

    public interface IZone
    {
        Action<ItemType, int> OnMewed { get; set; }
        void Mow(Vector3 position);
    }

    public interface IUpgrader
    {
        void Upgrade();
    }

    public interface ICustomer
    {
        void Buy();
    }
}
