using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Varez.CameraControl;

namespace Varez.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        [Header("READ ONLY")]
        public Vector2 moveValue;
        public bool isWalking;
        public bool isSprint;
        
        private GameManager _gameManager;
        private CameraEvents _cameraEvents;

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        public void OnControlsChanged(PlayerInput input)
        {
            if (_gameManager) _gameManager.ChangeUI(input);
        }

        public void OnMove(InputValue value)
        {
            moveValue = value.Get<Vector2>();
            if (moveValue.x != 0 || moveValue.y != 0)
            {
                isWalking = true;
                // GameManager.Instance.PlayerEvent.OnPlayerWalking.Invoke(isWalking);
            }
            else
            {
                isWalking = false;
            }
            GameManager.Instance.PlayerEvent.OnPlayerWalking?.Invoke(isWalking);
        }

        public void OnSprint(InputValue value)
        {
            isSprint = value.isPressed;
            if (!isWalking)
                return;
            GameManager.Instance.PlayerEvent.OnPlayerSprinting?.Invoke(isSprint);
        }
        
        public void OnRotateRight(InputValue value)
        {
            _gameManager.CameraEvents.OnTurnRight?.Invoke();
        }

        public void OnRotateLeft(InputValue value)
        {
            _gameManager.CameraEvents.OnTurnLeft?.Invoke();
        }
    }
}