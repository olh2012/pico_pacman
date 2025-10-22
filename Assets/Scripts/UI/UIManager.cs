using UnityEngine;
using UnityEngine.UI;
using PacMan.GameSystem;
using PacMan.Player;

namespace PacMan.UI
{
    /// <summary>
    /// Manages the game's user interface including menus and HUD
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject hudPanel;
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private GameObject gameOverPanel;
        
        [Header("HUD Elements")]
        [SerializeField] private Text scoreText;
        [SerializeField] private Text highScoreText;
        [SerializeField] private Text livesText;
        [SerializeField] private Text levelText;
        
        [Header("Menu Elements")]
        [SerializeField] private Text mainMenuHighScoreText;
        [SerializeField] private Text gameOverScoreText;
        [SerializeField] private Text gameOverHighScoreText;
        
        [Header("VR UI")]
        [SerializeField] private GameObject vrMenu;
        [SerializeField] private GameObject vrHUD;
        
        private GameManager _gameManager;
        private PlayerController _playerController;
        
        private void Start()
        {
            // Get references to game systems
            _gameManager = GameManager.Instance;
            _playerController = FindObjectOfType<PlayerController>();
            
            // Subscribe to events
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged += HandleGameStateChanged;
                _gameManager.OnScoreChanged += UpdateScoreDisplay;
                _gameManager.OnLivesChanged += UpdateLivesDisplay;
                _gameManager.OnLevelChanged += UpdateLevelDisplay;
            }
            
            if (_playerController != null)
            {
                _playerController.OnPelletCollected += HandlePelletCollected;
                _playerController.OnPowerPelletCollected += HandlePowerPelletCollected;
                _playerController.OnGhostEaten += HandleGhostEaten;
                _playerController.OnPlayerCaught += HandlePlayerCaught;
            }
            
            // Initialize UI
            InitializeUI();
        }
        
        /// <summary>
        /// Initialize the UI elements
        /// </summary>
        private void InitializeUI()
        {
            // Hide all panels initially
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (hudPanel != null) hudPanel.SetActive(false);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            
            // Update initial displays
            if (_gameManager != null)
            {
                UpdateScoreDisplay(_gameManager.GetScore());
                UpdateLivesDisplay(_gameManager.GetRemainingLives());
                UpdateLevelDisplay(_gameManager.GetLevel());
                
                if (mainMenuHighScoreText != null)
                {
                    mainMenuHighScoreText.text = $"High Score: {_gameManager.GetHighScore()}";
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
                case GameState.Menu:
                    ShowMainMenu();
                    break;
                case GameState.Playing:
                    ShowHUD();
                    break;
                case GameState.Paused:
                    ShowPauseMenu();
                    break;
                case GameState.GameOver:
                    ShowGameOver();
                    break;
                case GameState.LevelComplete:
                    // Handle level complete
                    break;
            }
        }
        
        /// <summary>
        /// Show the main menu
        /// </summary>
        private void ShowMainMenu()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (hudPanel != null) hudPanel.SetActive(false);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
            
            // Update high score display
            if (_gameManager != null && mainMenuHighScoreText != null)
            {
                mainMenuHighScoreText.text = $"High Score: {_gameManager.GetHighScore()}";
            }
        }
        
        /// <summary>
        /// Show the HUD during gameplay
        /// </summary>
        private void ShowHUD()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (hudPanel != null) hudPanel.SetActive(true);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
        }
        
        /// <summary>
        /// Show the pause menu
        /// </summary>
        private void ShowPauseMenu()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (hudPanel != null) hudPanel.SetActive(false);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
            if (gameOverPanel != null) gameOverPanel.SetActive(false);
        }
        
        /// <summary>
        /// Show the game over screen
        /// </summary>
        private void ShowGameOver()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (hudPanel != null) hudPanel.SetActive(false);
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
            
            // Update game over displays
            if (_gameManager != null)
            {
                if (gameOverScoreText != null)
                {
                    gameOverScoreText.text = $"Score: {_gameManager.GetScore()}";
                }
                
                if (gameOverHighScoreText != null)
                {
                    gameOverHighScoreText.text = $"High Score: {_gameManager.GetHighScore()}";
                }
            }
        }
        
        /// <summary>
        /// Update the score display
        /// </summary>
        /// <param name="score">Current score</param>
        private void UpdateScoreDisplay(int score)
        {
            if (scoreText != null)
            {
                scoreText.text = $"Score: {score}";
            }
        }
        
        /// <summary>
        /// Update the lives display
        /// </summary>
        /// <param name="lives">Remaining lives</param>
        private void UpdateLivesDisplay(int lives)
        {
            if (livesText != null)
            {
                livesText.text = $"Lives: {lives}";
            }
        }
        
        /// <summary>
        /// Update the level display
        /// </summary>
        /// <param name="level">Current level</param>
        private void UpdateLevelDisplay(int level)
        {
            if (levelText != null)
            {
                levelText.text = $"Level: {level}";
            }
        }
        
        /// <summary>
        /// Handle pellet collection
        /// </summary>
        private void HandlePelletCollected()
        {
            // Could show a visual effect or animation
            Debug.Log("Pellet collected UI feedback");
        }
        
        /// <summary>
        /// Handle power pellet collection
        /// </summary>
        private void HandlePowerPelletCollected()
        {
            // Could show a special effect
            Debug.Log("Power pellet collected UI feedback");
        }
        
        /// <summary>
        /// Handle ghost eaten
        /// </summary>
        private void HandleGhostEaten()
        {
            // Could show points earned
            Debug.Log("Ghost eaten UI feedback");
        }
        
        /// <summary>
        /// Handle player caught by ghost
        /// </summary>
        private void HandlePlayerCaught()
        {
            // Could show a visual effect
            Debug.Log("Player caught UI feedback");
        }
        
        /// <summary>
        /// Start a new game
        /// </summary>
        public void StartNewGame()
        {
            if (_gameManager != null)
            {
                _gameManager.StartNewGame();
            }
        }
        
        /// <summary>
        /// Resume the game from pause
        /// </summary>
        public void ResumeGame()
        {
            if (_gameManager != null)
            {
                _gameManager.ChangeGameState(GameState.Playing);
            }
        }
        
        /// <summary>
        /// Return to main menu
        /// </summary>
        public void ReturnToMainMenu()
        {
            if (_gameManager != null)
            {
                _gameManager.ChangeGameState(GameState.Menu);
            }
        }
        
        /// <summary>
        /// Quit the application
        /// </summary>
        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        
        /// <summary>
        /// Toggle pause state
        /// </summary>
        public void TogglePause()
        {
            if (_gameManager != null)
            {
                GameState currentState = _gameManager.GetGameState();
                if (currentState == GameState.Playing)
                {
                    _gameManager.ChangeGameState(GameState.Paused);
                }
                else if (currentState == GameState.Paused)
                {
                    _gameManager.ChangeGameState(GameState.Playing);
                }
            }
        }
    }
}