using UnityEngine;

namespace Serjbal.Infrastructure.Services
{
    public enum ItemType
    {
        GreenGrass, YellowGrass, Gold
    }

    public class Inventory : MonoBehaviour, IInventory
    {
        public void PutItem(ItemType itemType, int value)
        {
           
        }

        public void TakeItem(ItemType itemType, int value)
        {
           
        }


        public int CheckItem(ItemType itemType)
        {
            return 0;
        }

        public void SetLevel(int level)
        {
           
        }

        public void InventoryInfo()
        {
            
        }
    }
}