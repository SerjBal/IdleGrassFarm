using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serjbal
{
    [Serializable]
    public class InventoryModel
    {
        [SerializeField] private ItemPrice[] itemsDefaults;
        public Dictionary<ItemType, int> itemsValue = new Dictionary<ItemType, int>();
        public int limit;
        public int level = 1;
        
        public void InitializeDictionary()
        {
            itemsValue = itemsDefaults.ToDictionary(
                x => x.item,   
                x => x.value       
            );
        }
    }
}