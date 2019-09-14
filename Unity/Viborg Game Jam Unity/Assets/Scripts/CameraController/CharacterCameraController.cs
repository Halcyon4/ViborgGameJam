///-----------------------------------------------------------------
///   Namespace:      Spacehop.Gameplay.PlayerSystem.Core
///   Class:          CharacterCameraController
///   Description:    Primary camera conttroller
///   Author:         Liam Kerrigan
///   Created:        20/07/2019
///-----------------------------------------------------------------
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spacehop.Gameplay.PlayerSystem.Core
{
    public class CharacterCameraController : MonoBehaviour
    {
        [BoxGroup("Component References")] public Transform pitchTranform;                             // Reference to the pitch
        [BoxGroup("Component References")] public Camera cameraComponent;                              // Reference to the camera
        //BoxGroup("Component References")] public CharacterMovementController2 playerCharacterController; // Reference to player controller

        [BoxGroup("Camera Settings")] public Vector2 smoothAmount;                                     // Amount to smooth the camera
        [BoxGroup("Camera Settings")] public Vector2 lookAngleMinMax;                                  // Amount to min/max the view

        [BoxGroup("Camera Components")] public CameraSwayComponent cameraSway;                         // Camera sway component
        [BoxGroup("Camera Components")] public CameraBreathingComponent cameraBreathing;               // Camera breathing component
        [BoxGroup("Camera Components")] public CameraZoomComponent cameraZoom;                         // Camera zooming component
        [BoxGroup("Camera Components")] public CameraHeadbobComponent cameraHeadbob;                   // Camera bobbing component
        [BoxGroup("Camera Components")] public CameraFallComponent cameraFall;                         // Camera falling component
        [BoxGroup("Camera Components")] public CameraCrouchComponent cameraCrouch;                     // Camera crouching

        [Header("Private Variables")]
        private Quaternion m_originalRotation;                                                         // Default rotation in Quaternions
        private float m_desiredYaw;                                                                    // Input Yaw
        private float m_desiredPitch;                                                                  // Input Pitch
        private float m_yaw;                                                                           // Smooth Yaw
        private float m_pitch;                                                                         // Smooth Pitch



        /// <summary>
        /// Start is called on the first frame
        /// </summary>
        /// 
        private void Update()
        {
            CalculateRotation(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        }
        private void Start()
        {
            InitComponents();
            //            GPUInstancer.GPUInstancerAPI.SetCamera(cameraComponent);
            //VegetationStudioManager.AddCamera(cameraComponent);
        }

        /// <summary>
        /// Initialize data
        /// </summary>
        private void InitComponents()
        {
            m_originalRotation = pitchTranform.localRotation;
            cameraZoom.Initialize(cameraComponent);
            cameraHeadbob.Initialize(transform.localPosition.y);
            cameraSway.Initialize(cameraComponent.transform);
            cameraBreathing.Initialize(cameraComponent.transform);
            cameraFall.Initialize(transform);
            cameraCrouch.Initialize(transform);
        }

        /// <summary>
        /// LateUpdate is called after update
        /// </summary>
        private void LateUpdate()
        {
            SmoothRotation();
            ApplyRotation();
            HandleCameraBreathing();
        }

        /// <summary>
        /// Calculate the Rotation for the Camera
        /// </summary>
        public void CalculateRotation(float inputX, float inputY)
        {
            m_desiredYaw += inputX;
            m_desiredPitch -= inputY;

            m_desiredPitch = Mathf.Clamp(m_desiredPitch, lookAngleMinMax.x, lookAngleMinMax.y);
        }

        /// <summary>
        /// Smooth the Rotation
        /// </summary>
        private void SmoothRotation()
        {
            m_yaw = Mathf.Lerp(m_yaw, m_desiredYaw, smoothAmount.x * Time.deltaTime);
            m_pitch = Mathf.Lerp(m_pitch, m_desiredPitch, smoothAmount.y * Time.deltaTime);
        }

        /// <summary>
        /// Apply the Rotation to the pitch and yaw
        /// </summary>
        private void ApplyRotation()
        {
            transform.eulerAngles = new Vector3(0f, m_yaw, 0f);
            pitchTranform.localEulerAngles = new Vector3(m_pitch, 0f, 0f);
        }

        /// <summary>
        /// Adds the breathing effect to the camera transofrm
        /// </summary>
        private void HandleCameraBreathing()
        {
            cameraBreathing.BreathCamera();
        }

        /// <summary>
        /// Handles the camera sway based on the input vector
        /// </summary>
        public void HandleSway(Vector3 _inputVector, float _rawXInput)
        {
            cameraSway.SwayCamera(_inputVector, _rawXInput);
        }

        /// <summary>
        /// Handles the sprtint camera compoenent
        /// </summary>
        public void HandleSprintFOV(bool _returning)
        {
            cameraZoom.ChangeSprintFOV(_returning, this);
        }

        /// <summary>
        /// Handles camera fall (simulate bending knees)
        /// </summary>
        public void HandleCameraFall(float _airTime)
        {
            cameraFall.InvokeLandingRoutine(_airTime, this);
        }

        /// <summary>
        /// Handles the headbob camera component
        /// </summary>
        public void HandleHeadBob(bool _running, bool _crouching, Vector2 _input)
        {
            cameraHeadbob.ScrollHeadBob(_running, _crouching, _input);
            transform.localPosition = Vector3.Lerp(transform.localPosition, (Vector3.up * cameraHeadbob.currentStateHeight) + cameraHeadbob.finalOffset, Time.deltaTime * 5f);
        }

        /// <summary>
        /// Reset the headbob
        /// </summary>
        public void ResetHeadBob()
        {
            cameraHeadbob.ResetHeadBob();
        }

        /// <summary>
        /// Resets the position of the camera controller transform
        /// </summary>
        public void ResetHeadPosition()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, cameraHeadbob.currentStateHeight, 0f), Time.deltaTime * 5f);
        }
    }
}