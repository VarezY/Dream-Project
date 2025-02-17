using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Varez.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }
        public PlayerInputReader Input { get; private set; }

        [SerializeField] private CinemachineCamera dialogueCamera;
        
        private GameManager _gameManager;
        
        private void Awake()
        {
            Instance = this;

            Input = GetComponent<PlayerInputReader>();
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            
            _gameManager.UIEvents.OnPlayDialogue += OnPlayDialogue;
        }

        private void OnPlayDialogue(string arg1, string arg2)
        {
            dialogueCamera.gameObject.SetActive(true);
        }
    }
}