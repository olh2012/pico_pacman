using UnityEngine;
using PacMan.AI;
using PacMan.Player;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Manages scoring system including combos, multipliers, and high scores
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        [Header("Scoring Settings")]
        [SerializeField] private int smallPelletPoints = 10;
        [SerializeField] private int powerPelletPoints = 50;
        [SerializeField] private int[] ghostPoints = { 200, 400, 800, 1600 }; // Points for eating ghosts in sequence
        [SerializeField] private int fruitPoints = 100;
        
        [Header("Combo Settings")]
        [SerializeField] private float comboTimeWindow = 5f; // Time window for combos
        [SerializeField] private int comboMultiplier = 2; // Multiplier for combo scores
        
        private int _currentScore = 0;
        private int _highScore = 0;
        private int _ghostComboCount = 0;
        private float _lastGhostEatTime = 0f;
        private GameManager _gameManager;
        private PlayerController _playerController;
        private GhostManager _ghostManager;
        
        // Events
        public System.Action<int> OnScoreChanged;
        public System.Action<int> OnHighScoreChanged;
        public System.Action<int> OnComboAchieved;
        
        private void Start()
        {
            // Get references to game systems
            _gameManager = GameManager.Instance;
            _playerController = FindObjectOfType<PlayerController>();
            _ghostManager = FindObjectOfType<GhostManager>();
            
            // Load high score
            LoadHighScore();
            
            // Subscribe to events
            if (_playerController != null)
            {
                _playerController.OnPelletCollected += HandlePelletCollected;
                _playerController.OnPowerPelletCollected += HandlePowerPelletCollected;
                _playerController.OnGhostEaten += HandleGhostEaten;
            }
        }
        
        /// <summary>
        /// Handle regular pellet collection
        /// </summary>
        private void HandlePelletCollected()
        {
            AddScore(smallPelletPoints);
        }
        
        /// <summary>
        /// Handle power pellet collection
        /// </summary>
        private void HandlePowerPelletCollected()
        {
            AddScore(powerPelletPoints);
            
            // Reset ghost combo when power pellet is collected
            _ghostComboCount = 0;
        }
        
        /// <summary>
        /// Handle ghost eaten by player
        /// </summary>
        private void HandleGhostEaten()
        {
            // Check if combo is still valid
            if (Time.time - _lastGhostEatTime > comboTimeWindow)
            {
                _ghostComboCount = 0;
            }
            
            _ghostComboCount++;
            _lastGhostEatTime = Time.time;
            
            // Calculate points based on combo
            int ghostIndex = Mathf.Min(_ghostComboCount - 1, ghostPoints.Length - 1);
            int points = ghostPoints[ghostIndex];
            
            // Apply combo multiplier if applicable
            if (_ghostComboCount > 1)
            {
                points *= comboMultiplier;
                OnComboAchieved?.Invoke(_ghostComboCount);
            }
            
            AddScore(points);
        }
        
        /// <summary>
        /// Add points to the current score
        /// </summary>
        /// <param name="points">Points to add</param>
        public void AddScore(int points)
        {
            _currentScore += points;
            OnScoreChanged?.Invoke(_currentScore);
            
            // Check for new high score
            if (_currentScore > _highScore)
            {
                _highScore = _currentScore;
                OnHighScoreChanged?.Invoke(_highScore);
                SaveHighScore();
            }
        }
        
        /// <summary>
        /// Reset the current score
        /// </summary>
        public void ResetScore()
        {
            _currentScore = 0;
            _ghostComboCount = 0;
            OnScoreChanged?.Invoke(_currentScore);
        }
        
        /// <summary>
        /// Get the current score
        /// </summary>
        /// <returns>Current score</returns>
        public int GetScore()
        {
            return _currentScore;
        }
        
        /// <summary>
        /// Get the high score
        /// </summary>
        /// <returns>High score</returns>
        public int GetHighScore()
        {
            return _highScore;
        }
        
        /// <summary>
        /// Get the current ghost combo count
        /// </summary>
        /// <returns>Ghost combo count</returns>
        public int GetGhostComboCount()
        {
            return _ghostComboCount;
        }
        
        /// <summary>
        /// Load high score from player prefs
        /// </summary>
        private void LoadHighScore()
        {
            _highScore = PlayerPrefs.GetInt("PacManHighScore", 0);
            OnHighScoreChanged?.Invoke(_highScore);
        }
        
        /// <summary>
        /// Save high score to player prefs
        /// </summary>
        private void SaveHighScore()
        {
            PlayerPrefs.SetInt("PacManHighScore", _highScore);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Reset high score
        /// </summary>
        public void ResetHighScore()
        {
            _highScore = 0;
            PlayerPrefs.SetInt("PacManHighScore", _highScore);
            PlayerPrefs.Save();
            OnHighScoreChanged?.Invoke(_highScore);
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_playerController != null)
            {
                _playerController.OnPelletCollected -= HandlePelletCollected;
                _playerController.OnPowerPelletCollected -= HandlePowerPelletCollected;
                _playerController.OnGhostEaten -= HandleGhostEaten;
            }
        }
    }
}