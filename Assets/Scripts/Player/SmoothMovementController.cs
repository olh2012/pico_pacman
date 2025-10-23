using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PacMan.Player
{
    /// <summary>
    /// Smooth movement controller for VR with comfort settings
    /// Provides alternative to teleportation with anti-motion sickness features
    /// </summary>
    public class SmoothMovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float turnSpeed = 60f;
        [SerializeField] private bool useGravity = true;
        [SerializeField] private float gravity = -9.81f;
        
        [Header("Comfort Settings")]
        [SerializeField] private bool useTunnelVision = true;
        [SerializeField] private bool useSnapTurn = false;
        [SerializeField] private float snapTurnAngle = 45f;
        [SerializeField] private float smoothTurnSpeed = 60f;
        
        [Header("VR Interaction")]
        [SerializeField] private XRController leftController;
        [SerializeField] private XRController rightController;
        [SerializeField] private ContinuousMoveProviderBase continuousMoveProvider;
        [SerializeField] private ContinuousTurnProviderBase continuousTurnProvider;
        [SerializeField] private SnapTurnProviderBase snapTurnProvider;
        
        [Header("Tunnel Vision")]
        [SerializeField] private GameObject tunnelVisionEffect;
        [SerializeField] private float tunnelVisionIntensity = 0.5f;
        [SerializeField] private float tunnelVisionFadeSpeed = 5f;
        
        private CharacterController _characterController;
        private Vector3 _playerVelocity;
        private bool _isMoving = false;
        private float _currentTunnelVisionIntensity = 0f;
        private Transform _cameraTransform;
        
        // Events
        public System.Action<bool> OnTunnelVisionChanged;
        public System.Action<float> OnSnapTurn;
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _cameraTransform = Camera.main.transform;
            
            // Initialize comfort settings
            UpdateComfortSettings();
        }
        
        private void Update()
        {
            HandleMovement();
            HandleTurning();
            UpdateTunnelVision();
        }
        
        /// <summary>
        /// Handle player movement
        /// </summary>
        private void HandleMovement()
        {
            // Get movement input from XR controller
            Vector2 moveInput = Vector2.zero;
            
            if (leftController != null)
            {
                leftController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out moveInput);
            }
            
            // Check if player is moving
            _isMoving = moveInput.magnitude > 0.1f;
            
            if (_isMoving)
            {
                // Convert input to world space movement
                Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
                move = transform.TransformDirection(move);
                move.y = 0f;
                move.Normalize();
                
                // Apply movement
                if (_characterController != null)
                {
                    _characterController.Move(move * moveSpeed * Time.deltaTime);
                }
                
                // Apply gravity if enabled
                if (useGravity && _characterController != null)
                {
                    if (_characterController.isGrounded && _playerVelocity.y < 0)
                    {
                        _playerVelocity.y = -2f;
                    }
                    
                    _playerVelocity.y += gravity * Time.deltaTime;
                    _characterController.Move(_playerVelocity * Time.deltaTime);
                }
            }
        }
        
        /// <summary>
        /// Handle player turning
        /// </summary>
        private void HandleTurning()
        {
            if (useSnapTurn)
            {
                HandleSnapTurn();
            }
            else
            {
                HandleSmoothTurn();
            }
        }
        
        /// <summary>
        /// Handle snap turning
        /// </summary>
        private void HandleSnapTurn()
        {
            // Get turn input from XR controller
            float turnInput = 0f;
            
            if (rightController != null)
            {
                rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 axis);
                turnInput = axis.x;
            }
            
            // Check for snap turn input
            if (Mathf.Abs(turnInput) > 0.5f)
            {
                float angle = turnInput > 0 ? snapTurnAngle : -snapTurnAngle;
                transform.Rotate(Vector3.up, angle);
                OnSnapTurn?.Invoke(angle);
            }
        }
        
        /// <summary>
        /// Handle smooth turning
        /// </summary>
        private void HandleSmoothTurn()
        {
            // Get turn input from XR controller
            float turnInput = 0f;
            
            if (rightController != null)
            {
                rightController.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out Vector2 axis);
                turnInput = axis.x;
            }
            
            // Apply smooth turning
            transform.Rotate(Vector3.up, turnInput * smoothTurnSpeed * Time.deltaTime);
        }
        
        /// <summary>
        /// Update tunnel vision effect based on movement
        /// </summary>
        private void UpdateTunnelVision()
        {
            if (!useTunnelVision) return;
            
            // Adjust tunnel vision intensity based on movement
            float targetIntensity = _isMoving ? tunnelVisionIntensity : 0f;
            _currentTunnelVisionIntensity = Mathf.Lerp(_currentTunnelVisionIntensity, targetIntensity, tunnelVisionFadeSpeed * Time.deltaTime);
            
            // Update tunnel vision effect
            if (tunnelVisionEffect != null)
            {
                tunnelVisionEffect.SetActive(_currentTunnelVisionIntensity > 0.01f);
                
                // In a full implementation, we would adjust the effect intensity
                // This would typically involve adjusting material properties or shader parameters
            }
            
            OnTunnelVisionChanged?.Invoke(_currentTunnelVisionIntensity > 0.01f);
        }
        
        /// <summary>
        /// Update comfort settings based on current configuration
        /// </summary>
        public void UpdateComfortSettings()
        {
            // Enable/disable tunnel vision effect
            if (tunnelVisionEffect != null)
            {
                tunnelVisionEffect.SetActive(useTunnelVision);
            }
            
            // Configure turn providers
            if (continuousTurnProvider != null)
            {
                continuousTurnProvider.enabled = !useSnapTurn;
                continuousTurnProvider.turnSpeed = smoothTurnSpeed;
            }
            
            if (snapTurnProvider != null)
            {
                snapTurnProvider.enabled = useSnapTurn;
                snapTurnProvider.turnAmount = snapTurnAngle;
            }
            
            // Configure move provider
            if (continuousMoveProvider != null)
            {
                continuousMoveProvider.enabled = true;
                continuousMoveProvider.moveSpeed = moveSpeed;
            }
        }
        
        /// <summary>
        /// Set whether to use tunnel vision
        /// </summary>
        /// <param name="useTunnel">True to use tunnel vision</param>
        public void SetTunnelVision(bool useTunnel)
        {
            useTunnelVision = useTunnel;
            UpdateComfortSettings();
        }
        
        /// <summary>
        /// Set whether to use snap turn
        /// </summary>
        /// <param name="useSnap">True to use snap turn</param>
        public void SetSnapTurn(bool useSnap)
        {
            useSnapTurn = useSnap;
            UpdateComfortSettings();
        }
        
        /// <summary>
        /// Set movement speed
        /// </summary>
        /// <param name="speed">Movement speed</param>
        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
            
            if (continuousMoveProvider != null)
            {
                continuousMoveProvider.moveSpeed = moveSpeed;
            }
        }
        
        /// <summary>
        /// Get current movement speed
        /// </summary>
        /// <returns>Movement speed</returns>
        public float GetMoveSpeed()
        {
            return moveSpeed;
        }
        
        /// <summary>
        /// Get whether tunnel vision is enabled
        /// </summary>
        /// <returns>True if tunnel vision is enabled</returns>
        public bool IsTunnelVisionEnabled()
        {
            return useTunnelVision;
        }
        
        /// <summary>
        /// Get whether snap turn is enabled
        /// </summary>
        /// <returns>True if snap turn is enabled</returns>
        public bool IsSnapTurnEnabled()
        {
            return useSnapTurn;
        }
    }
}