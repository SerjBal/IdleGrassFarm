using System;
using Serjbal.Core;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private Transform _character;
        public Action<Vector3> OnMow { get; set; }
        public Action OnUpgrade { get; set; }
        public Action OnSell { get; set; }
        

        public void Init()
        {
            Debug.Log("Player Initialized");
        }
        
        
        private void Update()
        {
            OnMow.Invoke(_character.position);
        }
    }

    public interface IPlayer : IService, IInitializable
    {
        Action<Vector3> OnMow { get; set; }
        Action OnUpgrade { get; set; }
        Action OnSell { get; set; }

        // void AddToInventory(ItemType itemType, int value);
        //
        // void GetFromInventory(ItemType itemType, int value);
    }
}
