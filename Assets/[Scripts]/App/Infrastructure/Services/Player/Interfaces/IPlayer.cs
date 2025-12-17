using System;
using Serjbal.Core;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public interface IPlayer : IService, IInitializable
    {
        Action<Vector3, float> OnMow { get; set; }
        void SetScytheLevel(int level);
        ScytheModel GetScytheModel();
    }
}