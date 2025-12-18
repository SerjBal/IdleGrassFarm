using System;
using System.Collections.Generic;
using System.Linq;
using Serjbal.Core;
using UnityEngine;

namespace Serjbal
{
    public class InventoryView : MonoBehaviour
    {
        [Serializable]
        public class ItemPrefab
        {
            public ItemType item;
            public GameObject prefab;
        }
        
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private ItemPrefab[] _itemPrefabs;
        [SerializeField] private TMPro.TextMeshProUGUI _head;
        
        private Dictionary<ItemType, GameObject> _itemsPrefabsDict = new Dictionary<ItemType, GameObject>();
        private InventoryModel _inventoryModel;
        
        private void Awake()
        {
            foreach (var prefab in _itemPrefabs)
            {
                _itemsPrefabsDict = _itemPrefabs.ToDictionary(
                    x => x.item,   
                    x => x.prefab       
                );
            }

            CleanItems();
        }

        private void Start()
        {
            var eventBus = DI.GetService<IEventBus<InventoryEvent>>();
            eventBus.Subscribe<OnInventoryUpdateEvent>(SetData);
            
        }

        private void SetData(OnInventoryUpdateEvent eventData)
        {
            _inventoryModel = eventData.Model;
            Refresh();
        }

        private void Refresh()
        {
            CleanItems();
            CreateItems();
        }

        private void CreateItems()
        {
            _head.text = $"LVL {_inventoryModel.level}";
            
            var keys = _itemsPrefabsDict.Keys;
            foreach (var key in keys)
            {
                if (_inventoryModel.itemsValue.ContainsKey(key))
                {
                    var itemUIElement = _itemsPrefabsDict[key];
                    var itemValue = _inventoryModel.itemsValue[key];
                    if (itemValue > 0)
                    {
                        InstantiateElement(itemUIElement, $"{itemValue}/{_inventoryModel.limit}");
                    }
                    else
                    {
                        if (key == ItemType.Gold)
                        {
                            InstantiateElement(itemUIElement, $"{itemValue}");
                        }
                    }
                }
            }
        }

        private void CleanItems()
        {
            foreach (Transform item in _itemsContainer)
                Destroy(item.gameObject);
        }

        private void InstantiateElement(GameObject IElement, string value)
        {
            var item = Instantiate(IElement, _itemsContainer, false);
            var element = item.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            element.text = value;
        }
    }
}
