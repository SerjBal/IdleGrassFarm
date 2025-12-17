using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class ScytheUpgrader : MonoBehaviour, IUpgrader
    {

        public void Interact()
        {
            var player = DI.GetService<IPlayer>();
            var inventory = DI.GetService<IInventory>();
            var data = player.GetScytheModel();
            
            if (inventory.CheckPrice(data.levelPrice))
            {
                foreach (var prace in data.levelPrice)
                {
                    inventory.TakeItem(prace.item, prace.value);
                }

                player.SetScytheLevel(data.level++);
            }
        }
    }
}
