using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PacMan.GameSystem;

namespace PacMan.Player
{
    /// <summary>
    /// Teleportation controller for VR movement
    /// Allows players to move around the maze using teleportation to avoid motion sickness
    /// </summary>
    public class TeleportationController : MonoBehaviour
    {
        [Header("Teleportation Settings")]
        [SerializeField] private XRController teleportController;
        [SerializeField] private XRRayInteractor rayInteractor;
        [SerializeField] private LayerMask teleportLayerMask = 1 << 3; // Ignore Raycast layer by default
        [SerializeField] private float maxTeleportDistance = 10f;
        [SerializeField] private float teleportFadeDuration = 0.2f;
        
        [Header("Visual Feedback")]
        [SerializeField] private Material validTeleportMaterial;
        [SerializeField] private Material invalidTeleportMaterial;
        [SerializeField] private GameObject teleportIndicator;
        
        private bool _isTeleporting = false;
        private Vector3 _teleportPosition;
        private Quaternion _teleportRotation;
        private CharacterController _characterController;
        private InputManager _inputManager;
        private bool _teleportButtonPressed = false;
        
        // Events
        public System.Action OnTeleportStarted;
        public System.Action OnTeleportEnded;
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            
            // Find InputManager in the scene
            _inputManager = FindObjectOfType<InputManager>();
            if (_inputManager != null)
            {
                // Subscribe to input events
                _inputManager.OnRightJoystickMoved += HandleTeleportInput;
            }
            
            if (rayInteractor != null)
            {
                rayInteractor.selectEntered.AddListener(OnRaySelectEntered);
            }
            
            if (teleportIndicator != null)
            {
                teleportIndicator.SetActive(false);
            }
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from input events
            if (_inputManager != null)
            {
                _inputManager.OnRightJoystickMoved -= HandleTeleportInput;
            }
        }
        
        private void Update()
        {
            if (teleportController != null && rayInteractor != null && _teleportButtonPressed)
            {
                UpdateTeleportIndicator();
            }
            else
            {
                HideTeleportIndicator();
            }
        }
        
        /// <summary>
        /// Handle teleport input from VR controllers
        /// </summary>
        /// <param name="joystickInput">Joystick input vector</param>
        private void HandleTeleportInput(Vector2 joystickInput)
        {
            // Check if joystick is pressed down (magnitude close to 1)
            _teleportButtonPressed = joystickInput.magnitude > 0.9f;
        }
        
        /// <summary>
        /// Update the teleport indicator based on raycast results
        /// </summary>
        private void UpdateTeleportIndicator()
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                // Check if the hit point is within valid teleport distance
                float distance = Vector3.Distance(transform.position, hit.point);
                
                if (distance <= maxTeleportDistance && IsTeleportPositionValid(hit.point))
                {
                    ShowTeleportIndicator(hit.point, hit.normal, true);
                }
                else
                {
                    ShowTeleportIndicator(hit.point, hit.normal, false);
                }
            }
            else
            {
                HideTeleportIndicator();
            }
        }
        
        /// <summary>
        /// Show the teleport indicator at the target position
        /// </summary>
        /// <param name="position">Target position</param>
        /// <param name="normal">Surface normal</param>
        /// <param name="isValid">Whether the position is valid for teleportation</param>
        private void ShowTeleportIndicator(Vector3 position, Vector3 normal, bool isValid)
        {
            if (teleportIndicator != null)
            {
                teleportIndicator.SetActive(true);
                teleportIndicator.transform.position = position;
                teleportIndicator.transform.up = normal;
                
                // Update material based on validity
                Renderer renderer = teleportIndicator.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = isValid ? validTeleportMaterial : invalidTeleportMaterial;
                }
            }
        }
        
        /// <summary>
        /// Hide the teleport indicator
        /// </summary>
        private void HideTeleportIndicator()
        {
            if (teleportIndicator != null)
            {
                teleportIndicator.SetActive(false);
            }
        }
        
        /// <summary>
        /// Check if a teleport position is valid
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns>True if position is valid</returns>
        private bool IsTeleportPositionValid(Vector3 position)
        {
            // Check if position is on a valid surface (not too steep)
            // This would typically involve checking the surface normal angle
            
            // For now, we'll just check if it's not inside a wall
            Collider[] colliders = Physics.OverlapSphere(position, 0.1f);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.CompareTag("Wall"))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Handle ray selection
        /// </summary>
        /// <param name="args">Selection event args</param>
        private void OnRaySelectEntered(SelectEnterEventArgs args)
        {
            if (_teleportButtonPressed)
            {
                if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {
                    float distance = Vector3.Distance(transform.position, hit.point);
                    
                    if (distance <= maxTeleportDistance && IsTeleportPositionValid(hit.point))
                    {
                        InitiateTeleport(hit.point);
                    }
                }
            }
        }
        
        /// <summary>
        /// Initiate the teleportation process
        /// </summary>
        /// <param name="targetPosition">Target position</param>
        private void InitiateTeleport(Vector3 targetPosition)
        {
            if (_isTeleporting) return;
            
            _isTeleporting = true;
            _teleportPosition = targetPosition;
            
            OnTeleportStarted?.Invoke();
            
            // In a full implementation, we would:
            // 1. Fade out the screen
            // 2. Move the player
            // 3. Fade in the screen
            
            // For now, we'll just move the player directly
            PerformTeleport();
        }
        
        /// <summary>
        /// Perform the actual teleportation
        /// </summary>
        private void PerformTeleport()
        {
            // Move the player to the target position
            if (_characterController != null)
            {
                // Move to the target position (maintaining Y position to stay on ground)
                Vector3 newPosition = new Vector3(_teleportPosition.x, transform.position.y, _teleportPosition.z);
                _characterController.enabled = false;
                transform.position = newPosition;
                _characterController.enabled = true;
            }
            else
            {
                // Move to the target position (maintaining Y position to stay on ground)
                Vector3 newPosition = new Vector3(_teleportPosition.x, transform.position.y, _teleportPosition.z);
                transform.position = newPosition;
            }
            
            _isTeleporting = false;
            HideTeleportIndicator();
            
            OnTeleportEnded?.Invoke();
            
            Debug.Log($"Teleported to {_teleportPosition}");
        }
        
        /// <summary>
        /// Cancel teleportation
        /// </summary>
        public void CancelTeleportation()
        {
            _isTeleporting = false;
            HideTeleportIndicator();
        }
        
        /// <summary>
        /// Set the maximum teleport distance
        /// </summary>
        /// <param name="distance">Maximum distance</param>
        public void SetMaxTeleportDistance(float distance)
        {
            maxTeleportDistance = distance;
        }
        
        /// <summary>
        /// Get the maximum teleport distance
        /// </summary>
        /// <returns>Maximum teleport distance</returns>
        public float GetMaxTeleportDistance()
        {
            return maxTeleportDistance;
        }
    }
}