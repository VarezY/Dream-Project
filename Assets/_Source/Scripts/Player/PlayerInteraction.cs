using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Varez.Interact;

namespace Varez.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactionRadius = 2f;
        [SerializeField] private float interactionAngle = 100f;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private RenderingLayerMask renderingLayer;
        // [SerializeField] private KeyCode interactionKey = KeyCode.E;
        
        private SphereCollider _interactCollider;
        private List<InteractableObject> _nearbyInteractables = new();
        private InteractableObject _currentBestTarget;

        private void Awake()
        {
            _interactCollider = GetComponent<SphereCollider>();
            _interactCollider.isTrigger = true;
            _interactCollider.radius = interactionRadius;
        }

        private void Update()
        {
            UpdateBestTarget();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("InteractableItem"))
                return;
            if (!other.TryGetComponent<InteractableObject>(out InteractableObject interactable)) 
                return;
            if (_nearbyInteractables.Contains(interactable)) 
                return; 

            _nearbyInteractables.Add(interactable);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out InteractableObject interactable))
            {
                _nearbyInteractables.Remove(interactable);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!_currentBestTarget)
                    return;

                _currentBestTarget.OnInteract();    
            }
        }
        
        public void OnInteract()
        {
            if (!_currentBestTarget)
                return;

            _currentBestTarget.OnInteract();
        }
        
        private void UpdateBestTarget()
        {
            _nearbyInteractables.RemoveAll(item => !item);
            _currentBestTarget = null;
            float highestPriority = float.MinValue;
            float closestDistance = float.MaxValue;

            foreach (InteractableObject interactable in _nearbyInteractables)
            {
                interactable.ToggleOutline(false);
                if (!interactable.CanInteract()) continue;
                if (!IsInFront(interactable.GetTransform())) continue;
                
                float distance = Vector3.Distance(transform.position, interactable.GetTransform().position);
                float priority = interactable.GetPriority();

                // Prioritize by both priority value and distance
                // ReSharper disable once InvertIf
                if (priority > highestPriority || 
                    (Mathf.Approximately(priority, highestPriority) && distance < closestDistance))
                {
                    highestPriority = priority;
                    closestDistance = distance;
                    _currentBestTarget = interactable;
                }
            }

            if (_currentBestTarget)
            {
                GameManager.Instance.UIEvents.OnAbleInteract.Invoke(true);
                _currentBestTarget.ToggleOutline(true);
            }
            else
            {
                GameManager.Instance.UIEvents.OnAbleInteract.Invoke(false);
            }
        }

        private bool IsInFront(Transform objectTransform)
        {
            Vector3 directionToObject = (objectTransform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToObject);
            return angle <= interactionAngle * 0.5f;
        }
    }
}