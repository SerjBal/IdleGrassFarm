using System;
using Serjbal.Core;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public interface IPlayer : IService, IInitializable
    {
        Action<Vector3> OnMow { get; set; }
        void PutToInventory(string itemType, int value);
        
        void TakeFromInventory(string itemType, int value);
        
        int CheckInventory(string itemType);
        
        void UpgradeInventory(int level);
    }
}