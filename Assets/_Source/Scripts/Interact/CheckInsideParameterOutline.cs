using System;
using UnityEngine;

namespace Varez.Interact
{
    public class CheckInsideParameterOutline : MonoBehaviour
    {
        public RenderingLayerMask renderingMask;
        private MeshRenderer _meshRenderer;
        

        private void Awake()
        {
            _meshRenderer = GetComponentInParent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            _meshRenderer.renderingLayerMask |= (uint)renderingMask;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            _meshRenderer.renderingLayerMask &= ~(uint)renderingMask;
        }
    }
}