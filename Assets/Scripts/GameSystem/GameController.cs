using UnityEngine;
using PacMan.Player;
using PacMan.AI;
using PacMan.UI;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Main game controller that coordinates all game systems
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("Game Systems")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private MazeGenerator mazeGenerator;
        [SerializeField] private PelletManager pelletManager;
        [SerializeField] private GhostManager ghostManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private VRMenu vrMenu;
        [SerializeField] private HUDController hudController;
        
        [Header("Player Systems")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private TeleportationController teleportationController;
        [SerializeField] private SmoothMovementController smoothMovementController;
        
        [Header("Level Settings")]
        [SerializeField] private TextAsset[] levelLayouts;
        [SerializeField] private int currentLevelIndex = 0;
        
        private bool _isGameActive = false;
        
        private void Start()
        {
            // Get references to game systems if not manually assigned
            if (gameManager == null) gameManager = FindObjectOfType<GameManager>();
            if (mazeGenerator == null) mazeGenerator = FindObjectOfType<MazeGenerator>();
            if (pelletManager == null) pelletManager = FindObjectOfType<PelletManager>();
            if (ghostManager == null) ghostManager = FindObjectOfType<GhostManager>();
            if (scoreManager == null) scoreManager = FindObjectOfType<ScoreManager>();
            if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
            if (vrMenu == null) vrMenu = FindObjectOfType<VRMenu>();
            if (hudController == null) hudController = FindObjectOfType<HUDController>();
            if (playerController == null) playerController = FindObjectOfType<PlayerController>();
            if (teleportationController == null) teleportationController = FindObjectOfType<TeleportationController>();
            if (smoothMovementController == null) smoothMovementController = FindObjectOfType<SmoothMovementController>();
            
            // Subscribe to game events
            SubscribeToEvents();
            
            // Initialize the game
            InitializeGame();
        }
        
        /// <summary>
        /// Subscribe to all relevant game events
        /// </summary>
        private void SubscribeToEvents()
        {
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged += HandleGameStateChanged;
                gameManager.OnLevelChanged += HandleLevelChanged;
            }
        }
        
        /// <summary>
        /// Initialize the game systems
        /// </summary>
        private void InitializeGame()
        {
            // Initialize game state
            if (gameManager != null)
            {
                // Game starts in menu state
            }
            
            // Initialize other systems
            InitializeSystems();
        }
        
        /// <summary>
        /// Initialize all game systems
        /// </summary>
        private void InitializeSystems()
        {
            // Initialize maze if we have a layout
            if (mazeGenerator != null && levelLayouts != null && levelLayouts.Length > 0)
            {
                // In a full implementation, we would load the maze layout
                // and initialize the pellet manager with pellet positions
            }
            
            // Initialize player systems
            if (teleportationController != null)
            {
                teleportationController.enabled = true;
            }
            
            if (smoothMovementController != null)
            {
                smoothMovementController.enabled = false; // Disabled by default
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
                case GameState.Menu:
                    HandleMenuState();
                    break;
                case GameState.Playing:
                    HandlePlayingState();
                    break;
                case GameState.Paused:
                    HandlePausedState();
                    break;
                case GameState.GameOver:
                    HandleGameOverState();
                    break;
                case GameState.LevelComplete:
                    HandleLevelCompleteState();
                    break;
            }
        }
        
        /// <summary>
        /// Handle menu state
        /// </summary>
        private void HandleMenuState()
        {
            _isGameActive = false;
            
            // Enable menu systems
            if (vrMenu != null)
            {
                vrMenu.ShowMainMenu();
            }
            
            // Disable gameplay systems
            SetGameplaySystemsEnabled(false);
        }
        
        /// <summary>
        /// Handle playing state
        /// </summary>
        private void HandlePlayingState()
        {
            _isGameActive = true;
            
            // Disable menu systems
            if (vrMenu != null)
            {
                vrMenu.HideAllPanels();
            }
            
            // Enable gameplay systems
            SetGameplaySystemsEnabled(true);
        }
        
        /// <summary>
        /// Handle paused state
        /// </summary>
        private void HandlePausedState()
        {
            _isGameActive = false;
            
            // Show pause menu
            if (vrMenu != null)
            {
                vrMenu.ShowPauseMenu();
            }
            
            // Pause gameplay systems
            SetGameplaySystemsEnabled(false);
        }
        
        /// <summary>
        /// Handle game over state
        /// </summary>
        private void HandleGameOverState()
        {
            _isGameActive = false;
            
            // Show game over screen
            if (vrMenu != null)
            {
                vrMenu.ShowGameOver();
            }
            
            // Disable gameplay systems
            SetGameplaySystemsEnabled(false);
            
            // Save high score
            if (scoreManager != null)
            {
                // Score is automatically saved by ScoreManager
            }
        }
        
        /// <summary>
        /// Handle level complete state
        /// </summary>
        private void HandleLevelCompleteState()
        {
            _isGameActive = false;
            
            // Advance to next level after a delay
            Invoke(nameof(AdvanceToNextLevel), 2f);
        }
        
        /// <summary>
        /// Handle level changes
        /// </summary>
        /// <param name="newLevel">New level number</param>
        private void HandleLevelChanged(int newLevel)
        {
            // Load new level layout
            LoadLevel(newLevel);
        }
        
        /// <summary>
        /// Load a specific level
        /// </summary>
        /// <param name="levelNumber">Level to load</param>
        private void LoadLevel(int levelNumber)
        {
            int levelIndex = levelNumber - 1;
            
            // Check if we have a layout for this level
            if (levelLayouts != null && levelIndex < levelLayouts.Length)
            {
                // Load the level layout
                if (mazeGenerator != null)
                {
                    // In a full implementation, we would load the maze layout
                }
            }
            else
            {
                // Use default maze or repeat last level
                Debug.LogWarning($"No layout found for level {levelNumber}, using default");
            }
            
            // Reset game systems for new level
            ResetGameSystems();
        }
        
        /// <summary>
        /// Reset all game systems for a new level
        /// </summary>
        private void ResetGameSystems()
        {
            // Reset ghosts
            if (ghostManager != null)
            {
                ghostManager.ResetGhosts();
            }
            
            // Reset pellets
            if (pelletManager != null)
            {
                pelletManager.ResetPellets();
            }
            
            // Reset player
            if (playerController != null)
            {
                // In a full implementation, we would reset player position
            }
        }
        
        /// <summary>
        /// Advance to the next level
        /// </summary>
        private void AdvanceToNextLevel()
        {
            if (gameManager != null)
            {
                // This should be handled by the GameManager
                // but we can trigger it here if needed
            }
        }
        
        /// <summary>
        /// Enable or disable all gameplay systems
        /// </summary>
        /// <param name="enabled">True to enable, false to disable</param>
        private void SetGameplaySystemsEnabled(bool enabled)
        {
            // Enable/disable player systems
            if (playerController != null)
            {
                playerController.enabled = enabled;
            }
            
            if (teleportationController != null)
            {
                teleportationController.enabled = enabled;
            }
            
            if (smoothMovementController != null)
            {
                smoothMovementController.enabled = enabled;
            }
            
            // Enable/disable AI systems
            if (ghostManager != null)
            {
                // Ghost manager handles its own enable/disable logic
            }
        }
        
        /// <summary>
        /// Get whether the game is currently active
        /// </summary>
        /// <returns>True if game is active</returns>
        public bool IsGameActive()
        {
            return _isGameActive;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (gameManager != null)
            {
                gameManager.OnGameStateChanged -= HandleGameStateChanged;
                gameManager.OnLevelChanged -= HandleLevelChanged;
            }
        }
    }
}