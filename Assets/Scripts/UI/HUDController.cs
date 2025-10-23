using UnityEngine;
using UnityEngine.UI;
using PacMan.GameSystem;
using PacMan.AI;

namespace PacMan.UI
{
    /// <summary>
    /// Controls the heads-up display during gameplay
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        [Header("HUD Elements")]
        [SerializeField] private Text scoreText;
        [SerializeField] private Text highScoreText;
        [SerializeField] private Text livesText;
        [SerializeField] private Text levelText;
        [SerializeField] private Text ghostComboText;
        [SerializeField] private GameObject powerUpIndicator;
        [SerializeField] private Image powerUpTimerBar;
        [SerializeField] private GameObject[] lifeIcons;
        
        [Header("Ghost Status Indicators")]
        [SerializeField] private Image[] ghostStatusIndicators;
        [SerializeField] private Color normalGhostColor = Color.red;
        [SerializeField] private Color frightenedGhostColor = Color.blue;
        [SerializeField] private Color recoveringGhostColor = Color.cyan;
        [SerializeField] private Color homeGhostColor = Color.white;
        
        private GameManager _gameManager;
        private ScoreManager _scoreManager;
        private GhostManager _ghostManager;
        private Player.PlayerController _playerController;
        
        private void Start()
        {
            // Get references to game systems
            _gameManager = GameManager.Instance;
            _scoreManager = FindObjectOfType<ScoreManager>();
            _ghostManager = FindObjectOfType<GhostManager>();
            _playerController = FindObjectOfType<Player.PlayerController>();
            
            // Subscribe to events
            SubscribeToEvents();
            
            // Initialize HUD
            InitializeHUD();
        }
        
        /// <summary>
        /// Subscribe to all relevant events
        /// </summary>
        private void SubscribeToEvents()
        {
            if (_gameManager != null)
            {
                _gameManager.OnScoreChanged += UpdateScoreDisplay;
                _gameManager.OnLivesChanged += UpdateLivesDisplay;
                _gameManager.OnLevelChanged += UpdateLevelDisplay;
                _gameManager.OnGameStateChanged += HandleGameStateChanged;
            }
            
            if (_scoreManager != null)
            {
                _scoreManager.OnHighScoreChanged += UpdateHighScoreDisplay;
                _scoreManager.OnComboAchieved += UpdateGhostComboDisplay;
            }
            
            if (_playerController != null)
            {
                _playerController.OnPowerPelletCollected += ShowPowerUpIndicator;
                _playerController.OnGhostEaten += UpdateGhostComboDisplay;
            }
        }
        
        /// <summary>
        /// Initialize HUD elements
        /// </summary>
        private void InitializeHUD()
        {
            // Update all displays with initial values
            if (_gameManager != null)
            {
                UpdateScoreDisplay(_gameManager.GetScore());
                UpdateLivesDisplay(_gameManager.GetRemainingLives());
                UpdateLevelDisplay(_gameManager.GetLevel());
            }
            
            if (_scoreManager != null)
            {
                UpdateHighScoreDisplay(_scoreManager.GetHighScore());
            }
            
            // Hide power-up indicator by default
            if (powerUpIndicator != null)
            {
                powerUpIndicator.SetActive(false);
            }
            
            // Hide ghost combo text by default
            if (ghostComboText != null)
            {
                ghostComboText.gameObject.SetActive(false);
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
                scoreText.text = $"SCORE: {score:D6}";
            }
        }
        
        /// <summary>
        /// Update the high score display
        /// </summary>
        /// <param name="highScore">Current high score</param>
        private void UpdateHighScoreDisplay(int highScore)
        {
            if (highScoreText != null)
            {
                highScoreText.text = $"HIGH SCORE: {highScore:D6}";
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
                livesText.text = $"LIVES: {lives}";
            }
            
            // Update life icons
            UpdateLifeIcons(lives);
        }
        
        /// <summary>
        /// Update the life icons display
        /// </summary>
        /// <param name="lives">Remaining lives</param>
        private void UpdateLifeIcons(int lives)
        {
            if (lifeIcons == null) return;
            
            for (int i = 0; i < lifeIcons.Length; i++)
            {
                if (lifeIcons[i] != null)
                {
                    lifeIcons[i].SetActive(i < lives);
                }
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
                levelText.text = $"LEVEL: {level}";
            }
        }
        
        /// <summary>
        /// Update the ghost combo display
        /// </summary>
        /// <param name="comboCount">Current combo count</param>
        private void UpdateGhostComboDisplay(int comboCount)
        {
            if (ghostComboText != null)
            {
                if (comboCount > 1)
                {
                    ghostComboText.text = $"{comboCount}x COMBO!";
                    ghostComboText.gameObject.SetActive(true);
                    
                    // Hide combo text after a delay
                    Invoke(nameof(HideGhostCombo), 2f);
                }
                else
                {
                    HideGhostCombo();
                }
            }
        }
        
        /// <summary>
        /// Hide the ghost combo display
        /// </summary>
        private void HideGhostCombo()
        {
            if (ghostComboText != null)
            {
                ghostComboText.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Show the power-up indicator
        /// </summary>
        private void ShowPowerUpIndicator()
        {
            if (powerUpIndicator != null)
            {
                powerUpIndicator.SetActive(true);
                
                // Hide power-up indicator after duration
                if (_ghostManager != null)
                {
                    Invoke(nameof(HidePowerUpIndicator), _ghostManager.GetFrightenedTimeRemaining());
                }
            }
        }
        
        /// <summary>
        /// Hide the power-up indicator
        /// </summary>
        private void HidePowerUpIndicator()
        {
            if (powerUpIndicator != null)
            {
                powerUpIndicator.SetActive(false);
            }
        }
        
        /// <summary>
        /// Update the power-up timer bar
        /// </summary>
        private void UpdatePowerUpTimerBar()
        {
            if (powerUpTimerBar != null && _ghostManager != null)
            {
                if (_ghostManager.IsFrightened())
                {
                    float remainingTime = _ghostManager.GetFrightenedTimeRemaining();
                    float totalTime = 10f; // Should match the frightened duration
                    powerUpTimerBar.fillAmount = remainingTime / totalTime;
                }
                else
                {
                    powerUpTimerBar.fillAmount = 0f;
                }
            }
        }
        
        /// <summary>
        /// Update ghost status indicators
        /// </summary>
        private void UpdateGhostStatusIndicators()
        {
            // In a full implementation, this would update the status of each ghost
            // This requires references to individual ghosts
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
                    gameObject.SetActive(true);
                    break;
                case GameState.Paused:
                    gameObject.SetActive(false);
                    break;
                case GameState.GameOver:
                    gameObject.SetActive(false);
                    break;
            }
        }
        
        private void Update()
        {
            // Update power-up timer bar every frame
            UpdatePowerUpTimerBar();
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_gameManager != null)
            {
                _gameManager.OnScoreChanged -= UpdateScoreDisplay;
                _gameManager.OnLivesChanged -= UpdateLivesDisplay;
                _gameManager.OnLevelChanged -= UpdateLevelDisplay;
                _gameManager.OnGameStateChanged -= HandleGameStateChanged;
            }
            
            if (_scoreManager != null)
            {
                _scoreManager.OnHighScoreChanged -= UpdateHighScoreDisplay;
                _scoreManager.OnComboAchieved -= UpdateGhostComboDisplay;
            }
            
            if (_playerController != null)
            {
                _playerController.OnPowerPelletCollected -= ShowPowerUpIndicator;
                _playerController.OnGhostEaten -= UpdateGhostComboDisplay;
            }
        }
    }
}