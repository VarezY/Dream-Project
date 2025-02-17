using System;
using UnityEngine;

namespace Varez.Interact
{
    public class InteractableBall : InteractableObject
    {
        public override void OnInteract()
        {
            base.OnInteract();
            Debug.Log($"Interact with ball {gameObject.name}");
            
            Destroy(this.gameObject);
        }
    }
}