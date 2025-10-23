using UnityEngine;
using System.Collections.Generic;
using PacMan.Player;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Manages all pellets in the game, including collection and scoring
    /// </summary>
    public class PelletManager : MonoBehaviour
    {
        [Header("Pellet Settings")]
        [SerializeField] private int smallPelletPoints = 10;
        [SerializeField] private int powerPelletPoints = 50;
        [SerializeField] private float powerPelletDuration = 10f;
        
        [Header("Pellet References")]
        [SerializeField] private GameObject smallPelletPrefab;
        [SerializeField] private GameObject powerPelletPrefab;
        
        private List<GameObject> _pellets = new List<GameObject>();
        private int _totalPellets = 0;
        private int _collectedPellets = 0;
        private GameManager _gameManager;
        private PlayerController _playerController;
        private GhostManager _ghostManager;
        
        // Events
        public System.Action<int> OnPelletCollected;
        public System.Action OnAllPelletsCollected;
        public System.Action OnPowerPelletCollected;
        
        private void Start()
        {
            // Get references to game systems
            _gameManager = GameManager.Instance;
            _playerController = FindObjectOfType<PlayerController>();
            _ghostManager = FindObjectOfType<GhostManager>();
            
            // Subscribe to events
            if (_playerController != null)
            {
                _playerController.OnPelletCollected += HandlePelletCollected;
                _playerController.OnPowerPelletCollected += HandlePowerPelletCollected;
            }
        }
        
        /// <summary>
        /// Initialize the pellet manager with a list of pellet positions
        /// </summary>
        /// <param name="pelletPositions">List of pellet positions</param>
        /// <param name="powerPelletPositions">List of power pellet positions</param>
        public void InitializePellets(List<Vector3> pelletPositions, List<Vector3> powerPelletPositions)
        {
            _pellets.Clear();
            _totalPellets = 0;
            _collectedPellets = 0;
            
            // Create small pellets
            if (smallPelletPrefab != null && pelletPositions != null)
            {
                foreach (Vector3 position in pelletPositions)
                {
                    GameObject pellet = Instantiate(smallPelletPrefab, position, Quaternion.identity, transform);
                    pellet.tag = "Pellet";
                    _pellets.Add(pellet);
                    _totalPellets++;
                }
            }
            
            // Create power pellets
            if (powerPelletPrefab != null && powerPelletPositions != null)
            {
                foreach (Vector3 position in powerPelletPositions)
                {
                    GameObject powerPellet = Instantiate(powerPelletPrefab, position, Quaternion.identity, transform);
                    powerPellet.tag = "PowerPellet";
                    _pellets.Add(powerPellet);
                    _totalPellets++;
                }
            }
            
            Debug.Log($"Initialized {_totalPellets} pellets ({pelletPositions?.Count ?? 0} small, {powerPelletPositions?.Count ?? 0} power)");
        }
        
        /// <summary>
        /// Handle regular pellet collection
        /// </summary>
        private void HandlePelletCollected()
        {
            _collectedPellets++;
            
            // Add points to score
            if (_gameManager != null)
            {
                _gameManager.AddScore(smallPelletPoints);
            }
            
            OnPelletCollected?.Invoke(smallPelletPoints);
            
            CheckLevelComplete();
        }
        
        /// <summary>
        /// Handle power pellet collection
        /// </summary>
        private void HandlePowerPelletCollected()
        {
            _collectedPellets++;
            
            // Add points to score
            if (_gameManager != null)
            {
                _gameManager.AddScore(powerPelletPoints);
            }
            
            // Activate power-up mode
            if (_ghostManager != null)
            {
                _ghostManager.SetGhostsToFrightened();
            }
            
            // If we have a player controller, set powered up state
            if (_playerController != null)
            {
                _playerController.SetPoweredUp(true);
            }
            
            OnPowerPelletCollected?.Invoke();
            
            CheckLevelComplete();
        }
        
        /// <summary>
        /// Check if all pellets have been collected
        /// </summary>
        private void CheckLevelComplete()
        {
            if (_collectedPellets >= _totalPellets && _totalPellets > 0)
            {
                OnAllPelletsCollected?.Invoke();
                
                if (_gameManager != null)
                {
                    _gameManager.ChangeGameState(GameState.LevelComplete);
                }
            }
        }
        
        /// <summary>
        /// Get the number of remaining pellets
        /// </summary>
        /// <returns>Number of remaining pellets</returns>
        public int GetRemainingPellets()
        {
            return _totalPellets - _collectedPellets;
        }
        
        /// <summary>
        /// Get the total number of pellets
        /// </summary>
        /// <returns>Total number of pellets</returns>
        public int GetTotalPellets()
        {
            return _totalPellets;
        }
        
        /// <summary>
        /// Get the number of collected pellets
        /// </summary>
        /// <returns>Number of collected pellets</returns>
        public int GetCollectedPellets()
        {
            return _collectedPellets;
        }
        
        /// <summary>
        /// Reset the pellet manager for a new level
        /// </summary>
        public void ResetPellets()
        {
            // Destroy existing pellets
            foreach (GameObject pellet in _pellets)
            {
                if (pellet != null)
                {
                    Destroy(pellet);
                }
            }
            
            _pellets.Clear();
            _collectedPellets = 0;
            _totalPellets = 0;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_playerController != null)
            {
                _playerController.OnPelletCollected -= HandlePelletCollected;
                _playerController.OnPowerPelletCollected -= HandlePowerPelletCollected;
            }
        }
    }
}