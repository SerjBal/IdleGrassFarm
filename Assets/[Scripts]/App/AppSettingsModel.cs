using System;
using UnityEngine;

namespace Serjbal
{
    [Serializable]
    public class AppSettingsModel
    {
        [Header("Graphics Settings")]
        //public int qualityLevel = 2;
        //public Resolution resolution = new Resolution { width = 1920, height = 1080 };
        public bool fullscreen = true;
        //public float brightness = 1f;
    }
}