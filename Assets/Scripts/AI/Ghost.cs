using UnityEngine;
using PacMan.GameSystem;

namespace PacMan.AI
{
    /// <summary>
    /// Base class for all ghosts in the game
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Ghost : MonoBehaviour
    {
        [Header("Ghost Settings")]
        [SerializeField] protected float moveSpeed = 3f;
        [SerializeField] protected float frightenedSpeed = 1.5f;
        [SerializeField] protected float recoverSpeed = 2f;
        
        [Header("Behavior")]
        [SerializeField] protected float directionChangeInterval = 2f;
        [SerializeField] protected LayerMask wallLayerMask = 1 << 0; // Default layer for walls
        
        protected enum GhostState
        {
            Chase,
            Scatter,
            Frightened,
            Recovering,
            ReturningHome
        }
        
        protected GhostState currentState = GhostState.Chase;
        protected Rigidbody _rigidbody;
        protected Vector3 _moveDirection;
        protected float _directionChangeTimer = 0f;
        protected Vector3 _homePosition;
        protected bool _isMoving = true;
        
        // References
        protected Transform playerTransform;
        protected GameManager gameManager;
        
        // Events
        public System.Action OnGhostEaten;
        public System.Action OnGhostCaughtPlayer;
        
        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _homePosition = transform.position;
        }
        
        protected virtual void Start()
        {
            // Find the player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            
            // Get game manager reference
            gameManager = GameManager.Instance;
            
            // Subscribe to game state changes
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged += HandleGameStateChanged;
            }
            
