using System;
using UnityEngine.Serialization;

namespace Serjbal
{
    [Serializable]
    public struct ScytheModel
    {
        public float defaultRadius;
        public float radiusLevelCoef;
        public int level;
        public float mowSpeed;
    }
}