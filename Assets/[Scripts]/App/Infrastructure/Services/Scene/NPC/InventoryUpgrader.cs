using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class InventoryUpgrader : MonoBehaviour, IUpgrader
    {
        public void Interact()
        {
            var inventory = DI.GetService<IInventory>();
            var levelUp = inventory.InventoryInfo().level + 1;
            var price = DI.GetService<AppSettingsModel>().EconomyModel.inventoryLevelUpPrice;
            
            if (inventory.CheckPrice(price))
            {
                foreach (var p in price)
                    inventory.TakeItem(p);

                inventory.SetLevel(levelUp);
                inventory.Refresh();
            }
        }
    }
}
