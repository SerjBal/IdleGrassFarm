using System;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private MoveController _character;
        private float _powRadius;
        private float _mowAnimT;
        private ScytheModel _model;
        public Action<Vector3, float> OnMow { get; set; }

        public void Init()
        {
            var settings = DI.GetService<AppSettingsModel>();
            _character.moveSpeed = settings.playerSpeed;
            _character.rotationSpeed = settings.playerRotateionSpeed;
            
            _model = settings.scytheModel;
            SetScytheLevel(_model.level);
            
            Debug.Log("Player Initialized");
        }
        
        public void SetScytheLevel(int level)
        {
            _model.level = level;
            _powRadius = _model.powDefaultRadius * _model.level/_model.levelCoef;
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
                _mowAnimT = 1;
                OnMow?.Invoke(_character.transform.position, _powRadius);
            }
            else
            {
                _mowAnimT = Mathf.Clamp01(_mowAnimT - Time.deltaTime * _model.mowSpeed);
            }
        }
    }
}
