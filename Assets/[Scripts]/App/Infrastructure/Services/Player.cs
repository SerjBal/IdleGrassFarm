using System;
using Serjbal.Core;
using Serjbal.Infrastructure.Services;
using UnityEngine;

namespace Serjbal
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private MoveController _character;
        [SerializeField] private Transform _scythe;
        [SerializeField] private VisualTimer _buyTimer;
        private float _powRadius;
        private float _mowAnimT;
        private ScytheModel _model;
        
        private Animator _animator;
        private static readonly int IsMow = Animator.StringToHash("IsMow");
        
        public Action<Vector3, float> OnMow { get; set; }

        public void Init()
        {
            var settings = DI.GetService<AppSettingsModel>();
            _character.moveSpeed = settings.playerSpeed;
            _character.rotationSpeed = settings.playerRotateionSpeed;
            
            _model = settings.scytheModel;
            SetScytheLevel(_model.level);

            _animator = _character.GetComponent<Animator>();
                
            Debug.Log("Player Initialized");
        }
        
        public void SetScytheLevel(int level)
        {
            _model.level = level;
            _powRadius = _model.defaultRadius + (_model.level - 1) * _model.radiusLevelCoef;
        }

        public ScytheModel GetScytheModel()
        {
            return _model;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var upgrader))
            {
                _buyTimer.action = upgrader.Interact;
                _buyTimer.Show();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IInteractable>(out var upgrader))
            {
                _buyTimer.Hide();
            }
        }

        private void Update()
        {
            Mow();
            _animator.SetBool(IsMow, _character.IsMoving);
        }

        private void Mow()
        {
            if (_character.IsMoving && _mowAnimT == 0)
            {
                _mowAnimT = 1;
                OnMow?.Invoke(_scythe.position, _powRadius);
            }
            else
            {
                _mowAnimT = Mathf.Clamp01(_mowAnimT - Time.deltaTime * _model.mowSpeed);
            }
        }
    }
}
