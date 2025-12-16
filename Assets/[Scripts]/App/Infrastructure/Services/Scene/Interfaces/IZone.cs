using System;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public interface IZone
    {
        Action<ItemType, int> OnMewed { get; set; }
        void Mow(Vector3 position);
        void SetMowRadius(int upgradeLevel);
    }
}