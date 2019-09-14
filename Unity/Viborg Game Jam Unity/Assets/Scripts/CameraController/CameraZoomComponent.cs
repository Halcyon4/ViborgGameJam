///-----------------------------------------------------------------
///   Namespace:      Spacehop.Gameplay.PlayerSystem.Core
///   Class:          CameraZoomComponent
///   Description:    Creates a procedulra fov effect
///   Author:         Liam Kerrigan
///   Created:        22/07/2019
///-----------------------------------------------------------------
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Spacehop.Gameplay.PlayerSystem.Core
{
    [System.Serializable]
    public class CameraZoomComponent
    {
        [BoxGroup("Zoom Properties")] public float sprintingFOV;
        [BoxGroup("Zoom Properties")] public AnimationCurve sprintCurve;
        [BoxGroup("Zoom Properties")] public float sprintTransitionDuration;
        [BoxGroup("Zoom Properties")] public float sprintReturnTransitionDuration;

        [Header("Private Variables")]
        private float m_initFOV;
        private Camera m_cam;
        private IEnumerator m_ChangeRunFOVRoutine;



        /// <summary>
        /// Initialize this component
        /// </summary>
        public void Initialize(Camera _cam)
        {
            m_cam = _cam;
            m_initFOV = m_cam.fieldOfView;
        }

        /// <summary>
        /// Change tthe current FOV
        /// </summary>
        public void ChangeSprintFOV(bool _returning, MonoBehaviour _mono)
        {
            if (m_ChangeRunFOVRoutine != null) _mono.StopCoroutine(m_ChangeRunFOVRoutine);

            m_ChangeRunFOVRoutine = ChangeSprintFOVRoutine(_returning);
            _mono.StartCoroutine(m_ChangeRunFOVRoutine);
        }

        /// <summary>
        /// Internal Routine for smoothing FOV
        /// </summary>
        private IEnumerator ChangeSprintFOVRoutine(bool _returning)
        {
            float _percent = 0f;
            float _smoothPercent = 0f;

            float _duration = _returning ? sprintReturnTransitionDuration : sprintTransitionDuration;
            float _speed = 1f / _duration;

            float _currentFOV = m_cam.fieldOfView;
            float _targetFOV = _returning ? m_initFOV : sprintingFOV;

            while (_percent < 1f)
            {
                _percent += Time.deltaTime * _speed;
                _smoothPercent = sprintCurve.Evaluate(_percent);
                m_cam.fieldOfView = Mathf.Lerp(_currentFOV, _targetFOV, _smoothPercent);
                yield return null;
            }
        }
    }
}