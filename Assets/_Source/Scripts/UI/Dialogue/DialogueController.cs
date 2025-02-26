﻿using System;
using Febucci.UI;
using TMPro;
using UnityEngine;

namespace Varez.UI.Dialogue
{
    public class DialogueController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject dialogueBox;
        [SerializeField] private TMP_Text titleDialogue;
        [SerializeField] private TMP_Text textDialogue;
        
        [Header("Text Typewriters")]
        [SerializeField] private TypewriterByCharacter typewriter;

        private void Start()
        {
            GameManager.Instance.UIEvents.OnPlayDialogue += DisplayDialogue;
            GameManager.Instance.UIEvents.OnExitDialogue += OnExitDialogue;
            typewriter.onTextShowed.AddListener(NextDialogue);
        }

        private void OnExitDialogue()
        {
            dialogueBox.SetActive(false);
        }

        private void NextDialogue()
        {
            
        }

        private void OnDisable()
        {
            GameManager.Instance.UIEvents.OnPlayDialogue -= DisplayDialogue;
        }

        public void SingleDialogue()
        {
            
        }
        
        public void DisplayDialogue(string title, string text)
        {
            dialogueBox.SetActive(true);
            titleDialogue.text = title;
            typewriter.ShowText(text);
            // typewriter.onTextShowed
        }

        public void SkipTextDialogue()
        {
            typewriter.SkipTypewriter();
        }
    }
}