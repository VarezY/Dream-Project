using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Varez.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance { get; private set; }
        
        [Header("Start Configuration")]
        [SerializeField] private Button startButton;
        [SerializeField] private CanvasGroup menuCanvasGroup;
        [SerializeField] private RectTransform menuBar;
        [SerializeField] private Image fadeInPanel;
        [SerializeField] private CinemachineCamera virtualCamera;
        
        public Action OnStartGame;
        
        private Sequence _startSqSequence;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            startButton.onClick.AddListener(StartSequence);
        }

        private void StartSequence()
        {
            Sequence tempSequence = DOTween.Sequence();

            tempSequence.Append(menuBar.DOMoveY(-75, 1f));
            tempSequence.Insert(0, menuCanvasGroup.DOFade(0, 1f));
            // tempSequence
            tempSequence.AppendCallback(() =>
            {
                OnStartGame?.Invoke();
            });
            tempSequence.Append(fadeInPanel.DOFade(1, 1.5f));
            tempSequence.Insert(1, DOVirtual.Float(20, 0.5f, 1.5f, v => virtualCamera.Lens.OrthographicSize = v));
            tempSequence.AppendCallback(() =>
            {
                SceneManager.LoadScene("Gameplay");
            });

            _startSqSequence = tempSequence;
        }
    }
}