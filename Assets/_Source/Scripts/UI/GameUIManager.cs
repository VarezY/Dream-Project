using UnityEngine;

namespace Varez.UI
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject interactButton;
        
        [Header("Pop Up Configuration")]
        [SerializeField] private GameObject messagePrefab;
        [SerializeField] private Transform messageContainer;
        [SerializeField] private float waktuDatangKeluar = 1f;
        [SerializeField] private float waktuTahan = 1.5f;

        private void Start()
        {
            GameManager.Instance.UIEvents.OnAbleInteract += OnAbleInteract;
        }

        public void CreateMessage(string message, Sprite icon = null)
        {
            PopUpAnimation messageComponent = CreatePopUp();

            messageComponent.SetTiming(waktuDatangKeluar, waktuTahan);
            messageComponent.SendMessage(message, icon);
        }

        public void CreateMessage(string message, InputActionEnum inputAction)
        {
            PopUpAnimation messageComponent = CreatePopUp();

            messageComponent.SetTiming(waktuDatangKeluar, waktuTahan);
            messageComponent.SendMessage(message, inputAction);
        }

        private PopUpAnimation CreatePopUp()
        {
            GameObject messageObject = Instantiate(messagePrefab, messageContainer);
            PopUpAnimation messageComponent = messageObject.GetComponent<PopUpAnimation>();

            messageComponent.SetTiming(waktuDatangKeluar, waktuTahan);
            return messageComponent;
        }
        
        private void OnAbleInteract(bool isInteracting)
        {
            interactButton.SetActive(isInteracting);
        }
    }
}