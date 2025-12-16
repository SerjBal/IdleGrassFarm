using UnityEngine;
using UnityEngine.InputSystem;

namespace Serjbal
{
    [RequireComponent(typeof(CharacterController))]
    public class MoveController : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 15f;
        [SerializeField] private float _gravity = -9.81f;
        [SerializeField] private float _groundCheckDistance = 0.1f;
        [SerializeField] private CharacterController _characterController;
        
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private Vector2 _moveInput;
        private Vector3 _velocity;
        private bool _isGrounded;

        private void Awake()
        {
            if (_characterController == null)
                _characterController = GetComponent<CharacterController>();
                
            _playerInput = GetComponent<PlayerInput>();
            
            if (_playerInput != null)
            {
                _moveAction = _playerInput.actions["Move"];
            }
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
            HandleGravity();
            HandleMovement();
            HandleRotation();
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _moveInput = Vector2.zero;
        }

        private void HandleMovement()
        {
            if (_moveInput.magnitude < 0.1f) return;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraRight * _moveInput.x + cameraForward * _moveInput.y;
            
            if (moveDirection.magnitude > 1f)
                moveDirection.Normalize();

            Vector3 moveVelocity = moveDirection * _moveSpeed;
            _characterController.Move(new Vector3(moveVelocity.x, 0, moveVelocity.z) * Time.deltaTime);
        }

        private void HandleRotation()
        {
            if (_moveInput.magnitude < 0.1f) return;

            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            
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
                    _rotationSpeed * Time.deltaTime
                );
            }
        }

        private void HandleGravity()
        {
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -0.5f;
            }
            else
            {
                _velocity.y += _gravity * Time.deltaTime;
            }

            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}