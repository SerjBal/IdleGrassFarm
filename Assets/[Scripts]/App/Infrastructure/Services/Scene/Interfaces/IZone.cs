using System;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public interface IZone
    {
        Action<string, int> OnMewed { get; set; }
        void Mow(Vector3 position, float radius);
    }
}