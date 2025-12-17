using System.Collections.Generic;

namespace Serjbal
{
    public class InventoryModel
    {
        public Dictionary<string, int> itemsValue;
        public int limit;
        public int level;

        public InventoryModel()
        {
            itemsValue = new Dictionary<string, int>();
            itemsValue.Add("Gold", 0);
            itemsValue.Add("GreenGrass", 0);
            itemsValue.Add("YellowGrass", 0);
            limit = 0;
            level = 0;
        }
    }
}