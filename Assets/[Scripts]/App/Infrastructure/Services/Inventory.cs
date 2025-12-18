using System;
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
        
        public void PutItem(ItemPrice price)
        {
            _model.itemsValue[price.item] += price.value;
            _eventBus.Raise(new OnItemsUpdateEvent(_model));
        }

        public void TakeItem(ItemPrice price)
        {
            if (CheckPrice(price)) 
            {
                _model.itemsValue[price.item] -= price.value;
                _eventBus.Raise(new OnItemsUpdateEvent(_model));
            }
            else
            {
                //no item event
            }
        }

        public void TakeItem(ItemPrice[] price)
        {
            foreach (var p in price)
                TakeItem(p);
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

        public bool CheckPrice(ItemPrice[] price)
        {
            bool result = true;
            foreach (var p in price)
            {
                if (_model.itemsValue[p.item] >= p.value)
                    return false;
            }
            return result;
        }

        public bool CheckPrice(ItemPrice price)
        {
            return _model.itemsValue[price.item] >= price.value;
        }
    }
}