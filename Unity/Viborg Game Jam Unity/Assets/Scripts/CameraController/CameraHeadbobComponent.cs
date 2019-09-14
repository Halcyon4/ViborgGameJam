///-----------------------------------------------------------------
///   Namespace:      Spacehop.Gameplay.PlayerSystem.Core
///   Class:          CameraHeadbobComponent
///   Description:    Creates a procedural headbob effect
///   Author:         Liam Kerrigan
///   Created:        22/07/2019
///-----------------------------------------------------------------
using UnityEngine;
using Sirenix.OdinInspector;

namespace Spacehop.Gameplay.PlayerSystem.Core
{
    [System.Serializable]
    public class CameraHeadbobComponent
    {
        [BoxGroup("Curves")] public AnimationCurve xCurve;
        [BoxGroup("Curves")] public AnimationCurve yCurve;

        [BoxGroup("HeadBob Properties")] public float xAmplitude;                                  // Amplitude
        [BoxGroup("HeadBob Properties")] public float yAmplitude;                                  // Amplitude
        [BoxGroup("HeadBob Properties")] public float xFrequency;                                  // Frequency
        [BoxGroup("HeadBob Properties")] public float yFrequency;                                  // Frequency

        [BoxGroup("Multipliers")] public float moveBackwardsFrequencyMultiplier;                   // Multiplier
        [BoxGroup("Multipliers")] public float moveSideFrequencyMultiplier;                        // Multiplier
        [BoxGroup("Multipliers")] public float runAmplitudeMultiplier;                             // Multiplier
        [BoxGroup("Multipliers")] public float runFrequencyMultiplier;                             // Multiplier
        [BoxGroup("Multipliers")] public float crouchAmplitudeMultiplier;                          // Multiplier
        [BoxGroup("Multipliers")] public float crouchFrequencyMultiplier;                          // Multiplier

        [Header("Private Variables")]
        private float m_xScroll;
        private float m_yScroll;
        [HideInInspector] public float currentStateHeight = 0f;
        [HideInInspector] public Vector3 finalOffset;
        [HideInInspector] public bool resetted;



        /// <summary>
        /// Initialize this component
        /// </summary>
        public void Initialize(float _initCamHeight)
        {
            currentStateHeight = _initCamHeight;

            m_xScroll = 0f;
            m_yScroll = 0f;
            resetted = false;
            finalOffset = Vector3.zero;
        }

        /// <summary>
        /// Scroll the headbob effect
        /// </summary>
        public void ScrollHeadBob(bool _running, bool _crouching, Vector2 _input)
        {
            resetted = false;

            float _amplitudeMultiplier;
            float _frequencyMultiplier;
            float _additionalMultiplier; // when moving backwards or to sides

            _amplitudeMultiplier = _running ? runAmplitudeMultiplier : 1f;
            _amplitudeMultiplier = _crouching ? crouchAmplitudeMultiplier : _amplitudeMultiplier;

            _frequencyMultiplier = _running ? runFrequencyMultiplier : 1f;
            _frequencyMultiplier = _crouching ? crouchFrequencyMultiplier : _frequencyMultiplier;

            _additionalMultiplier = _input.y == -1 ? moveBackwardsFrequencyMultiplier : 1f;
            _additionalMultiplier = _input.x != 0 & _input.y == 0 ? moveSideFrequencyMultiplier : _additionalMultiplier;


            m_xScroll += Time.deltaTime * xFrequency * _frequencyMultiplier;
            m_yScroll += Time.deltaTime * yFrequency * _frequencyMultiplier;

            float _xValue;
            float _yValue;

            _xValue = xCurve.Evaluate(m_xScroll);
            _yValue = yCurve.Evaluate(m_yScroll);

            finalOffset.x = _xValue * xAmplitude * _amplitudeMultiplier * _additionalMultiplier;
            finalOffset.y = _yValue * yAmplitude * _amplitudeMultiplier * _additionalMultiplier;
        }

        /// <summary>
        /// Reset the headbob state
        /// </summary>
        public void ResetHeadBob()
        {
            resetted = true;
            m_xScroll = 0f;
            m_yScroll = 0f;
            finalOffset = Vector3.zero;
        }
    }
}
