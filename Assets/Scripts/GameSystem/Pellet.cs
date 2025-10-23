using UnityEngine;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Pellet component for collectible pellets
    /// </summary>
    public class Pellet : MonoBehaviour
    {
        [Header("Pellet Settings")]
        [SerializeField] private int points = 10;
        [SerializeField] private bool isPowerPellet = false;
        [SerializeField] private float powerDuration = 10f;
        [SerializeField] private Color powerPelletColor = Color.blue;
        
        private Renderer _renderer;
        private GameManager _gameManager;
        private PelletManager _pelletManager;
        
        private void Start()
        {
            // Get references
            _renderer = GetComponent<Renderer>();
            _gameManager = GameManager.Instance;
            _pelletManager = FindObjectOfType<PelletManager>();
            
            // Set up the pellet
            SetupPellet();
        }
        
        /// <summary>
        /// Set up the pellet based on its type
        /// </summary>
        private void SetupPellet()
        {
            // Set the tag
            gameObject.tag = isPowerPellet ? "PowerPellet" : "Pellet";
            
            // Visual changes for power pellets
            if (isPowerPellet && _renderer != null)
            {
                _renderer.material.color = powerPelletColor;
            }
        }
        
        /// <summary>
        /// Handle collision with player
        /// </summary>
        /// <param name="other">The collider that hit this pellet</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CollectPellet();
            }
        }
        
        /// <summary>
        /// Collect the pellet
        /// </summary>
        private void CollectPellet()
        {
            // Notify the game systems
            if (_gameManager != null)
            {
                _gameManager.AddScore(points);
            }
            
            if (_pelletManager != null)
            {
                // The pellet manager will handle the specific collection logic
            }
            
            // Trigger visual effects
            TriggerCollectionEffects();
            
            // Destroy the pellet
            Destroy(gameObject);
        }
        
        /// <summary>
        /// Trigger visual effects when collected
        /// </summary>
        private void TriggerCollectionEffects()
        {
            // In a full implementation, we would trigger particle effects or animations
            Debug.Log($"{(isPowerPellet ? "Power pellet" : "Pellet")} collected! +{points} points");
        }
        
        /// <summary>
        /// Get the points value of this pellet
        /// </summary>
        /// <returns>Points value</returns>
        public int GetPoints()
        {
            return points;
        }
        
        /// <summary>
        /// Check if this is a power pellet
        /// </summary>
        /// <returns>True if power pellet</returns>
        public bool IsPowerPellet()
        {
            return isPowerPellet;
        }
        
        /// <summary>
        /// Get the power duration for power pellets
        /// </summary>
        /// <returns>Power duration</returns>
        public float GetPowerDuration()
        {
            return powerDuration;
        }
    }
}