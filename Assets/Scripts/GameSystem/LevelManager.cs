using UnityEngine;
using System.Collections.Generic;
using PacMan.AI;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Manages level progression and difficulty scaling
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        [Header("Level Settings")]
        [SerializeField] private float[] ghostSpeedModifiers = { 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f }; // Per level
        [SerializeField] private float[] playerSpeedModifiers = { 1.0f, 1.0f, 1.1f, 1.1f, 1.2f, 1.2f }; // Per level
        [SerializeField] private float[] powerPelletDurations = { 10f, 9f, 8f, 7f, 6f, 5f }; // Decreasing duration
        [SerializeField] private int[] pointsPerGhost = { 200, 400, 800, 1600 }; // Points for eating ghosts
        
        [Header("References")]
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private PelletManager pelletManager;
        [SerializeField] private GhostManager ghostManager;
        [SerializeField] private GameManager gameManager;
        
        private int _currentLevel = 1;
        private List<TextAsset> _levelLayouts = new List<TextAsset>();
        
        // Events
        public System.Action<int> OnLevelChanged;
        public System.Action OnLevelComplete;
        
        private void Start()
        {
            // Get references
            if (mazeGenerator == null) mazeGenerator = FindObjectOfType<MazeGenerator>();
            if (pelletManager == null) pelletManager = FindObjectOfType<PelletManager>();
            if (ghostManager == null) ghostManager = FindObjectOfType<GhostManager>();
            if (gameManager == null) gameManager = GameManager.Instance;
            
            // Subscribe to events
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged += HandleGameStateChanged;
            }
        }
        
        /// <summary>
        /// Load a specific level
        /// </summary>
        /// <param name="levelNumber">Level to load</param>
        public void LoadLevel(int levelNumber)
        {
            _currentLevel = levelNumber;
            
            // Load level layout if available
            if (_levelLayouts.Count > 0 && levelNumber <= _levelLayouts.Count)
            {
                // In a full implementation, we would load the specific level layout
            }
            
            // Adjust difficulty based on level
            AdjustDifficulty();
            
            // Reset game systems for new level
            ResetLevel();
            
            OnLevelChanged?.Invoke(_currentLevel);
        }
        
        /// <summary>
        /// Advance to the next level
        /// </summary>
        public void AdvanceToNextLevel()
        {
            _currentLevel++;
            LoadLevel(_currentLevel);
        }
        
        /// <summary>
        /// Adjust game difficulty based on current level
        /// </summary>
        private void AdjustDifficulty()
        {
            // Adjust ghost speeds
            if (ghostManager != null)
            {
                // In a full implementation, we would adjust ghost speeds
            }
            
            // Adjust player speed
            // In a full implementation, we would adjust player speed
            
            // Adjust power pellet duration
            // In a full implementation, we would adjust power pellet duration
        }
        
        /// <summary>
        /// Reset level for a new game or level
        /// </summary>
        private void ResetLevel()
        {
            // Reset pellets
            if (pelletManager != null)
            {
                pelletManager.ResetPellets();
            }
            
            // Reset ghosts
            if (ghostManager != null)
            {
                ghostManager.ResetGhosts();
            }
            
            // Reset player
            // In a full implementation, we would reset player position
        }
        
        /// <summary>
        /// Handle game state changes
        /// </summary>
        /// <param name="newState">New game state</param>
        private void HandleGameStateChanged(GameState newState)
        {
            if (newState == GameState.LevelComplete)
            {
                OnLevelComplete?.Invoke();
                // Advance to next level after a delay
                Invoke(nameof(AdvanceToNextLevel), 2f);
            }
        }
        
        /// <summary>
        /// Get the current level
        /// </summary>
        /// <returns>Current level number</returns>
        public int GetCurrentLevel()
        {
            return _currentLevel;
        }
        
        /// <summary>
        /// Get ghost speed modifier for current level
        /// </summary>
        /// <returns>Ghost speed modifier</returns>
        public float GetGhostSpeedModifier()
        {
            int index = Mathf.Min(_currentLevel - 1, ghostSpeedModifiers.Length - 1);
            return ghostSpeedModifiers[index];
        }
        
        /// <summary>
        /// Get player speed modifier for current level
        /// </summary>
        /// <returns>Player speed modifier</returns>
        public float GetPlayerSpeedModifier()
        {
            int index = Mathf.Min(_currentLevel - 1, playerSpeedModifiers.Length - 1);
            return playerSpeedModifiers[index];
        }
        
        /// <summary>
        /// Get power pellet duration for current level
        /// </summary>
        /// <returns>Power pellet duration</returns>
        public float GetPowerPelletDuration()
        {
            int index = Mathf.Min(_currentLevel - 1, powerPelletDurations.Length - 1);
            return powerPelletDurations[index];
        }
        
        /// <summary>
        /// Get points for eating a ghost (based on combo)
        /// </summary>
        /// <param name="ghostCombo">Number of ghosts eaten in sequence</param>
        /// <returns>Points for eating the ghost</returns>
        public int GetGhostPoints(int ghostCombo)
        {
            int index = Mathf.Min(ghostCombo - 1, pointsPerGhost.Length - 1);
            return pointsPerGhost[index];
        }
        
        /// <summary>
        /// Add a level layout to the collection
        /// </summary>
        /// <param name="layout">Level layout to add</param>
        public void AddLevelLayout(TextAsset layout)
        {
            _levelLayouts.Add(layout);
        }
        
        /// <summary>
        /// Get total number of levels
        /// </summary>
        /// <returns>Total number of levels</returns>
        public int GetTotalLevels()
        {
            return _levelLayouts.Count > 0 ? _levelLayouts.Count : 1;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= HandleGameStateChanged;
            }
        }
    }
}