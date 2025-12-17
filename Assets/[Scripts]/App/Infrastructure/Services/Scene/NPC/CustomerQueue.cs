using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class CustomerQueue : MonoBehaviour, ICustomer
    {
        [SerializeField] private string _itemType;
        public void Interact()
        {
            var inventory = DI.GetService<IInventory>();
            inventory.TakeItem(_itemType, 1);
            switch (_itemType)
            {
                case "GreenGrass":
                   inventory.PutItem("Gold", 5); 
                   break;
                case "YellowGrass":
                    inventory.PutItem("Crystal", 1); 
                  break;
            }
        }
    }
}