            // Initialize movement direction
            _moveDirection = GetRandomDirection();
        }
        
        protected virtual void Update()
        {
            if (!_isMoving) return;
            
            HandleMovement();
            HandleDirectionChange();
            HandleStateBehavior();
        }
        
        /// <summary>
        /// Handle ghost movement based on current state
        /// </summary>
        protected virtual void HandleMovement()
        {
            float currentSpeed = GetCurrentSpeed();
            
            // Check for wall collisions
            if (Physics.Raycast(transform.position, _moveDirection, 1f, wallLayerMask))
            {
                // Change direction if about to hit a wall
                _moveDirection = GetRandomDirection();
            }
            
            // Move the ghost
            Vector3 newPosition = transform.position + _moveDirection * currentSpeed * Time.deltaTime;
            _rigidbody.MovePosition(newPosition);
        }
        
        /// <summary>
        /// Handle periodic direction changes
        /// </summary>
        protected virtual void HandleDirectionChange()
        {
            _directionChangeTimer += Time.deltaTime;
            
            if (_directionChangeTimer >= directionChangeInterval)
            {
                _moveDirection = GetRandomDirection();
                _directionChangeTimer = 0f;
            }
        }
        
        /// <summary>
        /// Handle behavior based on current state
        /// </summary>
        protected virtual void HandleStateBehavior()
        {
            switch (currentState)
            {
                case GhostState.Chase:
                    HandleChaseBehavior();
                    break;
                case GhostState.Scatter:
                    HandleScatterBehavior();
                    break;
                case GhostState.Frightened:
                    HandleFrightenedBehavior();
                    break;
                case GhostState.Recovering:
                    HandleRecoveringBehavior();
                    break;
                case GhostState.ReturningHome:
                    HandleReturningHomeBehavior();
                    break;
            }
        }
        
        /// <summary>
        /// Handle chase behavior (specific to each ghost type)
        /// </summary>
        protected virtual void HandleChaseBehavior()
        {
            // Base implementation - move randomly
            // Specific ghosts will override this
        }
        
        /// <summary>
        /// Handle scatter behavior (move to corners)
        /// </summary>
        protected virtual void HandleScatterBehavior()
        {
            // Move toward a corner of the maze
            // This would be implemented based on the specific maze layout
        }
        
        /// <summary>
        /// Handle frightened behavior (run away from player)
        /// </summary>
        protected virtual void HandleFrightenedBehavior()
        {
            if (playerTransform != null)
            {
                // Move away from the player
                Vector3 directionToPlayer = (transform.position - playerTransform.position).normalized;
                _moveDirection = directionToPlayer;
            }
        }
        
        /// <summary>
        /// Handle recovering behavior (transition from frightened to normal)
        /// </summary>
        protected virtual void HandleRecoveringBehavior()
        {
            // Flashing effect would be handled visually
        }
        
        /// <summary>
        /// Handle returning home behavior (after being eaten)
        /// </summary>
        protected virtual void HandleReturningHomeBehavior()
        {
            // Move back to home position
            Vector3 directionToHome = (_homePosition - transform.position).normalized;
            _moveDirection = directionToHome;
            
            // Check if reached home
            if (Vector3.Distance(transform.position, _homePosition) < 0.5f)
            {
                currentState = GhostState.Chase;
                _isMoving = true;
            }
        }
        
        /// <summary>
        /// Get a random movement direction
        /// </summary>
        /// <returns>Random direction vector</returns>
        protected Vector3 GetRandomDirection()
        {
            // Get a random direction on the XZ plane
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            return direction;
        }
        
        /// <summary>
        /// Get current movement speed based on state
        /// </summary>
        /// <returns>Current speed</returns>
        protected float GetCurrentSpeed()
        {
            switch (currentState)
            {
                case GhostState.Frightened:
                    return frightenedSpeed;
                case GhostState.Recovering:
                    return recoverSpeed;
                case GhostState.ReturningHome:
                    return moveSpeed * 1.5f; // Faster when returning home
                default:
                    return moveSpeed;
            }
        }
        
        /// <summary>
        /// Set the ghost to frightened state (when player eats power pellet)
        /// </summary>
        public virtual void SetFrightened()
        {
            if (currentState != GhostState.ReturningHome)
            {
                currentState = GhostState.Frightened;
                // Visual change would be handled here (blue color)
            }
        }
        
        /// <summary>
        /// Set the ghost to recovering state (when power-up is about to expire)
        /// </summary>
        public virtual void SetRecovering()
        {
            if (currentState == GhostState.Frightened)
            {
                currentState = GhostState.Recovering;
                // Visual change would be handled here (flashing)
            }
        }
        
        /// <summary>
        /// Set the ghost to returning home state (when eaten by player)
        /// </summary>
        public virtual void SetReturningHome()
        {
            currentState = GhostState.ReturningHome;
            _isMoving = true;
            OnGhostEaten?.Invoke();
            // Visual change would be handled here (eyes only)
        }
        
        /// <summary>
        /// Handle game state changes
        /// </summary>
        /// <param name="newState">New game state</param>
        protected virtual void HandleGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Playing:
                    _isMoving = true;
                    break;
                case GameState.Paused:
                    _isMoving = false;
                    break;
                case GameState.GameOver:
                    _isMoving = false;
                    break;
            }
        }
        
        /// <summary>
        /// Handle collision with player
        /// </summary>
        /// <param name="other">The collider that was hit</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (currentState == GhostState.Frightened || currentState == GhostState.Recovering)
                {
                    // Player can eat this ghost
                    SetReturningHome();
                }
                else if (currentState != GhostState.ReturningHome)
                {
                    // Ghost catches player
                    OnGhostCaughtPlayer?.Invoke();
                }
            }
        }
        
        /// <summary>
        /// Reset ghost to initial state
        /// </summary>
        public virtual void ResetGhost()
        {
            transform.position = _homePosition;
            currentState = GhostState.Chase;
            _isMoving = true;
            _moveDirection = GetRandomDirection();
        }
        
        /// <summary>
        /// Get current ghost state
        /// </summary>
        /// <returns>Current state</returns>
        public GhostState GetState()
        {
            return currentState;
        }
    }
}