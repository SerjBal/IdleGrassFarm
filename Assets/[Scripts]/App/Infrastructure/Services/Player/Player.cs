using System;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private Inventory _inventory;
        [SerializeField] private MoveController _character;
        public Action<Vector3> OnMow { get; set; }

        public void Init()
        {
            Debug.Log("Player Initialized");
        }
        
        public void PutToInventory(string itemType, int value)
        {
            DI.GetService<IInventory>().PutItem(itemType, value);
        }

        public void TakeFromInventory(string itemType, int value)
        {
            DI.GetService<IInventory>().TakeItem(itemType, value);
        }

        public int CheckInventory(string itemType)
        {
            return DI.GetService<IInventory>().CheckItem(itemType);
        }

        public void UpgradeInventory(int level)
        {
            DI.GetService<IInventory>().SetLevel(level);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var upgrader))
            {
                upgrader.Interact();
            }
        }

        private void Update()
        {
            if (_character.IsMoving && _mowAnimT == 0)
            {
                _mowAnimT = _mowAnimTime;
                OnMow?.Invoke(_character.transform.position);
            }
            else
            {
                _mowAnimT = Mathf.Clamp01(_mowAnimT - Time.deltaTime);
            }
        }

        private float _mowAnimT = 0.5f;
        private float _mowAnimTime = 0.5f;
    }
}
