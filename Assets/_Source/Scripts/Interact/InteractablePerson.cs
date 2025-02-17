using UnityEngine;

namespace Varez.Interact
{
    public class InteractablePerson : InteractableObject
    {
        [SerializeField] private bool isOneTime;
        public override void OnInteract()
        {
            base.OnInteract();

            if (isOneTime) isInteractable = false;
        }
    }
}