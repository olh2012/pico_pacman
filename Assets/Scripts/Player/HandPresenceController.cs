using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PacMan.Player
{
    /// <summary>
    /// Controls hand presence visualization in VR
    /// Shows controller models when controllers are present, and hand models when using hand tracking
    /// </summary>
    public class HandPresenceController : MonoBehaviour
    {
        [Header("Hand Models")]
        [SerializeField] private GameObject leftHandModel;
        [SerializeField] private GameObject rightHandModel;
        
        [Header("Controller Models")]
        [SerializeField] private GameObject leftControllerModel;
        [SerializeField] private GameObject rightControllerModel;
        
        [Header("XR References")]
        [SerializeField] private XRController leftController;
        [SerializeField] private XRController rightController;
        
        private void Start()
        {
            // Initially hide all models
            SetHandVisibility(false);
            SetControllerVisibility(false);
            
            // Subscribe to controller events
            if (leftController != null)
            {
                leftController.onTrackingStateChanged.AddListener(OnLeftControllerTrackingChanged);
            }
            
            if (rightController != null)
            {
                rightController.onTrackingStateChanged.AddListener(OnRightControllerTrackingChanged);
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from controller events
            if (leftController != null)
            {
                leftController.onTrackingStateChanged.RemoveListener(OnLeftControllerTrackingChanged);
            }
            
            if (rightController != null)
            {
                rightController.onTrackingStateChanged.RemoveListener(OnRightControllerTrackingChanged);
            }
        }
        
        /// <summary>
        /// Handle left controller tracking state changes
        /// </summary>
        /// <param name="state">New tracking state</param>
        private void OnLeftControllerTrackingChanged(InputTrackingState state)
        {
            UpdateHandPresence();
        }
        
        /// <summary>
        /// Handle right controller tracking state changes
        /// </summary>
        /// <param name="state">New tracking state</param>
        private void OnRightControllerTrackingChanged(InputTrackingState state)
        {
            UpdateHandPresence();
        }
        
        /// <summary>
        /// Update hand presence based on controller availability
        /// </summary>
        private void UpdateHandPresence()
        {
            bool leftControllerActive = IsControllerActive(leftController);
            bool rightControllerActive = IsControllerActive(rightController);
            
            // Show controller models when controllers are active
            if (leftControllerModel != null)
            {
                leftControllerModel.SetActive(leftControllerActive);
            }
            
            if (rightControllerModel != null)
            {
                rightControllerModel.SetActive(rightControllerActive);
            }
            
            // Show hand models when controllers are not active
            if (leftHandModel != null)
            {
                leftHandModel.SetActive(!leftControllerActive);
            }
            
            if (rightHandModel != null)
            {
                rightHandModel.SetActive(!rightControllerActive);
            }
        }
        
        /// <summary>
        /// Check if a controller is active and tracked
        /// </summary>
        /// <param name="controller">Controller to check</param>
        /// <returns>True if controller is active</returns>
        private bool IsControllerActive(XRController controller)
        {
            if (controller == null) return false;
            
            // Check if controller is tracked
            return controller.IsTracked();
        }
        
        /// <summary>
        /// Set visibility of hand models
        /// </summary>
        /// <param name="visible">True to show hand models</param>
        public void SetHandVisibility(bool visible)
        {
            if (leftHandModel != null)
            {
                leftHandModel.SetActive(visible);
            }
            
            if (rightHandModel != null)
            {
                rightHandModel.SetActive(visible);
            }
        }
        
        /// <summary>
        /// Set visibility of controller models
        /// </summary>
        /// <param name="visible">True to show controller models</param>
        public void SetControllerVisibility(bool visible)
        {
            if (leftControllerModel != null)
            {
                leftControllerModel.SetActive(visible);
            }
            
            if (rightControllerModel != null)
            {
                rightControllerModel.SetActive(visible);
            }
        }
        
        /// <summary>
        /// Force show hand models (for hand tracking)
        /// </summary>
        public void ShowHands()
        {
            SetHandVisibility(true);
            SetControllerVisibility(false);
        }
        
        /// <summary>
        /// Force show controller models
        /// </summary>
        public void ShowControllers()
        {
            SetHandVisibility(false);
            SetControllerVisibility(true);
        }
    }
}