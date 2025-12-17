using System;
using System.Collections.Generic;
using UnityEngine;

namespace Serjbal
{
    [Serializable]
    public class InventoryModel
    {
        public Dictionary<string, int> itemsValue;
        public int limit;
        public int level = 1;


        public InventoryModel()
        {
            itemsValue = new Dictionary<string, int>();
            itemsValue.Add("Gold", 0);
            itemsValue.Add("Crystal", 0);
            itemsValue.Add("GreenGrass", 0);
            itemsValue.Add("YellowGrass", 0);
            
        }
    }
}