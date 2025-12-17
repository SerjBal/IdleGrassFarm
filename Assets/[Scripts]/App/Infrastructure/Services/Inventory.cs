using Serjbal.Core;
using UnityEngine;

namespace Serjbal.Infrastructure.Services
{
    public class Inventory : MonoBehaviour, IInventory
    {
        private InventoryModel _model;
        private IEventBus<InventoryEvent> _eventBus;

        public void Init()
        {
            _eventBus = DI.GetService<IEventBus<InventoryEvent>>();
            _model = DI.GetService<AppSettingsModel>().inventoryModel;
        }
        
        public void PutItem(string itemType, int value)
        {
            _model.itemsValue[itemType] += value;
            _eventBus.Raise(new OnItemsUpdateEvent(_model));
        }

        public void TakeItem(string itemType, int value)
        {
            if (_model.itemsValue[itemType] >= value)
            {
                _model.itemsValue[itemType] = value;
                _eventBus.Raise(new OnItemsUpdateEvent(_model));
            }
            else
            {
                //no item event
            }
        }
        
        public int CheckItem(string itemType)
        {
            return _model.itemsValue[itemType];
        }

        public void SetLevel(int level)
        {
            var diff = level - _model.level;
            _model.level = level;
            _eventBus.Raise(new OnLevelUpdateEvent(diff));
        }

        public InventoryModel InventoryInfo()
        {
            return _model;
        }

        public bool CheckPrice(ItemPrice[] dataLevelPrice)
        {
            bool result = true;
            foreach (var price in dataLevelPrice)
            {
                if (_model.itemsValue[price.item] < price.value)
                    return false;
            }
            return result;
        }
    }
}