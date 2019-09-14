///-----------------------------------------------------------------
///   Namespace:      Spacehop.Gameplay.PlayerSystem.Core
///   Class:          CameraCrouchComponent
///   Description:    Creates a camera crouching transition using curves
///   Author:         Liam Kerrigan
///   Created:        23/07/2019
///-----------------------------------------------------------------
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Spacehop.Gameplay.PlayerSystem.Core
{
    [System.Serializable]
    public class CameraCrouchComponent
    {
        [BoxGroup("Crouch Properties")] public float crouchTransitionDuration = 1f;
        [BoxGroup("Crouch Properties")] public float standingHeight;
        [BoxGroup("Crouch Properties")] public float crouchingHeight;
        [BoxGroup("Crouch Properties")] public AnimationCurve crouchTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Private Variables")]
        private IEnumerator m_CrouchRoutine;
        private Transform m_cameraTransform;



        /// <summary>
        /// Initialize this component
        /// </summary>
        public void Initialize(Transform _cam)
        {
            m_cameraTransform = _cam;
        }

        /// <summary>
        /// Invokes the Crouch routine
        /// </summary>
        public void InvokeCrouchRoutine(bool _isCrouching, MonoBehaviour _mono)
        {
            if (m_CrouchRoutine != null) _mono.StopCoroutine(m_CrouchRoutine);

            m_CrouchRoutine = CrouchRoutine(_isCrouching);
            _mono.StartCoroutine(m_CrouchRoutine);
        }

        /// <summary>
        /// Start crouching
        /// </summary>
        private IEnumerator CrouchRoutine(bool _isCrouchaa)
        {
            float _percent = 0f;
            float _smoothPercent = 0f;
            float _speed = 1f / crouchTransitionDuration;

            Vector3 _camPos = m_cameraTransform.localPosition;
            float _camCurrentHeight = _camPos.y;
            float _camDesiredHeight = _isCrouchaa ? crouchingHeight : standingHeight;

            while (_percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                _smoothPercent = crouchTransitionCurve.Evaluate(_percent);
                _camPos.y = Mathf.Lerp(_camCurrentHeight, _camDesiredHeight, _smoothPercent);
                m_cameraTransform.localPosition = _camPos;

                yield return null;
            }
            yield return null;
        }
    }
}