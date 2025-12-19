using System;
using System.Collections.Generic;
using System.Linq;
using Serjbal.Core;
using UnityEngine;

namespace Serjbal
{
    public class HUDView : MonoBehaviour
    {
        [Serializable]
        public class ItemPrefab
        {
            public ItemType item;
            public GameObject prefab;
        }
        
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private ItemPrefab[] _itemPrefabs;
        [SerializeField] private GameObject _scaleDownAnimPref;
        [SerializeField] private GameObject _scaleUpAnimPref;
        [SerializeField] private TMPro.TextMeshProUGUI _inventaryLevel;
        [SerializeField] private TMPro.TextMeshProUGUI _scytheLevel;
        
        private Dictionary<ItemType, GameObject> _itemsPrefabsDict = new Dictionary<ItemType, GameObject>();
        private InventoryModel _inventoryModel;
        private InventoryModel _oldInventoryModel;
        private ScytheModel _scytheModel;
        private ScytheModel _oldScytheModel;


        private void Awake()
        {
            foreach (var prefab in _itemPrefabs)
            {
                _itemsPrefabsDict = _itemPrefabs.ToDictionary(
                    x => x.item,   
                    x => x.prefab       
                );
            }
        }

        private void Start()
        {
            _inventoryModel = DI.GetService<AppSettingsModel>().InventoryModel;
            _oldInventoryModel = (InventoryModel) _inventoryModel.Clone();
            _scytheModel = DI.GetService<AppSettingsModel>().PlayerModel.scytheModel;
            _oldScytheModel = _scytheModel;
            var eventBus = DI.GetService<IEventBus<GameEvent>>();
            eventBus.Subscribe<OnGameUpdateEvent>(RefreshInventary);
            eventBus.Subscribe<OnScytheUpdateEvent>(RefreshIScythe);

            Refresh();
        }

        private void RefreshIScythe(OnScytheUpdateEvent eventData)
        {
            _scytheModel = eventData.Model;
            UpdateScytheLVL();
            _oldScytheModel = _scytheModel;
        }

        private void RefreshInventary(OnGameUpdateEvent eventData)
        {
            _inventoryModel = eventData.Model;
            Refresh();
            _oldInventoryModel = (InventoryModel) _inventoryModel.Clone();
        }

        private void Refresh()
        {
            CleanItems();
            CreateItems();
            UpdateInventoryLVL();
            UpdateScytheLVL();
        }

        private void UpdateScytheLVL()
        {
            _scytheLevel.text = $"LVL {_scytheModel.level}";
            if (_scytheModel.level != _oldScytheModel.level)
            {
                Instantiate(_scaleUpAnimPref, _scytheLevel.transform);
            }
        }

        private void UpdateInventoryLVL()
        {
            _inventaryLevel.text = $"LVL {_inventoryModel.level}";
            if (_inventoryModel.level != _oldInventoryModel.level)
            {
                Instantiate(_scaleUpAnimPref, _inventaryLevel.transform);
            }
        }

        private void CreateItems()
        {
            var keys = _itemsPrefabsDict.Keys;
            foreach (var key in keys)
            {
                if (_inventoryModel.itemsValue.ContainsKey(key))
                {
                    var itemUIElement = _itemsPrefabsDict[key];
                    var itemValue = _inventoryModel.itemsValue[key];
                    var oldItemValue = _oldInventoryModel.itemsValue[key];
                    var isAnimated = itemValue != oldItemValue;
                    if (key == ItemType.Gold || key == ItemType.Crystal)
                    {
                        InstantiateElement(itemUIElement, $"{itemValue}", key);
                    }
                    else if (itemValue > 0)
                    {
                        InstantiateElement(itemUIElement, $"{itemValue}/{_inventoryModel.limit}", key);
                    }
                }
            }
        }

        private void CleanItems()
        {
            foreach (Transform item in _itemsContainer)
                Destroy(item.gameObject);
        }

        private void InstantiateElement(GameObject IElement, string value, ItemType key)
        {
            var item = Instantiate(IElement, _itemsContainer, false);
            var element = item.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            element.text = value;
            
            if (_inventoryModel.itemsValue[key] > _oldInventoryModel.itemsValue[key])
            {
                Instantiate(_scaleUpAnimPref, element.transform);
            }
            if (_inventoryModel.itemsValue[key] < _oldInventoryModel.itemsValue[key])
            {
                Instantiate(_scaleDownAnimPref, element.transform);
            }
        }
    }
}
