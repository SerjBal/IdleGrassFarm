using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class Scene : MonoBehaviour, IScene
    {
        // private ICustomer _customer;
        // private IUpgrader[] _upgraders;
        private IZone[] _zones;

        public void Init()
        {
            InitZones();
            
            Debug.Log("Scene Initialized");
        }

        private void InitZones()
        {
            var player = DI.GetService<IPlayer>();
            var inventory = DI.GetService<IInventory>();
            _zones = transform.GetComponentsInChildren<IZone>();
            
            foreach (var zone in _zones)
            {
                player.OnMow += zone.Mow;
                zone.OnMewed += inventory.PutItem;
                zone.OnMewed += (x) => inventory.Refresh();
            }
        }
    }
}
