using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Serjbal
{
    [RequireComponent(typeof(CharacterController))]
    public class MoveController : MonoBehaviour
    {
        public bool IsMoving;
        public float moveSpeed = 5f;
        public float rotationSpeed = 15f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _groundCheckDistance = 0.1f;
        [SerializeField] private CharacterController _characterController;
        
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private Vector2 _moveInput;
        private Vector3 _velocity;

        private Transform _camera;

        private void Awake()
        {
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
                
            _playerInput = GetComponent<PlayerInput>();
            
            if (_playerInput != null)
            {
                _moveAction = _playerInput.actions["Move"];
            }

            _camera = Camera.main.transform;
        }

        private void OnEnable()
        {
            if (_moveAction != null)
            {
                _moveAction.performed += OnMovePerformed;
                _moveAction.canceled += OnMoveCanceled;
            }
        }

        private void OnDisable()
        {
            if (_moveAction != null)
            {
                _moveAction.performed -= OnMovePerformed;
                _moveAction.canceled -= OnMoveCanceled;
            }
        }

        private void Update()
        {
            HandleMovement();
            HandleRotation();
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
            IsMoving = true;
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _moveInput = Vector2.zero;
            IsMoving = false;
        }

        private void HandleMovement()
        {
            if (_moveInput.magnitude < 0.1f) return;

            Vector3 cameraForward = _camera.forward;
            Vector3 cameraRight = _camera.right;
            
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraRight * _moveInput.x + cameraForward * _moveInput.y;
            
            if (moveDirection.magnitude > 1f)
                moveDirection.Normalize();

            Vector3 moveVelocity = moveDirection * moveSpeed;
            _characterController.Move(new Vector3(moveVelocity.x, 0, moveVelocity.z) * Time.deltaTime);
        }

        private void HandleRotation()
        {
            if (_moveInput.magnitude < 0.1f) return;

            Vector3 cameraForward = _camera.forward;
            Vector3 cameraRight = _camera.right;
            
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraRight * _moveInput.x + cameraForward * _moveInput.y;

            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}