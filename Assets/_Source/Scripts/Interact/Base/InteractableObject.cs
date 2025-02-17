using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Varez.Interact
{
    [RequireComponent(typeof(Rigidbody))]
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactionPrompt = "Press E to interact";
        [SerializeField] private string playerAnimationTrigger = "Interact";
        [SerializeField] protected bool isInteractable = true;
        [SerializeField] private float interactionPriority = 0f;
        [SerializeField] private RenderingLayerMask outlineLayer;

        [Header("Base Audio Settings")] 
        [SerializeField] protected float volumeScale = 1f;
        [SerializeField] protected bool usingSound = true;
        
        // Unity Event that will be triggered on interaction
        [Space]
        public UnityEvent onInteract;
        
        protected AudioSource AudioSourceItem;
        private AudioClip _interactSound;
        private SpriteRenderer _indicator;
        private MeshRenderer _mesh;
        private SkinnedMeshRenderer _skin;

        protected virtual void Awake()
        { 
            AudioSourceItem = GetComponent<AudioSource>();
            _indicator = GetComponentInChildren<SpriteRenderer>();

            if (!TryGetComponent(out _skin))
            {
                _skin = GetComponentInChildren<SkinnedMeshRenderer>();
            }
            
            if (!TryGetComponent(out _mesh))
            {
                _mesh = GetComponentInChildren<MeshRenderer>();
            }
        }

        protected virtual void Start()
        {
            _interactSound = GameManager.Instance.AudioManager.defaultInteractSound;
            
            // ReSharper disable once InvertIf
            if (!AudioSourceItem && usingSound)
            {
                AudioSourceItem = gameObject.AddComponent<AudioSource>();
                AudioSourceItem.playOnAwake = false;
                AudioSourceItem.spatialBlend = 1f; // 3D sound
            }
        }

        public virtual void OnInteract()
        {
            PlayInteractionSound();
            onInteract?.Invoke();
        }

        public void ToggleOutline(bool isOn)
        {
            if (isOn)
            {
                // _meshRenderer.renderingLayerMask |= (uint)outlineLayer;
                if (_mesh) _mesh.gameObject.layer = LayerMask.NameToLayer("Outlined");
                if (_skin) _skin.gameObject.layer = LayerMask.NameToLayer("Outlined");
                
                if (_indicator)
                    _indicator.enabled = true;
            }
            else
            {
                // _meshRenderer.renderingLayerMask &= ~(uint)outlineLayer;
                if (_mesh) _mesh.gameObject.layer = LayerMask.NameToLayer("Default");
                if (_skin) _skin.gameObject.layer = LayerMask.NameToLayer("Default");
                
                if (_indicator)
                    _indicator.enabled = false;
            }
        }

        public virtual void PlayInteractionSound()
        {
            if (!usingSound)
                return;
            
            AudioSourceItem.PlayOneShot(_interactSound, volumeScale);
        }

        public string GetInteractionPrompt() 
        {
            return interactionPrompt;
        }

        public string PlayerAnimationTrigger()
        {
            return playerAnimationTrigger;
        }

        public float GetPriority()
        {
            return interactionPriority;
        }

        public Transform GetTransform()
        {
            return this.transform;
        }

        public bool CanInteract()
        {
            return isInteractable;
        }
    }
}