using UnityEngine;
using UnityEngine.SceneManagement;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Manages scene transitions and loading
    /// </summary>
    public class SceneManager : MonoBehaviour
    {
        [Header("Scene Settings")]
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string gameScene = "Game";
        [SerializeField] private string gameOverScene = "GameOver";
        [SerializeField] private float sceneTransitionDelay = 1f;
        
        private GameManager _gameManager;
        private bool _isTransitioning = false;
        
        private void Start()
        {
            // Get reference to game manager
            _gameManager = GameManager.Instance;
            
            // Subscribe to events
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged += HandleGameStateChanged;
            }
        }
        
        /// <summary>
        /// Handle game state changes for scene transitions
        /// </summary>
        /// <param name="newState">New game state</param>
        private void HandleGameStateChanged(GameState newState)
        {
            if (_isTransitioning) return;
            
            switch (newState)
            {
                case GameState.Menu:
                    LoadMainMenu();
                    break;
                case GameState.Playing:
                    LoadGameScene();
                    break;
                case GameState.GameOver:
                    LoadGameOverScene();
                    break;
            }
        }
        
        /// <summary>
        /// Load the main menu scene
        /// </summary>
        public void LoadMainMenu()
        {
            if (_isTransitioning) return;
            
            _isTransitioning = true;
            Debug.Log("Loading main menu scene");
            
            // In a full implementation, we would load the main menu scene
            // For now, we'll just reset the transitioning flag
            Invoke(nameof(ResetTransitioning), sceneTransitionDelay);
        }
        
        /// <summary>
        /// Load the game scene
        /// </summary>
        public void LoadGameScene()
        {
            if (_isTransitioning) return;
            
            _isTransitioning = true;
            Debug.Log("Loading game scene");
            
            // In a full implementation, we would load the game scene
            // For now, we'll just reset the transitioning flag
            Invoke(nameof(ResetTransitioning), sceneTransitionDelay);
        }
        
        /// <summary>
        /// Load the game over scene
        /// </summary>
        public void LoadGameOverScene()
        {
            if (_isTransitioning) return;
            
            _isTransitioning = true;
            Debug.Log("Loading game over scene");
            
            // In a full implementation, we would load the game over scene
            // For now, we'll just reset the transitioning flag
            Invoke(nameof(ResetTransitioning), sceneTransitionDelay);
        }
        
        /// <summary>
        /// Reset the transitioning flag
        /// </summary>
        private void ResetTransitioning()
        {
            _isTransitioning = false;
        }
        
        /// <summary>
        /// Load a scene by name
        /// </summary>
        /// <param name="sceneName">Name of the scene to load</param>
        public void LoadScene(string sceneName)
        {
            if (_isTransitioning) return;
            
            _isTransitioning = true;
            Debug.Log($"Loading scene: {sceneName}");
            
            // In a full implementation, we would load the specified scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        
        /// <summary>
        /// Load a scene by index
        /// </summary>
        /// <param name="sceneIndex">Index of the scene to load</param>
        public void LoadScene(int sceneIndex)
        {
            if (_isTransitioning) return;
            
            _isTransitioning = true;
            Debug.Log($"Loading scene at index: {sceneIndex}");
            
            // In a full implementation, we would load the specified scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }
        
        /// <summary>
        /// Reload the current scene
        /// </summary>
        public void ReloadCurrentScene()
        {
            if (_isTransitioning) return;
            
            _isTransitioning = true;
            Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            Debug.Log($"Reloading scene: {currentScene.name}");
            
            // In a full implementation, we would reload the current scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
        }
        
        /// <summary>
        /// Check if a scene is currently transitioning
        /// </summary>
        /// <returns>True if transitioning</returns>
        public bool IsTransitioning()
        {
            return _isTransitioning;
        }
        
        /// <summary>
        /// Set the scene transition delay
        /// </summary>
        /// <param name="delay">Delay in seconds</param>
        public void SetSceneTransitionDelay(float delay)
        {
            sceneTransitionDelay = delay;
        }
        
        /// <summary>
        /// Get the scene transition delay
        /// </summary>
        /// <returns>Delay in seconds</returns>
        public float GetSceneTransitionDelay()
        {
            return sceneTransitionDelay;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged -= HandleGameStateChanged;
            }
        }
    }
}