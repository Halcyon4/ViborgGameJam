///-----------------------------------------------------------------
///   Namespace:      Spacehop.Gameplay.PlayerSystem.Core
///   Class:          CameraSwayComponent
///   Description:    Adds sway (tilt) to the camera when moving left and right
///   Author:         Liam Kerrigan
///   Created:        22/07/2019
///-----------------------------------------------------------------
using UnityEngine;
using Sirenix.OdinInspector;

namespace Spacehop.Gameplay.PlayerSystem.Core
{
    [System.Serializable]
    public class CameraSwayComponent
    {
        [BoxGroup("Sway Settings")] public float swayAmount;                                       // Anount to sway the camera
        [BoxGroup("Sway Settings")] public float swaySpeed;                                        // Sway speed
        [BoxGroup("Sway Settings")] public float returnSpeed;                                      // Return to 0 speed
        [BoxGroup("Sway Settings")] public float changeDirectionMultiplier;                        // Direction 1 : 0 change 
        [BoxGroup("Sway Settings")] public AnimationCurve swayCurve;                               // Sample curve

        [Header("Private Variables")]
        private Transform m_camTransform;
        private float m_scrollSpeed;
        private float m_xAmountThisFrame;
        private float m_xAmountPreviousFrame;
        private bool m_diffrentDirection;



        /// <summary>
        /// Initialize the components
        /// </summary>
        public void Initialize(Transform _cam)
        {
            m_camTransform = _cam;
        }

        /// <summary>
        /// Sways the camera transform based on input vector
        /// </summary>
        public void SwayCamera(Vector3 _inputVector, float _rawXInput)
        {
            float _xAmount = _inputVector.x;
            m_xAmountThisFrame = _rawXInput;

            if (_rawXInput != 0f) // if we have some input
            {
                // if our previous dir is not equal to current one and the previous one was not idle
                if (m_xAmountThisFrame != m_xAmountPreviousFrame && m_xAmountPreviousFrame != 0) m_diffrentDirection = true;

                // then we multiplier our scroll so when changing direction it will sway to the other direction faster
                float _speedMultiplier = m_diffrentDirection ? changeDirectionMultiplier : 1f;
                m_scrollSpeed += (_xAmount * swaySpeed * Time.deltaTime * _speedMultiplier);
            }
            else // if we are not moving so there is no input
            {
                // check if our previous dir equals current dir and reset the bool is needed
                if (m_xAmountThisFrame == m_xAmountPreviousFrame) m_diffrentDirection = false;
                m_scrollSpeed = Mathf.Lerp(m_scrollSpeed, 0f, Time.deltaTime * returnSpeed);
            }

            m_scrollSpeed = Mathf.Clamp(m_scrollSpeed, -1f, 1f);

            float _swayFinalAmount;
            if (m_scrollSpeed < 0f) _swayFinalAmount = -swayCurve.Evaluate(m_scrollSpeed) * -swayAmount;
            else _swayFinalAmount = swayCurve.Evaluate(m_scrollSpeed) * -swayAmount;

            m_camTransform.localEulerAngles = new Vector3(m_camTransform.localPosition.x, m_camTransform.localPosition.y, _swayFinalAmount);
            m_xAmountPreviousFrame = m_xAmountThisFrame;
        }
    }
}