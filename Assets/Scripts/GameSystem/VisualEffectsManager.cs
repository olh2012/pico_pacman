using UnityEngine;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Manages visual effects for game events
    /// </summary>
    public class VisualEffectsManager : MonoBehaviour
    {
        [Header("Effect Prefabs")]
        [SerializeField] private GameObject pelletCollectionEffect;
        [SerializeField] private GameObject powerPelletCollectionEffect;
        [SerializeField] private GameObject ghostEatenEffect;
        [SerializeField] private GameObject playerDeathEffect;
        [SerializeField] private GameObject levelCompleteEffect;
        
        [Header("Effect Settings")]
        [SerializeField] private float effectDuration = 2f;
        [SerializeField] private bool useParticleEffects = true;
        
        private GameManager _gameManager;
        private Player.PlayerController _playerController;
        
        private void Start()
        {
            // Get references
            _gameManager = GameManager.Instance;
            _playerController = FindObjectOfType<Player.PlayerController>();
            
            // Subscribe to events
            SubscribeToEvents();
        }
        
        /// <summary>
        /// Subscribe to all relevant events
        /// </summary>
        private void SubscribeToEvents()
        {
            if (_playerController != null)
            {
                _playerController.OnPelletCollected += PlayPelletCollectionEffect;
                _playerController.OnPowerPelletCollected += PlayPowerPelletCollectionEffect;
                _playerController.OnGhostEaten += PlayGhostEatenEffect;
                _playerController.OnPlayerCaught += PlayPlayerDeathEffect;
            }
            
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged += HandleGameStateChanged;
            }
        }
        
        /// <summary>
        /// Play pellet collection effect
        /// </summary>
        private void PlayPelletCollectionEffect()
        {
            if (pelletCollectionEffect != null && useParticleEffects)
            {
                // In a full implementation, we would play the effect at the pellet position
                Debug.Log("Playing pellet collection effect");
            }
        }
        
        /// <summary>
        /// Play power pellet collection effect
        /// </summary>
        private void PlayPowerPelletCollectionEffect()
        {
            if (powerPelletCollectionEffect != null && useParticleEffects)
            {
                // In a full implementation, we would play the effect at the pellet position
                Debug.Log("Playing power pellet collection effect");
            }
        }
        
        /// <summary>
        /// Play ghost eaten effect
        /// </summary>
        private void PlayGhostEatenEffect()
        {
            if (ghostEatenEffect != null && useParticleEffects)
            {
                // In a full implementation, we would play the effect at the ghost position
                Debug.Log("Playing ghost eaten effect");
            }
        }
        
        /// <summary>
        /// Play player death effect
        /// </summary>
        private void PlayPlayerDeathEffect()
        {
            if (playerDeathEffect != null && useParticleEffects)
            {
                // In a full implementation, we would play the effect at the player position
                Debug.Log("Playing player death effect");
            }
        }
        
        /// <summary>
        /// Play level complete effect
        /// </summary>
        private void PlayLevelCompleteEffect()
        {
            if (levelCompleteEffect != null && useParticleEffects)
            {
                // In a full implementation, we would play the effect
                Debug.Log("Playing level complete effect");
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
                case GameState.LevelComplete:
                    PlayLevelCompleteEffect();
                    break;
            }
        }
        
        /// <summary>
        /// Play a visual effect at a specific position
        /// </summary>
        /// <param name="effectPrefab">Effect prefab to instantiate</param>
        /// <param name="position">Position to play the effect</param>
        /// <param name="rotation">Rotation of the effect</param>
        public void PlayEffectAtPosition(GameObject effectPrefab, Vector3 position, Quaternion rotation)
        {
            if (effectPrefab != null && useParticleEffects)
            {
                GameObject effect = Instantiate(effectPrefab, position, rotation);
                // Destroy the effect after its duration
                Destroy(effect, effectDuration);
            }
        }
        
        /// <summary>
        /// Play a visual effect at a specific position with default rotation
        /// </summary>
        /// <param name="effectPrefab">Effect prefab to instantiate</param>
        /// <param name="position">Position to play the effect</param>
        public void PlayEffectAtPosition(GameObject effectPrefab, Vector3 position)
        {
            PlayEffectAtPosition(effectPrefab, position, Quaternion.identity);
        }
        
        /// <summary>
        /// Set whether to use particle effects
        /// </summary>
        /// <param name="useEffects">True to use particle effects</param>
        public void SetUseParticleEffects(bool useEffects)
        {
            useParticleEffects = useEffects;
        }
        
        /// <summary>
        /// Get whether particle effects are enabled
        /// </summary>
        /// <returns>True if particle effects are enabled</returns>
        public bool GetUseParticleEffects()
        {
            return useParticleEffects;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_playerController != null)
            {
                _playerController.OnPelletCollected -= PlayPelletCollectionEffect;
                _playerController.OnPowerPelletCollected -= PlayPowerPelletCollectionEffect;
                _playerController.OnGhostEaten -= PlayGhostEatenEffect;
                _playerController.OnPlayerCaught -= PlayPlayerDeathEffect;
            }
            
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged -= HandleGameStateChanged;
            }
        }
    }
}