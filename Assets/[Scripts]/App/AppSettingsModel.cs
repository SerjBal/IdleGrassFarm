using System;
using UnityEngine;
using UnityEngine.Serialization;

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
        public string item;
        public int value;
    }

    [Serializable]
    public class EconomyModel
    {
        public ItemPrice[] scytheLevelUpPrice;
        public ItemPrice[] inventoryLevelUpPrice;
    }
    
}