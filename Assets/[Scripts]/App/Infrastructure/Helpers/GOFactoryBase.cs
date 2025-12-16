using UnityEngine;
using System;

namespace Serjbal
{
    public abstract class Factorys
    { 
        public static T Create<T>(string name, GameObject prefab, Transform container) where T : class
        {
            var pageInstance = UnityEngine.Object.Instantiate(prefab, container);
            pageInstance.name = name;
            if (pageInstance.TryGetComponent<T>(out var newPage))
            {
                return newPage;
            }

            throw new InvalidOperationException($"{prefab.name} is missing component {typeof(T)}");
        }
    }
}