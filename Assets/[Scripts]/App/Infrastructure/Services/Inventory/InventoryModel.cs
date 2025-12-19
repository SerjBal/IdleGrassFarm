using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Serjbal
{
    [Serializable]
    public struct InventoryModel : ICloneable
    {
        [SerializeField] private ItemPrice[] _itemsDefaults;
        public Dictionary<ItemType, int> itemsValue;
        public int limit;
        public int level;
        public int limitLevelCoef;
        public object Clone()
        {
            return new InventoryModel
            {
                itemsValue = itemsValue != null ? new Dictionary<ItemType, int>(itemsValue) : null,
                limit = limit,
                level = level,
                limitLevelCoef = limitLevelCoef
            };
        }

        public void Initialize()
        {
            itemsValue =  _itemsDefaults.ToDictionary(
                x => x.item,   
                x => x.value       
            );
        }
    }
}