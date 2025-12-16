using UnityEngine;

namespace Serjbal.Infrastructure.Services
{
    public enum ItemType
    {
        GreenGrass, YellowGrass, Gold
    }

    public interface IInventory : IService
    {
        
    }
        
    public class Inventory : MonoBehaviour, IInventory
    {
        
    }
}