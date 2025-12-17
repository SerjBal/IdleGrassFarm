using System;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private MoveController _character;
        private float _powRadius;
        private float _mowAnimT = 0.5f;
        private float _mowAnimTime = 0.5f;
        private ScytheModel _model;
        public Action<Vector3, float> OnMow { get; set; }

        public void Init()
        {
            Debug.Log("Player Initialized");
        }
        
        public void SetScytheLevel(int level)
        {
            _model.mowLevel = level;
            _powRadius = _model.powDefaultRadius * _model.mowLevel/_model.levelCoef;
        }

        public ScytheModel GetScytheModel()
        {
            return _model;
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
                OnMow?.Invoke(_character.transform.position, _powRadius);
            }
            else
            {
                _mowAnimT = Mathf.Clamp01(_mowAnimT - Time.deltaTime);
            }
        }
    }
}
