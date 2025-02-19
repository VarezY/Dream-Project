using System;
using Ink.Runtime;
using UnityEngine;

namespace Varez.Interact
{
    public class InteractablePerson : InteractableObject
    {
        [SerializeField] private bool isOneTime;
        [SerializeField] private TextAsset dialogueInkText = null;
        
        public static event Action<Story> OnCreateStory;

        private Story _story;
        private string _text;
        private bool _isTalking;
        
        protected override void Start()
        {
            base.Start();
            GameManager.Instance.UIEvents.OnNextDialogue += OnNextDialogue;
            
            _story = new Story(dialogueInkText.text);
        }

        private void OnDisable()
        {
            GameManager.Instance.UIEvents.OnNextDialogue -= OnNextDialogue;
        }

        private void OnNextDialogue()
        {
            if (!_isTalking) return;
            if (_story.canContinue)
            {
                _text = _story.Continue();
                GameManager.Instance.UIEvents.OnPlayDialogue?.Invoke(gameObject.name, _text);
            }
            else
            {
                _isTalking = false;
                GameManager.Instance.UIEvents.OnExitDialogue?.Invoke();
            }
        }

        public override void OnInteract()
        {
            base.OnInteract();
            OnCreateStory?.Invoke(_story);
            _isTalking = true;
            OnNextDialogue();
            isInteractable = false;
        }
    }
}