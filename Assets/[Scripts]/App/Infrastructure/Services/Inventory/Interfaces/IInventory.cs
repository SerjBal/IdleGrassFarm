using Serjbal.Core;

namespace Serjbal.Infrastructure.Services
{
    public interface IInventory : IService, IInitializable
    {
        void PutItem(ItemPrice price);
        void TakeItem(ItemPrice price);
        void TakeItem(ItemPrice[] price);
        void SetLevel(int level);
        InventoryModel InventoryInfo();
        bool CheckPrice(ItemPrice[] price);
        bool CheckPrice(ItemPrice price);
        
        bool CheckLimit(ItemPrice[] price);
        bool CheckLimit(ItemPrice price);
        void Refresh();
    }
}