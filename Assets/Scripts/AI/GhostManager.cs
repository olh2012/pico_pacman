using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PacMan.GameSystem;

namespace PacMan.AI
{
    /// <summary>
    /// Manages all ghosts in the game, coordinating their behavior and state transitions
    /// </summary>
    public class GhostManager : MonoBehaviour
    {
        [Header("Ghost References")]
        [SerializeField] private Ghost[] ghosts;
        
        [Header("Timing Settings")]
        [SerializeField] private float scatterDuration = 7f;
        [SerializeField] private float chaseDuration = 20f;
        [SerializeField] private float frightenedDuration = 10f;
        [SerializeField] private float flashingDuration = 3f;
        
        [Header("Difficulty Settings")]
        [SerializeField] private float[] ghostSpeedModifiers = { 1.0f, 1.1f, 1.2f, 1.3f }; // Per level
        [SerializeField] private int currentLevel = 1;
        
        private GameManager _gameManager;
        private bool _isFrightened = false;
        private float _frightenedTimer = 0f;
        private Coroutine _behaviorCoroutine;
        
        // Events
        public System.Action OnGhostsFrightened;
        public System.Action OnGhostsNormal;
        
        private void Start()
        {
            // Get references to game systems
            _gameManager = GameManager.Instance;
            
            // Find all ghosts if not manually assigned
            if (ghosts == null || ghosts.Length == 0)
            {
                ghosts = FindObjectsOfType<Ghost>();
            }
            
            // Subscribe to game state changes
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged += HandleGameStateChanged;
                _gameManager.OnLevelChanged += HandleLevelChanged;
            }
            
            // Start the behavior cycle
            StartBehaviorCycle();
        }
        
        /// <summary>
        /// Start the ghost behavior cycle (scatter/chase)
        /// </summary>
        private void StartBehaviorCycle()
        {
            if (_behaviorCoroutine != null)
            {
                StopCoroutine(_behaviorCoroutine);
            }
            
            _behaviorCoroutine = StartCoroutine(GhostBehaviorCycle());
        }
        
        /// <summary>
        /// Coroutine to manage ghost behavior cycles
        /// </summary>
        private IEnumerator GhostBehaviorCycle()
        {
            while (true)
            {
                // Scatter mode
                SetGhostsToScatter();
                yield return new WaitForSeconds(scatterDuration);
                
                // Chase mode
                SetGhostsToChase();
                yield return new WaitForSeconds(chaseDuration);
            }
        }
        
        /// <summary>
        /// Set all ghosts to scatter mode
        /// </summary>
        private void SetGhostsToScatter()
        {
            if (_isFrightened) return;
            
            foreach (Ghost ghost in ghosts)
            {
                if (ghost != null && ghost.GetState() != Ghost.GhostState.ReturningHome)
                {
                    // Ghosts don't change to scatter if they're returning home
                    // This state is handled internally by the ghost
                }
            }
        }
        
        /// <summary>
        /// Set all ghosts to chase mode
        /// </summary>
        private void SetGhostsToChase()
        {
            if (_isFrightened) return;
            
            foreach (Ghost ghost in ghosts)
            {
                if (ghost != null && ghost.GetState() != Ghost.GhostState.ReturningHome)
                {
                    // Ghosts don't change to chase if they're returning home
                    // This state is handled internally by the ghost
                }
            }
        }
        
        /// <summary>
        /// Set all ghosts to frightened mode (when player eats power pellet)
        /// </summary>
        public void SetGhostsToFrightened()
        {
            if (_isFrightened) return;
            
            _isFrightened = true;
            _frightenedTimer = frightenedDuration;
            
            foreach (Ghost ghost in ghosts)
            {
                if (ghost != null && ghost.GetState() != Ghost.GhostState.ReturningHome)
                {
                    ghost.SetFrightened();
                }
            }
            
            OnGhostsFrightened?.Invoke();
        }
        
        /// <summary>
        /// Update frightened timer and handle transitions
        /// </summary>
        private void Update()
        {
            if (_isFrightened)
            {
                _frightenedTimer -= Time.deltaTime;
                
                // Start flashing when time is running out
                if (_frightenedTimer <= flashingDuration && _frightenedTimer > 0)
                {
                    // Notify ghosts to start flashing
                    foreach (Ghost ghost in ghosts)
                    {
                        if (ghost != null && ghost.GetState() == Ghost.GhostState.Frightened)
                        {
                            ghost.SetRecovering();
                        }
                    }
                }
                else if (_frightenedTimer <= 0)
                {
                    // Return to normal behavior
                    _isFrightened = false;
                    
                    foreach (Ghost ghost in ghosts)
                    {
                        if (ghost != null && 
                            (ghost.GetState() == Ghost.GhostState.Frightened || 
                             ghost.GetState() == Ghost.GhostState.Recovering))
                        {
                            // Ghosts return to normal state
                        }
                    }
                    
                    OnGhostsNormal?.Invoke();
                    
                    // Restart behavior cycle
                    StartBehaviorCycle();
                }
            }
        }
        
        /// <summary>
        /// Handle game state changes
        /// </summary>
        /// <param name="newState">New game state</param>
        private void HandleGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Playing:
                    // Resume ghost behavior
                    if (_behaviorCoroutine == null)
                    {
                        StartBehaviorCycle();
                    }
                    break;
                case GameState.Paused:
                    // Pause ghost behavior
                    if (_behaviorCoroutine != null)
                    {
                        StopCoroutine(_behaviorCoroutine);
                        _behaviorCoroutine = null;
                    }
                    break;
                case GameState.GameOver:
                    // Stop all ghost behavior
                    if (_behaviorCoroutine != null)
                    {
                        StopCoroutine(_behaviorCoroutine);
                        _behaviorCoroutine = null;
                    }
                    break;
            }
        }
        
        /// <summary>
        /// Handle level changes
        /// </summary>
        /// <param name="newLevel">New level</param>
        private void HandleLevelChanged(int newLevel)
        {
            currentLevel = newLevel;
            
            // Adjust ghost speeds based on level
            AdjustGhostSpeeds();
        }
        
        /// <summary>
        /// Adjust ghost speeds based on current level
        /// </summary>
        private void AdjustGhostSpeeds()
        {
            int levelIndex = Mathf.Min(currentLevel - 1, ghostSpeedModifiers.Length - 1);
            float speedModifier = ghostSpeedModifiers[levelIndex];
            
            // In a full implementation, we would adjust each ghost's speed
            // This is handled individually by each ghost class
        }
        
        /// <summary>
        /// Reset all ghosts to their initial positions
        /// </summary>
        public void ResetGhosts()
        {
            foreach (Ghost ghost in ghosts)
            {
                if (ghost != null)
                {
                    ghost.ResetGhost();
                }
            }
            
            _isFrightened = false;
            _frightenedTimer = 0f;
            
            // Restart behavior cycle
            StartBehaviorCycle();
        }
        
        /// <summary>
        /// Get the current frightened state
        /// </summary>
        /// <returns>True if ghosts are frightened</returns>
        public bool IsFrightened()
        {
            return _isFrightened;
        }
        
        /// <summary>
        /// Get the remaining frightened time
        /// </summary>
        /// <returns>Remaining frightened time</returns>
        public float GetFrightenedTimeRemaining()
        {
            return _frightenedTimer;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged -= HandleGameStateChanged;
                _gameManager.OnLevelChanged -= HandleLevelChanged;
            }
        }
    }
}