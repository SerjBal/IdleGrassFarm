using Serjbal.Core;

namespace Serjbal.Infrastructure.Services
{
    public interface IInventory : IService, IInitializable
    {
        void PutItem(string itemType, int value);
        void TakeItem(string itemType, int value);
        int CheckItem(string itemType);
        void SetLevel(int level);
        void InventoryInfo();
    }
}