using System;
using UnityEngine;

namespace Serjbal
{
    [Serializable]
    public class AppSettingsModel
    {
        [Header("\n\n[PLAYER SETTINGS]\n\n")]
        public int playerSpeed = 5;
        public float playerRotateionSpeed = 15;
        public ScytheModel scytheModel;
        
        [Header("\n\n[INVENTORY SETTINGS]\n\n")]
        public InventoryModel inventoryModel;

        [Header("\n\n[ECONOMY SETTINGS]\n\n")]
        public EconomyModel economyModel;
    }

    [Serializable]
    public struct ItemPrice
    {
        public ItemType item;
        public int value;

        public ItemPrice(ItemType item, int value)
        {
            this.item = item;
            this.value = value;
        }
    }

    [Serializable]
    public struct EconomyModel
    {
        public ItemPrice[] scytheLevelUpPrice;
        public ItemPrice[] inventoryLevelUpPrice;
    }
    
}