using UnityEngine;

namespace Serjbal
{
    public class Scene : MonoBehaviour, IScene
    {
        private ICustomer _customer;
        private IUpgrader[] _upgraders;
        private IZone[] _zones;

        public void Init()
        {
            var player = DI.GetService<IPlayer>();
            InitZones(player);
            
            Debug.Log("Scene Initialized");
        }

        private void InitZones(IPlayer player)
        {
            _zones = GetComponentsInChildren<IZone>();
            foreach (var zone in _zones)
            {
                player.OnMow += zone.Mow;
                zone.OnMewed += player.PutToInventory;
            }
        }
    }
}
