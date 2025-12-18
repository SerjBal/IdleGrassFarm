using System;
using UnityEngine;

namespace Serjbal
{
    [Serializable]
    public class AppSettingsModel
    {
        [Header("Player Settings")]
        public int playerSpeed = 5;
        public float playerRotateionSpeed = 15;
        public ScytheModel scytheModel;
        
        [Header("Inventory Settings")]
        public InventoryModel inventoryModel;

        [Header("Economy Settings")]
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
    public class EconomyModel
    {
        public ItemPrice[] scytheLevelUpPrice;
        public ItemPrice[] inventoryLevelUpPrice;
    }
    
}