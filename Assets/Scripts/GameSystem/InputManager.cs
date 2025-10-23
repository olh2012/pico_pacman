using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Manages input from VR controllers
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [Header("Controller References")]
        [SerializeField] private XRController leftController;
        [SerializeField] private XRController rightController;
        
        [Header("Input Settings")]
        [SerializeField] private float triggerThreshold = 0.5f;
        [SerializeField] private float gripThreshold = 0.5f;
        [SerializeField] private float joystickThreshold = 0.2f;
        
        // Events
        public System.Action<bool> OnLeftTriggerPressed;
        public System.Action<bool> OnRightTriggerPressed;
        public System.Action<bool> OnLeftGripPressed;
        public System.Action<bool> OnRightGripPressed;
        public System.Action<Vector2> OnLeftJoystickMoved;
        public System.Action<Vector2> OnRightJoystickMoved;
        public System.Action OnLeftPrimaryButtonPressed;
        public System.Action OnRightPrimaryButtonPressed;
        public System.Action OnLeftSecondaryButtonPressed;
        public System.Action OnRightSecondaryButtonPressed;
        
        private bool _leftTriggerPressed = false;
        private bool _rightTriggerPressed = false;
        private bool _leftGripPressed = false;
        private bool _rightGripPressed = false;
        
        private void Update()
        {
            HandleLeftControllerInput();
            HandleRightControllerInput();
        }
        
        /// <summary>
        /// Handle input from the left controller
        /// </summary>
        private void HandleLeftControllerInput()
        {
            if (leftController == null) return;
            
            // Trigger input
            float leftTriggerValue = 0f;
            if (leftController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out leftTriggerValue))
            {
                bool isPressed = leftTriggerValue > triggerThreshold;
                if (isPressed != _leftTriggerPressed)
                {
                    _leftTriggerPressed = isPressed;
                    OnLeftTriggerPressed?.Invoke(isPressed);
                }
            }
            
            // Grip input
            float leftGripValue = 0f;
            if (leftController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out leftGripValue))
            {
                bool isPressed = leftGripValue > gripThreshold;
                if (isPressed != _leftGripPressed)
                {
                    _leftGripPressed = isPressed;
                    OnLeftGripPressed?.Invoke(isPressed);
                }
            }
            
            // Joystick input
            Vector2 leftJoystickValue = Vector2.zero;
            if (leftController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out leftJoystickValue))
            {
                if (leftJoystickValue.magnitude > joystickThreshold)
                {
                    OnLeftJoystickMoved?.Invoke(leftJoystickValue);
                }
            }
            
            // Primary button (A/X button)
            bool leftPrimaryButtonPressed = false;
            if (leftController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out leftPrimaryButtonPressed))
            {
                if (leftPrimaryButtonPressed)
                {
                    OnLeftPrimaryButtonPressed?.Invoke();
                }
            }
            
            // Secondary button (B/Y button)
            bool leftSecondaryButtonPressed = false;
            if (leftController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out leftSecondaryButtonPressed))
            {
                if (leftSecondaryButtonPressed)
                {
                    OnLeftSecondaryButtonPressed?.Invoke();
                }
            }
        }
        
        /// <summary>
        /// Handle input from the right controller
        /// </summary>
        private void HandleRightControllerInput()
        {
            if (rightController == null) return;
            
            // Trigger input
            float rightTriggerValue = 0f;
            if (rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out rightTriggerValue))
            {
                bool isPressed = rightTriggerValue > triggerThreshold;
                if (isPressed != _rightTriggerPressed)
                {
                    _rightTriggerPressed = isPressed;
                    OnRightTriggerPressed?.Invoke(isPressed);
                }
            }
            
            // Grip input
            float rightGripValue = 0f;
            if (rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out rightGripValue))
            {
                bool isPressed = rightGripValue > gripThreshold;
                if (isPressed != _rightGripPressed)
                {
                    _rightGripPressed = isPressed;
                    OnRightGripPressed?.Invoke(isPressed);
                }
            }
            
            // Joystick input
            Vector2 rightJoystickValue = Vector2.zero;
            if (rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rightJoystickValue))
            {
                if (rightJoystickValue.magnitude > joystickThreshold)
                {
                    OnRightJoystickMoved?.Invoke(rightJoystickValue);
                }
            }
            
            // Primary button (A/X button)
            bool rightPrimaryButtonPressed = false;
            if (rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out rightPrimaryButtonPressed))
            {
                if (rightPrimaryButtonPressed)
                {
                    OnRightPrimaryButtonPressed?.Invoke();
                }
            }
            
            // Secondary button (B/Y button)
            bool rightSecondaryButtonPressed = false;
            if (rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out rightSecondaryButtonPressed))
            {
                if (rightSecondaryButtonPressed)
                {
                    OnRightSecondaryButtonPressed?.Invoke();
                }
            }
        }
        
        /// <summary>
        /// Vibrate the left controller
        /// </summary>
        /// <param name="amplitude">Vibration amplitude (0.0 to 1.0)</param>
        /// <param name="duration">Vibration duration in seconds</param>
        public void VibrateLeftController(float amplitude, float duration)
        {
            if (leftController != null)
            {
                leftController.SendHapticImpulse(0, amplitude, duration);
            }
        }
        
        /// <summary>
        /// Vibrate the right controller
        /// </summary>
        /// <param name="amplitude">Vibration amplitude (0.0 to 1.0)</param>
        /// <param name="duration">Vibration duration in seconds</param>
        public void VibrateRightController(float amplitude, float duration)
        {
            if (rightController != null)
            {
                rightController.SendHapticImpulse(0, amplitude, duration);
            }
        }
        
        /// <summary>
        /// Vibrate both controllers
        /// </summary>
        /// <param name="amplitude">Vibration amplitude (0.0 to 1.0)</param>
        /// <param name="duration">Vibration duration in seconds</param>
        public void VibrateBothControllers(float amplitude, float duration)
        {
            VibrateLeftController(amplitude, duration);
            VibrateRightController(amplitude, duration);
        }
        
        /// <summary>
        /// Set the trigger threshold
        /// </summary>
        /// <param name="threshold">Trigger threshold (0.0 to 1.0)</param>
        public void SetTriggerThreshold(float threshold)
        {
            triggerThreshold = Mathf.Clamp01(threshold);
        }
        
        /// <summary>
        /// Set the grip threshold
        /// </summary>
        /// <param name="threshold">Grip threshold (0.0 to 1.0)</param>
        public void SetGripThreshold(float threshold)
        {
            gripThreshold = Mathf.Clamp01(threshold);
        }
        
        /// <summary>
        /// Set the joystick threshold
        /// </summary>
        /// <param name="threshold">Joystick threshold (0.0 to 1.0)</param>
        public void SetJoystickThreshold(float threshold)
        {
            joystickThreshold = Mathf.Clamp01(threshold);
        }
    }
}