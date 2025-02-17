using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Varez.CameraControl
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("Direction Settings")]
        [SerializeField] private HashSet<CameraCardinalDirection> allowedDirections = new HashSet<CameraCardinalDirection>();
        [SerializeField] private CameraCardinalDirection startingDirection = CameraCardinalDirection.North;

        [Header("DOTween Animation Settings")]
        [SerializeField] private float rotationDuration = 0.5f;
        [SerializeField] private Ease rotationEase = Ease.InOutQuad;
        [SerializeField] private float bounceStrength = 15f;

        [Header("Cinemachine Settings")]
        [SerializeField] private Vector3 baseFollowOffset = new Vector3(0, 2, -5);
        [SerializeField] private Ease shakeEase = Ease.InOutQuint;

        private GameManager _gameManager;
        private CameraCardinalDirection _currentDirection = CameraCardinalDirection.North;
        private CinemachineBasicMultiChannelPerlin _cameraNoise;
        private Tween _currentRotationTween;
        private Tween _currentOffsetTween;
        private Tween _shakeTween;

        private void Awake()
        {
            _cameraNoise = GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            InitializeRotation();
            _gameManager.CameraEvents.OnTurnRight += TryRotateClockwise;
            _gameManager.CameraEvents.OnTurnLeft += TryRotateCounterClockwise;
            _gameManager.CameraEvents.OnShake += AnimateShake;
        }

        private void AnimateShake(float intensity, float duration)
        {
            _shakeTween?.Kill();
            _shakeTween = DOVirtual.Float(intensity, 0, duration, 
                value => _cameraNoise.AmplitudeGain = value)
                .SetEase(shakeEase);
        }

        public void LoadRotationPreset(params CameraCardinalDirection[] directions)
        {
            allowedDirections.Clear();
            foreach (var direction in directions)
            {
                allowedDirections.Add(direction);
            }

            if (!allowedDirections.Contains(_currentDirection))
            {
                RotateToNearAllowedDirection();
            }

            Debug.Log($"Camera rotation preset loaded. Allowed directions: {string.Join(", ", directions)}");
        }
        
        public void LoadRotationPreset(IEnumerable<CameraCardinalDirection> directions)
        {
            LoadRotationPreset(directions.ToArray());
        }
        
        public CameraCardinalDirection GetCurrentDirection()
        {
            return _currentDirection;
        }
        
        private void InitializeRotation()
        {
            // Set initial allowed directions if none are specified
            if (allowedDirections.Count == 0)
            {
                allowedDirections = new HashSet<CameraCardinalDirection>
                {
                    CameraCardinalDirection.North,
                    CameraCardinalDirection.East,
                    CameraCardinalDirection.South,
                    CameraCardinalDirection.West
                };
            }

            _currentDirection = startingDirection;
        }
        
        private void RotateToNearAllowedDirection()
        {
            if (allowedDirections.Count == 0) return;

            CameraCardinalDirection nearestDirection = _currentDirection;
            int minAngleDifference = int.MaxValue;

            foreach (CameraCardinalDirection direction in allowedDirections)
            {
                int angleDifference = Mathf.Abs(Mathf.RoundToInt(
                    Mathf.DeltaAngle((float)_currentDirection, (float)direction)
                ));

                if (angleDifference >= minAngleDifference) continue;
                
                minAngleDifference = angleDifference;
                nearestDirection = direction;
            }

            RotateToDirection(nearestDirection);
        }

        
        private void TryRotateClockwise()
        {
            CameraCardinalDirection nextDirection = _currentDirection;
            bool foundDirection = false;
        
            for (int i = 1; i <= 4; i++)
            {
                CameraCardinalDirection testDirection = (CameraCardinalDirection)(((int)_currentDirection - (45 * i) + 360) % 360);
                if (!allowedDirections.Contains(testDirection)) continue;
               
                nextDirection = testDirection;
                foundDirection = true;
                break;
            }

            if (foundDirection)
            {
                RotateToDirection(nextDirection);
            }
            else
            {
                RotateBounceBack(true);
            }
        }

        private void TryRotateCounterClockwise()
        {
            CameraCardinalDirection nextDirection = _currentDirection;
            bool foundDirection = false;
        
            for (int i = 1; i <= 4; i++)
            {
                CameraCardinalDirection testDirection = (CameraCardinalDirection)(((int)_currentDirection + (45 * i)) % 360);
                if (!allowedDirections.Contains(testDirection)) continue;
               
                nextDirection = testDirection;
                foundDirection = true;
                break;
            }

            if (foundDirection)
            {
                RotateToDirection(nextDirection);
            }
            else
            {
                RotateBounceBack(false);
            }
        }

        
        private void RotateToDirection(CameraCardinalDirection newDirection)
        {
            if (!allowedDirections.Contains(newDirection))
            {
                Debug.LogWarning($"Attempted to rotate to restricted direction: {newDirection}");
                return;
            }
            
            // Kill any ongoing tweens to prevent conflicts
            KillOngoingTweens();

            float targetRotationY = (float)newDirection;
            _currentDirection = newDirection;

            // Calculate the shortest rotation path
            float currentRotationY = transform.eulerAngles.y;
            float rotationDelta = Mathf.DeltaAngle(currentRotationY, targetRotationY);

            // Create the rotation sequence
            Sequence rotationSequence = DOTween.Sequence();
            
            // Simple smooth rotation
            rotationSequence.Append(transform.DORotate(
                new Vector3(30, targetRotationY, 0),
                rotationDuration,
                RotateMode.Fast)
                .SetEase(rotationEase)
            );

            _currentRotationTween = rotationSequence;
        }

        private void RotateBounceBack(bool isRight)
        {
            if (IsAnimating())
            {
                return;
            }
            
            float currentRotationY = transform.eulerAngles.y;

            Sequence rotationSequence = DOTween.Sequence();

            rotationSequence.Append(transform.DORotate(
                    new Vector3(30, isRight ? currentRotationY - bounceStrength : currentRotationY + bounceStrength, 0),
                    rotationDuration * 0.5f,
                    RotateMode.Fast)
                .SetEase(rotationEase)
                .SetLoops(2, LoopType.Yoyo)
            );
            
            _currentRotationTween = rotationSequence;
        }

        private bool IsAnimating()
        {
            return (_currentRotationTween != null && _currentRotationTween.IsPlaying()) ||
                   (_currentOffsetTween != null && _currentOffsetTween.IsPlaying());
        }
        
        private void KillOngoingTweens()
        {
            if (_currentRotationTween != null && _currentRotationTween.IsPlaying())
            {
                _currentRotationTween.Kill();
            }
        
            if (_currentOffsetTween != null && _currentOffsetTween.IsPlaying())
            {
                _currentOffsetTween.Kill();
            }
        }
    }
}
