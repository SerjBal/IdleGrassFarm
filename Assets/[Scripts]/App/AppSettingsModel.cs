using System;
using System.Linq;
using UnityEngine;

namespace Serjbal
{
    [Serializable]
    public class AppSettingsModel
    {
        [Header("\n\n[PLAYER SETTINGS]\n\n")]
        [SerializeField] private PlayerModel _playerModel;
        
        [Header("\n\n[INVENTORY SETTINGS]\n\n")]
        [SerializeField] private InventoryModel _inventoryModel;

        [Header("\n\n[ECONOMY SETTINGS]\n\n")]
        [SerializeField] private EconomyModel _economyModel;
        
        public PlayerModel PlayerModel => _playerModel;
        public InventoryModel InventoryModel => _inventoryModel;
        public EconomyModel EconomyModel => _economyModel;

        public void InitializeDictionarys()
        {
            _inventoryModel.Initialize();
        }
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

    [Serializable]
    public struct PlayerModel
    {
        public int playerSpeed;
        public float playerRotateionSpeed;
        public ScytheModel scytheModel;
    }
}