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
            var levelUpPrice = DI.GetService<AppSettingsModel>().economyModel.inventoryLevelUpPrice;
            
            if (inventory.CheckPrice(levelUpPrice))
            {
                foreach (var price in levelUpPrice)
                    inventory.TakeItem(price.item, price.value);

                inventory.SetLevel(levelUp);
            }
        }
    }
}
