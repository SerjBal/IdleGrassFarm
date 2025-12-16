namespace Serjbal.Infrastructure.Services
{
    public interface IInventory : IService
    {
        void PutItem(ItemType itemType, int value);
        void TakeItem(ItemType itemType, int value);
        int CheckItem(ItemType itemType);
        void SetLevel(int level);
        void InventoryInfo();
    }
}