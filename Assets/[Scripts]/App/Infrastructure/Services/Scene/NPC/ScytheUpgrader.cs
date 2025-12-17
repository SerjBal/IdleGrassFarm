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
            var levelUpPrice = DI.GetService<AppSettingsModel>().economyModel.scytheLevelUpPrice;
            var inventory = DI.GetService<IInventory>();
            
            if (inventory.CheckPrice(levelUpPrice))
            {
                foreach (var prace in levelUpPrice)
                    inventory.TakeItem(prace.item, prace.value);

                player.SetScytheLevel(levelUp);
            }
        }
    }
}
