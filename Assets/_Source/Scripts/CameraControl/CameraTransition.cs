using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace Varez.CameraControl
{
    public class CameraTransition : MonoBehaviour
    {
        private CinemachineBrain _cinemachineBrain;
        private CinemachineCamera _virtualCamera;

        private void Start()
        {
            GameManager.Instance.CameraEvents.OnChangeCamera += TransitionToCamera;
        }

        public void TransitionToCamera(CinemachineCamera virtualCamera, Ease ease)
        {
            _virtualCamera = virtualCamera;
            // _cinemachineBrain.CustomBlends.CustomBlends.ad
        }
    }
}