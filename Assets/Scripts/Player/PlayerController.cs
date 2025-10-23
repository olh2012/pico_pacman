using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PacMan.GameSystem;

namespace PacMan.Player
{
    /// <summary>
    /// Player controller for VR Pac-Man game
    /// Handles movement, collision detection, and interaction with game elements
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private bool useContinuousMovement = true;
        [SerializeField] private bool useSmoothTurn = true;
        [SerializeField] private float turnSpeed = 60f;
        [SerializeField] private float gravity = -9.81f;
        
        [Header("VR Interaction")]
        [SerializeField] private XRController leftController;
        [SerializeField] private XRController rightController;
        [SerializeField] private XRRayInteractor leftRayInteractor;
        [SerializeField] private XRRayInteractor rightRayInteractor;
        
        [Header("Gameplay")]
        [SerializeField] private LayerMask pelletLayerMask = 1 << 5; // Default layer for pellets
        [SerializeField] private LayerMask ghostLayerMask = 1 << 6; // Default layer for ghosts
        [SerializeField] private float interactionRadius = 0.5f;
        
        private Rigidbody _rigidbody;
        private CharacterController _characterController;
        private Vector3 _playerVelocity;
        private bool _isGrounded;
        private Transform _cameraTransform;
        private InputManager _inputManager;
        
        // Input actions
        private Vector2 _moveInput;
        private Vector2 _turnInput;
        private bool _isPoweredUp = false;
        private float _powerUpTimer = 0f;
        private float _powerUpDuration = 10f;
        
        // Events
        public System.Action OnPelletCollected;
        public System.Action OnPowerPelletCollected;
        public System.Action OnGhostEaten;
        public System.Action OnPlayerCaught;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _characterController = GetComponent<CharacterController>();
            _cameraTransform = Camera.main.transform;
            
            // Find InputManager in the scene
            _inputManager = FindObjectOfType<InputManager>();
            if (_inputManager != null)
            {
                // Subscribe to input events
                _inputManager.OnLeftJoystickMoved += HandleMoveInput;
                _inputManager.OnRightJoystickMoved += HandleTurnInput;
            }
            
