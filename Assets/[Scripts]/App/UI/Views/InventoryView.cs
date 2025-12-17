using System.Collections.Generic;
using Serjbal.Core;
using UnityEngine;

namespace Serjbal
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private GameObject[] _itemPrefabs;
        [SerializeField] private TMPro.TextMeshProUGUI _head;
        
        private Dictionary<string, GameObject> _itemsPrefabsDict = new Dictionary<string, GameObject>();
        private InventoryModel _inventoryModel;
        
        private void Awake()
        {
            foreach (var prefab in _itemPrefabs)
                _itemsPrefabsDict.Add(prefab.name, prefab);
            CleanItems();
        }

        private void Start()
        {
            var eventBus = DI.GetService<IEventBus<InventoryEvent>>();
            eventBus.Subscribe<OnItemsUpdateEvent>(SetData);
        }

        private void SetData(OnItemsUpdateEvent eventData)
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
                var itemUIElement = _itemsPrefabsDict[key];
                var itemValue = _inventoryModel.itemsValue[key];
                if (itemValue > 0)
                {
                    InstantiateElement(itemUIElement, $"{itemValue}/{_inventoryModel.limit}");
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
