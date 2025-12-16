using System;
using Serjbal.Core;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public interface IPlayer : IService, IInitializable
    {
        Action<Vector3> OnMow { get; set; }
        void PutToInventory(ItemType itemType, int value);
        
        void TakeFromInventory(ItemType itemType, int value);
        
        int CheckInventory(ItemType itemType);
        
        void UpgradeInventory(int level);
    }
}