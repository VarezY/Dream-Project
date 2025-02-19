using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Varez.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float gravity = -15f;
        [SerializeField] private float moveSpeed, sprintSpeed;
        [Range(0.0f, 0.3f)]
        [SerializeField] private float rotationSmoothTime = 0.12f;
        [SerializeField] private float fallTimeout = 0.15f;
        [SerializeField] private float groundedOffset = 0.14f;
        [SerializeField] private float groundedRadius = 0.28f;
        [SerializeField] private LayerMask groundLayers;

        private GameManager _gameManager;
        private PlayerManager _playerManager;
        private PlayerInput _playerInput;
        private CharacterController _characterController;
        private Camera _mainCamera;
        private float _targetRotation;
        private float _rotationVelocity;
        private float _fallTimeoutDelta;
        private float _verticalVelocity;
        private bool _isGrounded;
        private float _terminalVelocity;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _playerInput = GetComponent<PlayerInput>();
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _playerManager = PlayerManager.Instance;
            _gameManager = GameManager.Instance;
            
            _gameManager.PlayerEvent.OnPlayerStanding += OnPlayerStanding;
        }

        private void OnDisable()
        {
            _gameManager.PlayerEvent.OnPlayerStanding -= OnPlayerStanding; 
        }

        private void OnPlayerStanding()
        {
            _playerInput.enabled = true;
        }

        private void Update()
        {
            JumpAndGravity();
            CheckGrounded();
            
            Move();
        }

        private void CheckGrounded()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset,
                transform.position.z);
            _isGrounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);
        }
        
        private void JumpAndGravity()
        {
            if (_isGrounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = fallTimeout;

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity <= 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                /*// Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }*/
            }
            else
            {
                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += gravity * Time.deltaTime;
            }
        }

        private void Move()
        {
            Vector2 moveValue = _playerManager.Input.moveValue;
            bool isWalking = _playerManager.Input.isWalking;
            if (!isWalking) return;
            
            float targetSpeed = _playerManager.Input.isSprint ? sprintSpeed : moveSpeed;

            if (moveValue == Vector2.zero) targetSpeed = 0.0f;
            
            Vector3 inputDirection = new Vector3(moveValue.x, 0.0f, moveValue.y).normalized;
            
            if (moveValue != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            _characterController.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime) +
                                      new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }
    }
}