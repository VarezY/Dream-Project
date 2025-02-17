using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Varez.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");
        
        [SerializeField] private Animator animator;
        [Range(0, 1)] public float footstepAudioVolume = 0.5f;
        public AudioClip[] footstepAudioClips;
        
        private CharacterController _controller;
        private Tween _walkingTween;
        private float _animationBlend;
        private float _speed;
        private bool _isWalking;
        private bool _isRunning;

        private void Awake()
        {
            _controller = GetComponentInParent<CharacterController>();
        }

        private void Start()
        {
            // GameManager.Instance.PlayerEvent.OnPlayerWalking += OnPlayerWalking;
            // GameManager.Instance.PlayerEvent.OnPlayerSprinting += OnPlayerSprinting;
            _speed = 10f;
        }

        private void OnPlayerSprinting(bool isSprinting)
        {
            _isRunning = isSprinting;
        }

        private void OnPlayerWalking(bool isWalking)
        {
            _isWalking = isWalking;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            _isWalking = PlayerManager.Instance.Input.isWalking;
            _isRunning = PlayerManager.Instance.Input.isSprint;
            _animationBlend = _isWalking ? 
                Mathf.Lerp(_animationBlend, 2.5f, Time.deltaTime * _speed) : 
                Mathf.Lerp(_animationBlend, 0, Time.deltaTime * _speed);

            if (_isRunning && _isWalking)
            {
                _animationBlend = Mathf.Lerp(_animationBlend, 6f, Time.deltaTime * _speed);
            }
            
            if (_animationBlend < 0.01f)
                _animationBlend = 0f;

            animator.SetFloat(MotionSpeed, 1);
            animator.SetFloat(Speed, _animationBlend);
        }
        
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if ((animationEvent.animatorClipInfo.weight < 0.5f)) return;
            if (footstepAudioClips.Length <= 0) return;
            
            int index = Random.Range(0, footstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.TransformPoint(_controller.center), footstepAudioVolume);
        }
    }
}