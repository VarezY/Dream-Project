using UnityEngine;
using UnityEngine.InputSystem;

namespace Varez.Player
{
    public class PlayerDialogue : MonoBehaviour
    {
        public void OnSkipDialogue(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                
            }
            else if (context.performed)
            {
                Debug.Log("Next Dialogue");
                GameManager.Instance.UIEvents.OnNextDialogue?.Invoke();
            }   
            else if (context.canceled)
            {
                
            }
            
        }
        
        public void OnSkipDialogue(InputValue value)
        {
            Debug.Log("Next Dialogue");
            GameManager.Instance.UIEvents.OnNextDialogue?.Invoke();
        }    
    }
}