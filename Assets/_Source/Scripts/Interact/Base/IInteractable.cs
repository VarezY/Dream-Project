using UnityEngine;

namespace Varez.Interact
{
    public interface IInteractable
    {
        void OnInteract();
        void ToggleOutline(bool isOn);
        void PlayInteractionSound();
        string GetInteractionPrompt();
        string PlayerAnimationTrigger();
        float GetPriority();
        Transform GetTransform();
        bool CanInteract();
    }
}