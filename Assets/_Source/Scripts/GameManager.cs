using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Varez.Audio;
using Varez.CameraControl;
using Varez.Player;
using Varez.UI;

namespace Varez
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [Header("UI Elements")]
        public GameUIScriptables gameUIScriptable;

        [Header("Cinemachine Camera")] 
        [SerializeField] private bool skipCutscene;
        [SerializeField] private CinemachineCamera orthoCamera;
        [SerializeField] private CinemachineCamera dialogueCamera;
        
        [Header("UI Configuration")]
        [SerializeField] private Image fadeOutImage;
        
        public GameUIEvents UIEvents;
        public CameraEvents CameraEvents;
        public PlayerEvents PlayerEvent;
        
        public AudioSource mainAudioSource;

        public AudioManager AudioManager {get; private set;}
        public GameUIManager GameUIManager {get; private set;}
        
        private Sequence _gameStartSequence;
        
        private void Awake()
        {
            Instance = this;
            AudioManager = GetComponentInChildren<AudioManager>();
            GameUIManager = GetComponentInChildren<GameUIManager>();
            mainAudioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (!skipCutscene)
            {
                StartGameSequence();
            }
            else
            {
                PlayerEvent.OnPlayerStanding?.Invoke();
            }
        }

        public void SendDebugMessage(string message)
        {
            Debug.Log(message);
        }
        
        public void ChangeUI(PlayerInput input)
        {
            switch (input.currentControlScheme)
            {
                case "PC":
                    Debug.Log("Game Scheme Change to PC");
                    UIEvents.OnSchemeChange?.Invoke(input.currentControlScheme);
                    break;
                case "Gamepad":
                    Debug.Log("Game Scheme Change to Gamepad");
                    UIEvents.OnSchemeChange?.Invoke(input.currentControlScheme);
                    break;
            }
        }

        private void StartGameSequence()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(fadeOutImage.DOFade(0, 2.5f));
            sequence.Insert(0, DOVirtual.Float(.5f, 5f, 2.5f, value => orthoCamera.Lens.OrthographicSize = value));
            sequence.AppendInterval(9f);
            sequence.AppendCallback(() => PlayerEvent.OnPlayerStanding?.Invoke());
            
            _gameStartSequence = sequence;
        }
    }
}
