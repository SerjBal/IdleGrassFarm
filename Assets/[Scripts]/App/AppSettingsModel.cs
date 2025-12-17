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
    }

    [Serializable]
    public struct ItemPrice
    {
        public string item;
        [FormerlySerializedAs("price")] public int value;
    }
}