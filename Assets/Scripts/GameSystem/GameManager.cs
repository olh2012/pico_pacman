using UnityEngine;
using PacMan.Utils;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Game states for the Pac-Man game
    /// </summary>
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver,
        LevelComplete
    }

    /// <summary>
    /// Main game manager that controls the overall game flow
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Header("Game Settings")]
        [SerializeField] private int initialLives = 3;
        [SerializeField] private int currentLevel = 1;
        
        [Header("Game State")]
        [SerializeField] private GameState currentState = GameState.Menu;
        [SerializeField] private int currentScore = 0;
        [SerializeField] private int highScore = 0;
        [SerializeField] private int remainingLives = 0;
        
        // Events
        public System.Action<GameState> OnGameStateChanged;
        public System.Action<int> OnScoreChanged;
        public System.Action<int> OnLivesChanged;
        public System.Action<int> OnLevelChanged;
        
        protected override void Awake()
        {
            base.Awake();
            InitializeGame();
        }
        
        /// <summary>
        /// Initialize the game settings
        /// </summary>
        private void InitializeGame()
        {
            remainingLives = initialLives;
            currentScore = 0;
            currentLevel = 1;
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            
            Debug.Log("Game Manager initialized");
        }
        
        /// <summary>
        /// Start a new game
        /// </summary>
        public void StartNewGame()
        {
            currentScore = 0;
            remainingLives = initialLives;
            currentLevel = 1;
            
            ChangeGameState(GameState.Playing);
            
            OnScoreChanged?.Invoke(currentScore);
            OnLivesChanged?.Invoke(remainingLives);
            OnLevelChanged?.Invoke(currentLevel);
            
            Debug.Log("New game started");
        }
        
        /// <summary>
        /// Change the current game state
        /// </summary>
        /// <param name="newState">The new game state</param>
        public void ChangeGameState(GameState newState)
        {
            if (currentState == newState) return;
            
            GameState previousState = currentState;
            currentState = newState;
            
            HandleGameStateChange(previousState, newState);
            
            OnGameStateChanged?.Invoke(newState);
            
            Debug.Log($"Game state changed from {previousState} to {newState}");
        }
        
        /// <summary>
        /// Handle logic when game state changes
        /// </summary>
        /// <param name="previousState">Previous game state</param>
        /// <param name="newState">New game state</param>
        private void HandleGameStateChange(GameState previousState, GameState newState)
        {
            switch (newState)
            {
                case GameState.Menu:
                    // Return to main menu
                    break;
                case GameState.Playing:
                    // Resume or start gameplay
                    Time.timeScale = 1f;
                    break;
                case GameState.Paused:
                    // Pause the game
                    Time.timeScale = 0f;
                    break;
                case GameState.GameOver:
                    // Handle game over
                    Time.timeScale = 1f;
                    CheckHighScore();
                    break;
                case GameState.LevelComplete:
                    // Handle level completion
                    currentLevel++;
                    OnLevelChanged?.Invoke(currentLevel);
                    break;
            }
        }
        
        /// <summary>
        /// Add points to the player's score
        /// </summary>
        /// <param name="points">Points to add</param>
        public void AddScore(int points)
        {
            currentScore += points;
            OnScoreChanged?.Invoke(currentScore);
            
            Debug.Log($"Score increased by {points}. Total score: {currentScore}");
        }
        
        /// <summary>
        /// Player loses a life
        /// </summary>
        public void LoseLife()
        {
            remainingLives--;
            OnLivesChanged?.Invoke(remainingLives);
            
            if (remainingLives <= 0)
            {
                GameOver();
            }
            
            Debug.Log($"Life lost. Remaining lives: {remainingLives}");
        }
        
        /// <summary>
        /// Player gains an extra life
        /// </summary>
        public void GainLife()
        {
            remainingLives++;
            OnLivesChanged?.Invoke(remainingLives);
            
            Debug.Log($"Extra life gained. Remaining lives: {remainingLives}");
        }
        
        /// <summary>
        /// End the current game
        /// </summary>
        public void GameOver()
        {
            ChangeGameState(GameState.GameOver);
            Debug.Log("Game Over!");
        }
        
        /// <summary>
        /// Check if the current score is a new high score
        /// </summary>
        private void CheckHighScore()
        {
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
                Debug.Log($"New high score: {highScore}");
            }
        }
        
        /// <summary>
        /// Get the current game state
        /// </summary>
        /// <returns>Current game state</returns>
        public GameState GetGameState()
        {
            return currentState;
        }
        
        /// <summary>
        /// Get the current score
        /// </summary>
        /// <returns>Current score</returns>
        public int GetScore()
        {
            return currentScore;
        }
        
        /// <summary>
        /// Get the high score
        /// </summary>
        /// <returns>High score</returns>
        public int GetHighScore()
        {
            return highScore;
        }
        
        /// <summary>
        /// Get the remaining lives
        /// </summary>
        /// <returns>Remaining lives</returns>
        public int GetRemainingLives()
        {
            return remainingLives;
        }
        
        /// <summary>
        /// Get the current level
        /// </summary>
        /// <returns>Current level</returns>
        public int GetLevel()
        {
            return currentLevel;
        }
    }
}