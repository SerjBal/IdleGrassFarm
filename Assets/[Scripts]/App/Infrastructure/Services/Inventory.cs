using System;
using Serjbal.Core;
using UnityEngine;

namespace Serjbal.Infrastructure.Services
{
    public class Inventory : MonoBehaviour, IInventory
    {
        private InventoryModel _model;
        private IEventBus<GameEvent> _eventBus;

        public void Init()
        {
            _eventBus = DI.GetService<IEventBus<GameEvent>>();
            _model = DI.GetService<AppSettingsModel>().InventoryModel;
        }
        
        public void PutItem(ItemPrice price)
        {
            _model.itemsValue[price.item] += price.value;
        }

        public void TakeItem(ItemPrice price)
        {
            if (CheckPrice(price)) 
            {
                _model.itemsValue[price.item] -= price.value;
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
            _model.limit += diff * _model.limitLevelCoef;
            _model.level = level;
        }

        public InventoryModel InventoryInfo() => (InventoryModel)_model.Clone();

        public bool CheckPrice(ItemPrice[] price)
        {
            foreach (var p in price)
            {
                if (!CheckPrice(p))
                    return false;
            }
            return true;
        }

        public bool CheckPrice(ItemPrice price)
        {
            return _model.itemsValue[price.item] >= price.value;
        }

        public bool CheckLimit(ItemPrice[] price)
        {
            foreach (var p in price)
            {
                if (!CheckLimit(p))
                    return false;
            }
            return true;
        }

        public bool CheckLimit(ItemPrice price)
        {
            if (price.item != ItemType.Gold || price.item != ItemType.Crystal)
            {
                return _model.itemsValue[price.item] + price.value <= _model.limit;
            }
            return true;
        }

        public void Refresh()
        {
            _eventBus.Raise(new OnGameUpdateEvent(_model));
        }
    }
}