            // Lock cursor for desktop testing (optional)
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from input events
            if (_inputManager != null)
            {
                _inputManager.OnLeftJoystickMoved -= HandleMoveInput;
                _inputManager.OnRightJoystickMoved -= HandleTurnInput;
            }
        }
        
        private void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleInteraction();
            HandlePowerUpTimer();
        }
        
        /// <summary>
        /// Handle player movement input from VR controllers
        /// </summary>
        /// <param name="moveInput">Movement input vector</param>
        private void HandleMoveInput(Vector2 moveInput)
        {
            _moveInput = moveInput;
        }
        
        /// <summary>
        /// Handle player turning input from VR controllers
        /// </summary>
        /// <param name="turnInput">Turning input vector</param>
        private void HandleTurnInput(Vector2 turnInput)
        {
            _turnInput = turnInput;
        }
        
        /// <summary>
        /// Handle player movement
        /// </summary>
        private void HandleMovement()
        {
            if (useContinuousMovement)
            {
                // Continuous movement based on input
                Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
                move = _cameraTransform.TransformDirection(move);
                move.y = 0f;
                
                if (_characterController != null)
                {
                    _characterController.Move(move * moveSpeed * Time.deltaTime);
                }
                else
                {
                    _rigidbody.velocity = new Vector3(move.x * moveSpeed, _rigidbody.velocity.y, move.z * moveSpeed);
                }
            }
            else
            {
                // Teleportation movement would be handled by a separate system
            }
            
            // Apply gravity
            if (_characterController != null)
            {
                _isGrounded = _characterController.isGrounded;
                if (_isGrounded && _playerVelocity.y < 0)
                {
                    _playerVelocity.y = -2f;
                }
                
                _playerVelocity.y += gravity * Time.deltaTime;
                _characterController.Move(_playerVelocity * Time.deltaTime);
            }
        }
        
        /// <summary>
        /// Handle player rotation
        /// </summary>
        private void HandleRotation()
        {
            if (useSmoothTurn)
            {
                // Smooth turning
                transform.Rotate(Vector3.up, _turnInput.x * turnSpeed * Time.deltaTime);
            }
            else
            {
                // Snap turning could be implemented here
            }
        }
        
        /// <summary>
        /// Handle interaction with game objects (pellets, ghosts, etc.)
        /// </summary>
        private void HandleInteraction()
        {
            // Check for pellet collection
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRadius, pelletLayerMask);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Pellet"))
                {
                    CollectPellet(hitCollider.gameObject);
                }
                else if (hitCollider.CompareTag("PowerPellet"))
                {
                    CollectPowerPellet(hitCollider.gameObject);
                }
            }
            
            // Check for ghost interaction
            Collider[] ghostColliders = Physics.OverlapSphere(transform.position, interactionRadius, ghostLayerMask);
            foreach (var ghostCollider in ghostColliders)
            {
                if (ghostCollider.CompareTag("Ghost"))
                {
                    if (_isPoweredUp)
                    {
                        // Eat the ghost
                        EatGhost(ghostCollider.gameObject);
                    }
                    else
                    {
                        // Get caught by ghost
                        GetCaughtByGhost(ghostCollider.gameObject);
                    }
                }
            }
        }
        
        /// <summary>
        /// Collect a regular pellet
        /// </summary>
        /// <param name="pellet">The pellet GameObject</param>
        private void CollectPellet(GameObject pellet)
        {
            Destroy(pellet);
            OnPelletCollected?.Invoke();
            Debug.Log("Pellet collected");
        }
        
        /// <summary>
        /// Collect a power pellet
        /// </summary>
        /// <param name="powerPellet">The power pellet GameObject</param>
        private void CollectPowerPellet(GameObject powerPellet)
        {
            Destroy(powerPellet);
            ActivatePowerUp();
            OnPowerPelletCollected?.Invoke();
            Debug.Log("Power pellet collected");
        }
        
        /// <summary>
        /// Activate power-up state
        /// </summary>
        private void ActivatePowerUp()
        {
            _isPoweredUp = true;
            _powerUpTimer = _powerUpDuration;
            // Visual effect for power-up could be added here
            Debug.Log("Power-up activated");
        }
        
        /// <summary>
        /// Handle power-up timer
        /// </summary>
        private void HandlePowerUpTimer()
        {
            if (_isPoweredUp)
            {
                _powerUpTimer -= Time.deltaTime;
                if (_powerUpTimer <= 0)
                {
                    _isPoweredUp = false;
                    Debug.Log("Power-up expired");
                }
            }
        }
        
        /// <summary>
        /// Eat a ghost
        /// </summary>
        /// <param name="ghost">The ghost GameObject</param>
        private void EatGhost(GameObject ghost)
        {
            // In a full implementation, we would notify the ghost to return to its spawn point
            // For now, we'll just trigger the event
            OnGhostEaten?.Invoke();
            Debug.Log("Ghost eaten");
        }
        
        /// <summary>
        /// Get caught by a ghost
        /// </summary>
        /// <param name="ghost">The ghost GameObject</param>
        private void GetCaughtByGhost(GameObject ghost)
        {
            if (!_isPoweredUp)
            {
                OnPlayerCaught?.Invoke();
                Debug.Log("Caught by ghost");
            }
        }
        
        /// <summary>
        /// Set whether the player is powered up
        /// </summary>
        /// <param name="poweredUp">True if powered up</param>
        public void SetPoweredUp(bool poweredUp)
        {
            _isPoweredUp = poweredUp;
        }
        
        /// <summary>
        /// Get whether the player is powered up
        /// </summary>
        /// <returns>True if powered up</returns>
        public bool IsPoweredUp()
        {
            return _isPoweredUp;
        }
        
        /// <summary>
        /// OnDrawGizmos to visualize interaction radius
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}