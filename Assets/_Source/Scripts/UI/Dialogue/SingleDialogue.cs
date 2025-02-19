using System;
using UnityEngine;

namespace Varez.UI.Dialogue
{
    public class SingleDialogue : MonoBehaviour
    {
        [SerializeField] private string titleDialogue;
        [SerializeField] private string textDialogue;
        
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameManager.Instance;
        }
        
        [ContextMenu("Show Dialogue")]
        public void StartDialogue()
        {
            // _gameManager.UIEvents.OnPlayDialogue?.Invoke(titleDialogue, textDialogue);
        }
    }
}