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
            _model = new InventoryModel();
        }
        
        public void PutItem(string itemType, int value)
        {
            _model.itemsValue[itemType] += value;
            _eventBus.Raise(new OnItemsUpdate(_model));
        }

        public void TakeItem(string itemType, int value)
        {
            if (_model.itemsValue[itemType] >= value)
            {
                _model.itemsValue[itemType] = value;
                _eventBus.Raise(new OnItemsUpdate(_model));
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
           
        }

        public void InventoryInfo()
        {
            
        }
    }
}