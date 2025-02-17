using UnityEngine;

namespace Varez
{
    public class ExplodeBarrel : MonoBehaviour
    {
        [Range(0, 10)]
        [SerializeField] private float intensity = 1f;
        [ContextMenu("Explode")]
        public void Explode()
        {
            GameManager.Instance.CameraEvents.OnShake?.Invoke(intensity, 2f);
        }
    }
}