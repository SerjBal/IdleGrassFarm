using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Serjbal
{
    [Serializable]
    public class ScytheModel
    {
        public readonly float powDefaultRadius = 0.1f;
        public readonly float levelCoef = 2f;
        public int level = 1;
        public float mowSpeed;
    }
}