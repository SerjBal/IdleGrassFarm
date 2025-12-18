using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Serjbal
{
    [Serializable]
    public struct InventoryModel
    {
        [SerializeField] private ItemPrice[] itemsDefaults;
        public Dictionary<ItemType, int> itemsValue;
        public int limit;
        public int level;
        
        public void InitializeDictionary()
        {
            itemsValue = itemsDefaults.ToDictionary(
                x => x.item,   
                x => x.value       
            );
        }
    }
}