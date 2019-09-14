///-----------------------------------------------------------------
///   Namespace:      Spacehop.Gameplay.PlayerSystem.Core
///   Class:          CameraBreathingComponent
///   Description:    Adds breathing to the camera transform
///   Author:         Liam Kerrigan
///   Created:        22/07/2019
///-----------------------------------------------------------------
using UnityEngine;
using Sirenix.OdinInspector;

namespace Spacehop.Gameplay.PlayerSystem.Core
{
    [System.Serializable]
    public class CameraBreathingComponent
    {
        [BoxGroup("Breathing Properties")] public float frequency;                                   // Frequency of the breathing
        [BoxGroup("Breathing Properties")] public float amplitude;                                   // Amplitude of the breathing

        [Header("Private Variables")]
        private float m_offset;
        private Vector3 m_noise;
        private Vector3 m_finalRot;
        private Transform m_camTransform;



        /// <summary>
        /// Initialize this component
        /// </summary>
        public void Initialize(Transform m_cam)
        {
            m_camTransform = m_cam;
            m_offset = Random.Range(0f, 32f);
        }

        /// <summary>
        /// Updates the value of the PerlinNoise
        /// </summary>
        private void UpdateNoise()
        {
            float _scrollOffset = Time.deltaTime * frequency;

            m_offset += _scrollOffset;

            m_noise.x = Mathf.PerlinNoise(m_offset, 0f);
            m_noise.y = Mathf.PerlinNoise(m_offset, 1f);

            m_noise -= Vector3.one * 0.5f;
            m_noise *= amplitude;
        }

        /// <summary>
        /// Breaths the camera transform based on the perlin noise
        /// </summary>
        public void BreathCamera()
        {
            UpdateNoise();
            Vector3 _rotOffset = Vector3.zero;

            _rotOffset.x += m_noise.x;
            _rotOffset.y += m_noise.y;

            m_finalRot.x = _rotOffset.x;
            m_finalRot.y = _rotOffset.y;
            m_finalRot.z = m_camTransform.localEulerAngles.z;

            m_camTransform.localEulerAngles = m_finalRot;
        }
    }
}