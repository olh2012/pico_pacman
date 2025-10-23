using UnityEngine;
using UnityEngine.UI;
using PacMan.GameSystem;

namespace PacMan.UI
{
    /// <summary>
    /// Manages the VR menu system including 3D UI elements and interactions
    /// </summary>
    public class VRMenu : MonoBehaviour
    {
        [Header("Menu Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject creditsPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;
        
        [Header("Main Menu Elements")]
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Text highScoreText;
        
        [Header("Settings Elements")]
        [SerializeField] private Toggle teleportationToggle;
        [SerializeField] private Toggle smoothMovementToggle;
        [SerializeField] private Toggle tunnelVisionToggle;
        [SerializeField] private Toggle snapTurnToggle;
        [SerializeField] private Slider moveSpeedSlider;
        [SerializeField] private Button backButton;
        
        [Header("Game Over Elements")]
        [SerializeField] private Text finalScoreText;
        [SerializeField] private Text gameOverHighScoreText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        
        private GameManager _gameManager;
        private ScoreManager _scoreManager;
        
        private void Start()
        {
            // Get references to game systems
            _gameManager = GameManager.Instance;
            _scoreManager = FindObjectOfType<ScoreManager>();
            
            // Initialize UI elements
            InitializeUI();
            
            // Subscribe to events
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged += HandleGameStateChanged;
            }
            
            if (_scoreManager != null)
            {
                _scoreManager.OnHighScoreChanged += UpdateHighScoreDisplay;
            }
        }
        
        /// <summary>
        /// Initialize UI elements and set up button listeners
        /// </summary>
        private void InitializeUI()
        {
            // Main menu buttons
            if (startGameButton != null)
                startGameButton.onClick.AddListener(StartGame);
                
            if (settingsButton != null)
                settingsButton.onClick.AddListener(ShowSettings);
                
            if (creditsButton != null)
                creditsButton.onClick.AddListener(ShowCredits);
                
            if (quitButton != null)
                quitButton.onClick.AddListener(QuitGame);
            
            // Settings buttons
            if (backButton != null)
                backButton.onClick.AddListener(ShowMainMenu);
            
            // Settings toggles
            if (teleportationToggle != null)
                teleportationToggle.onValueChanged.AddListener(SetTeleportation);
                
            if (smoothMovementToggle != null)
                smoothMovementToggle.onValueChanged.AddListener(SetSmoothMovement);
                
            if (tunnelVisionToggle != null)
                tunnelVisionToggle.onValueChanged.AddListener(SetTunnelVision);
                
            if (snapTurnToggle != null)
                snapTurnToggle.onValueChanged.AddListener(SetSnapTurn);
            
            // Settings slider
            if (moveSpeedSlider != null)
                moveSpeedSlider.onValueChanged.AddListener(SetMoveSpeed);
            
            // Game over buttons
            if (restartButton != null)
                restartButton.onClick.AddListener(RestartGame);
                
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(ReturnToMainMenu);
            
            // Initialize settings values
            InitializeSettings();
            
            // Show main menu by default
            ShowMainMenu();
        }
        
        /// <summary>
        /// Initialize settings values from player preferences
        /// </summary>
        private void InitializeSettings()
        {
            // Load settings from PlayerPrefs
            if (teleportationToggle != null)
                teleportationToggle.isOn = PlayerPrefs.GetInt("Teleportation", 1) == 1;
                
            if (smoothMovementToggle != null)
                smoothMovementToggle.isOn = PlayerPrefs.GetInt("SmoothMovement", 0) == 1;
                
            if (tunnelVisionToggle != null)
                tunnelVisionToggle.isOn = PlayerPrefs.GetInt("TunnelVision", 1) == 1;
                
            if (snapTurnToggle != null)
                snapTurnToggle.isOn = PlayerPrefs.GetInt("SnapTurn", 0) == 1;
                
            if (moveSpeedSlider != null)
                moveSpeedSlider.value = PlayerPrefs.GetFloat("MoveSpeed", 2.0f);
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
                    HideAllPanels();
                    break;
                case GameState.Paused:
                    ShowPauseMenu();
                    break;
                case GameState.GameOver:
                    ShowGameOver();
                    break;
            }
        }
        
        /// <summary>
        /// Show the main menu
        /// </summary>
        public void ShowMainMenu()
        {
            HideAllPanels();
            
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
                
            // Update high score display
            if (_scoreManager != null && highScoreText != null)
            {
                highScoreText.text = $"High Score: {_scoreManager.GetHighScore()}";
            }
        }
        
