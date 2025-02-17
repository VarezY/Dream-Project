using System;
using DG.Tweening;
using UnityEngine;

namespace Varez.MainMenu
{
    public class FlickerLight : MonoBehaviour
    {
        [SerializeField] private Light lightTarget;
        [SerializeField] private float blinkRate;
        [SerializeField] private float delayStart;
        [SerializeField] private bool isLoop;
        [SerializeField] private Light gloveLight;
        
        private Sequence _lightSequence;

        private void Awake()
        {
            if (!lightTarget)
            {
                lightTarget = GetComponent<Light>();
            }
        }

        private void Start()
        {
            Flicker();
            MainMenuManager.Instance.OnStartGame += OnStartGame;
            // InvokeRepeating(nameof(Flicker), delayStart, blinkRate);
        }

        private void OnStartGame()
        {
            _lightSequence.Kill();
        }

        [ContextMenu("Flicker Light")]
        private void Flicker()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(lightTarget.DOIntensity(800, 0f).SetEase(Ease.Linear));
            sequence.Append(lightTarget.DOIntensity(1500, 0f).SetEase(Ease.Linear).SetDelay(.1f));
            sequence.AppendInterval(blinkRate);
            sequence.SetLoops(-1);
            
            _lightSequence = sequence;
        }
    }
}