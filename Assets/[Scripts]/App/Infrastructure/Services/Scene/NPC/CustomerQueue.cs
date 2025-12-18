using System;
using Serjbal.Core;
using Serjbal.Infrastructure.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Serjbal
{
    public class CustomerQueue : MonoBehaviour, ICustomer
    {
        [SerializeField] private Follower _character;
        private ItemType _itemToBuy;
        [SerializeField] private Transform[] _targets;
        private int _currentIndex;
        private IEventBus<InventoryEvent> _eventBus;

        private void Start()
        {
            _currentIndex = transform.GetSiblingIndex();
            _character.SetTarget(_targets[_currentIndex]);
            
            _eventBus = DI.GetService<IEventBus<InventoryEvent>>();
            _eventBus.Subscribe((NPCCustomerQueueEvent x) => GoNextTarget());
        }

        private void OnEnable()
        {
            _itemToBuy = Random.Range(0, 2) > 0 ? ItemType.GreenGrass : ItemType.YellowGrass;
        }

        public void Interact()
        {
            var inventory = DI.GetService<IInventory>();
            var price = new ItemPrice(_itemToBuy, 1);
            
            if (inventory.CheckPrice(price))
            {
                inventory.TakeItem(price);
                switch (_itemToBuy)
                {
                    case ItemType.GreenGrass:
                        inventory.PutItem(new ItemPrice(ItemType.Crystal, 1));
                        break;
                    default:
                         inventory.PutItem(new ItemPrice(ItemType.Gold, 10));
                        break;
                }

                GoNextTarget();
                _eventBus.Raise(new NPCCustomerQueueEvent());
            }
        }

        private void GoNextTarget()
        {
            OnEnable();
            _currentIndex = _currentIndex + 1 < transform.parent.childCount - 1
                ? _currentIndex + 1
                : 0;
                
            transform.SetSiblingIndex(_currentIndex);
            _character.SetTarget(_targets[_currentIndex]);
        }
    }
}
