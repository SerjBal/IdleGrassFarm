using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class InventoryUpgrader : MonoBehaviour, IUpgrader
    {
        public void Interact()
        {
            var inventory = DI.GetService<IInventory>();
            var data = inventory.InventoryInfo();

            if (inventory.CheckPrice(data.levelPrice))
            {
                foreach (var prace in data.levelPrice)
                {
                    inventory.TakeItem(prace.item, prace.value);
                }

                inventory.SetLevel(data.level++);
            }
        }
    }
}