        /// <summary>
        /// Show the settings menu
        /// </summary>
        public void ShowSettings()
        {
            HideAllPanels();
            
            if (settingsPanel != null)
                settingsPanel.SetActive(true);
        }
        
        /// <summary>
        /// Show the credits menu
        /// </summary>
        public void ShowCredits()
        {
            HideAllPanels();
            
            if (creditsPanel != null)
                creditsPanel.SetActive(true);
        }
        
        /// <summary>
        /// Show the pause menu
        /// </summary>
        public void ShowPauseMenu()
        {
            HideAllPanels();
            
            if (pausePanel != null)
                pausePanel.SetActive(true);
        }
        
        /// <summary>
        /// Show the game over screen
        /// </summary>
        public void ShowGameOver()
        {
            HideAllPanels();
            
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
                
            // Update game over displays
            if (_gameManager != null && _scoreManager != null)
            {
                if (finalScoreText != null)
                {
                    finalScoreText.text = $"Score: {_scoreManager.GetScore()}";
                }
                
                if (gameOverHighScoreText != null)
                {
                    gameOverHighScoreText.text = $"High Score: {_scoreManager.GetHighScore()}";
                }
            }
        }
        
        /// <summary>
        /// Hide all menu panels
        /// </summary>
        private void HideAllPanels()
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(false);
                
            if (settingsPanel != null)
                settingsPanel.SetActive(false);
                
            if (creditsPanel != null)
                creditsPanel.SetActive(false);
                
            if (pausePanel != null)
                pausePanel.SetActive(false);
                
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);
        }
        
        /// <summary>
        /// Update high score display
        /// </summary>
        /// <param name="highScore">New high score</param>
        private void UpdateHighScoreDisplay(int highScore)
        {
            if (highScoreText != null)
            {
                highScoreText.text = $"High Score: {highScore}";
            }
        }
        
        /// <summary>
        /// Start a new game
        /// </summary>
        public void StartGame()
        {
            if (_gameManager != null)
            {
                _gameManager.StartNewGame();
            }
        }
        
        /// <summary>
        /// Restart the current game
        /// </summary>
        public void RestartGame()
        {
            if (_gameManager != null)
            {
                _gameManager.StartNewGame();
            }
        }
        
        /// <summary>
        /// Return to the main menu
        /// </summary>
        public void ReturnToMainMenu()
        {
            if (_gameManager != null)
            {
                _gameManager.ChangeGameState(GameState.Menu);
            }
        }
        
        /// <summary>
        /// Resume the paused game
        /// </summary>
        public void ResumeGame()
        {
            if (_gameManager != null)
            {
                _gameManager.ChangeGameState(GameState.Playing);
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
        /// Set teleportation movement mode
        /// </summary>
        /// <param name="enabled">True to enable teleportation</param>
        private void SetTeleportation(bool enabled)
        {
            PlayerPrefs.SetInt("Teleportation", enabled ? 1 : 0);
            PlayerPrefs.Save();
            
            // In a full implementation, this would communicate with the movement system
        }
        
        /// <summary>
        /// Set smooth movement mode
        /// </summary>
        /// <param name="enabled">True to enable smooth movement</param>
        private void SetSmoothMovement(bool enabled)
        {
            PlayerPrefs.SetInt("SmoothMovement", enabled ? 1 : 0);
            PlayerPrefs.Save();
            
            // In a full implementation, this would communicate with the movement system
        }
        
        /// <summary>
        /// Set tunnel vision comfort setting
        /// </summary>
        /// <param name="enabled">True to enable tunnel vision</param>
        private void SetTunnelVision(bool enabled)
        {
            PlayerPrefs.SetInt("TunnelVision", enabled ? 1 : 0);
            PlayerPrefs.Save();
            
            // In a full implementation, this would communicate with the movement system
        }
        
        /// <summary>
        /// Set snap turn setting
        /// </summary>
        /// <param name="enabled">True to enable snap turn</param>
        private void SetSnapTurn(bool enabled)
        {
            PlayerPrefs.SetInt("SnapTurn", enabled ? 1 : 0);
            PlayerPrefs.Save();
            
            // In a full implementation, this would communicate with the movement system
        }
        
        /// <summary>
        /// Set movement speed
        /// </summary>
        /// <param name="speed">Movement speed value</param>
        private void SetMoveSpeed(float speed)
        {
            PlayerPrefs.SetFloat("MoveSpeed", speed);
            PlayerPrefs.Save();
            
            // In a full implementation, this would communicate with the movement system
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged -= HandleGameStateChanged;
            }
            
            if (_scoreManager != null)
            {
                _scoreManager.OnHighScoreChanged -= UpdateHighScoreDisplay;
            }
        }
    }
}