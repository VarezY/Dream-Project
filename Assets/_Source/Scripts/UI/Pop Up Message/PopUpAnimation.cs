using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Varez.UI
{
    public class PopUpAnimation : MonoBehaviour
    {
        public Image icon;
        public TMP_Text text;
        
        private RectTransform _rectTransform;
        private Sequence _popUpSequence;
        private UISchemeChangeHandler _schemeChangeHandler;
        private float _waktuMasukKeluar = 1f;
        private float _waktuTahan = 1.5f;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _schemeChangeHandler = GetComponentInChildren<UISchemeChangeHandler>();
        }

        [ContextMenu("Animate Pop Up")]
        public void AnimatePopUp()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DOMoveX(0, _waktuMasukKeluar).SetEase(Ease.InOutCubic));
            sequence.InsertCallback(0, () =>
            {
                GameManager.Instance.UIEvents.OnNotificationStart?.Invoke();
                GameManager.Instance.mainAudioSource.PlayOneShot(GameManager.Instance.AudioManager.defaultNotificationSound);
            });
            sequence.AppendInterval(_waktuTahan);
            sequence.Append(_rectTransform.DOMoveX(-500f, _waktuMasukKeluar).SetEase(Ease.InOutCubic));
            sequence.InsertCallback(2, () =>
            {
                GameManager.Instance.UIEvents.OnNotificationEnd?.Invoke();
            });
            sequence.AppendCallback(() => Destroy(this.gameObject));
        }

        public void SetTiming(float waktuMasukKeluar = 1f, float waktuTahan = 1.5f)
        {
            _waktuMasukKeluar = waktuMasukKeluar;
            _waktuTahan = waktuTahan;
        }
        
        public void SendMessage(string message, Sprite iconImage = null)
        {
            _schemeChangeHandler.enabled = false;
            icon.sprite = iconImage;
            text.text = message;
            AnimatePopUp();
        }

        public void SendMessage(string message, InputActionEnum inputAction)
        {
            _schemeChangeHandler.uiKey = inputAction;
            text.text = message;
            AnimatePopUp();
        }
    }
}
