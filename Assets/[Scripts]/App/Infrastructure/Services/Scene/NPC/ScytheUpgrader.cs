using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class ScytheUpgrader : MonoBehaviour, IUpgrader
    {
        public void Interact()
        {
            var player = DI.GetService<IPlayer>();
            var levelUp = player.GetScytheModel().level + 1;
            var price = DI.GetService<AppSettingsModel>().economyModel.scytheLevelUpPrice;
            var inventory = DI.GetService<IInventory>();
            
            if (inventory.CheckPrice(price))
            {
                foreach (var p in price)
                    inventory.TakeItem(p);

                player.SetScytheLevel(levelUp);
            }
        }
    }
}
