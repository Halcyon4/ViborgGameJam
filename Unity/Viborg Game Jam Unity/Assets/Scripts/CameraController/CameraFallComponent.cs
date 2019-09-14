///-----------------------------------------------------------------
///   Namespace:      Spacehop.Gameplay.PlayerSystem.Core
///   Class:          CameraFallComponent
///   Description:    Create a procedural 
///   Author:         Liam Kerrigan
///   Created:        22/07/2019
///-----------------------------------------------------------------
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Spacehop.Gameplay.PlayerSystem.Core
{
    [System.Serializable]
    public class CameraFallComponent
    {
        [BoxGroup("Landing Properties")] public float highLandTimer = 0.5f;                          // The minimum airTime required to trigger the highLandAmount jump
        [BoxGroup("Landing Properties")] public float landDuration = 1f;                             // Duration of the return to 0 (speed of transition)
        [BoxGroup("Landing Properties")] [Range(0.05f, 0.5f)] public float lowLandAmount = 0.1f;     // lowJump effect
        [BoxGroup("Landing Properties")] [Range(0.2f, 0.9f)] public float highLandAmount = 0.6f;     // highJump effect
        [BoxGroup("Landing Properties")] public AnimationCurve landCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Private Variables")]
        private float m_inAirTimer;
        private Transform m_cameraTransform;
        private IEnumerator m_LandRoutine;



        /// <summary>
        /// Initialize this component
        /// </summary>
        public void Initialize(Transform _cam)
        {
            m_cameraTransform = _cam;
        }

        /// <summary>
        /// Invokes the landing routine
        /// </summary>
        public void InvokeLandingRoutine(float _airTime, MonoBehaviour _mono)
        {
            m_inAirTimer = _airTime;
            if (m_LandRoutine != null) _mono.StopCoroutine(m_LandRoutine);

            m_LandRoutine = LandingRoutine();
            _mono.StartCoroutine(m_LandRoutine);
        }

        /// <summary>
        /// Creates a fake animation for landing (bending the knees)
        /// </summary>
        private IEnumerator LandingRoutine()
        {
            float _percent = 0f;
            float _landAmount = 0f;
            float _speed = 1f / landDuration;

            Vector3 _localPos = m_cameraTransform.localPosition;
            float _initLandHeight = _localPos.y;
            _landAmount = m_inAirTimer > highLandTimer ? highLandAmount : lowLandAmount;

            while (_percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                float _desiredY = landCurve.Evaluate(_percent) * _landAmount;

                _localPos.y = _initLandHeight + _desiredY;
                m_cameraTransform.localPosition = _localPos;

                yield return null;
            }
        }
    }
}