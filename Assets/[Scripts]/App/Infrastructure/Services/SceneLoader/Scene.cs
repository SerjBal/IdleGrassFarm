using UnityEngine;
using Serjbal.Core;

namespace Serjbal
{
    public interface IScene : IService, IInitializable
    {
    }

    public class Scene : MonoBehaviour, IScene
    {
        private ICustomer customer;
        private IUpgrader[] upgraders;
        private IZones[] zones;

        public void Init()
        {
            customer = GetComponentInChildren<ICustomer>();
            upgraders = GetComponentsInChildren<IUpgrader>();
            zones = GetComponentsInChildren<IZones>();

            var player = DI.GetService<IPlayer>();
            player.OnSell += customer.Buy;
            foreach (var upgrader in upgraders)
                player.OnUpgrade += upgrader.Upgrade;
            foreach (var zone in zones)
                player.OnMow += zone.Mow;
        }
    }

    public interface IZones
    {
        void Mow();
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
