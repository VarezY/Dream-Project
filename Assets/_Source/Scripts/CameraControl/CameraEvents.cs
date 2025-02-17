using System;
using Unity.Cinemachine;

namespace Varez.CameraControl
{
    public struct CameraEvents
    {
        public Action OnTurnRight;
        public Action OnTurnLeft;

        public Action<CinemachineCamera, DG.Tweening.Ease> OnChangeCamera;
        
        public Action<float> OnZoom; //Zoom Level
        
        /// <summary>
        /// (Intensity Shake, Duration)
        /// </summary>
        public Action<float, float> OnShake;   
    }
}