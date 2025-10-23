using UnityEngine;
using PacMan.GameSystem;

namespace PacMan.VisualEffects
{
    /// <summary>
    /// Manages visual effects and feedback for game events
    /// </summary>
    public class VisualEffectsManager : MonoBehaviour
    {
        [Header("Particle Effects")]
        [SerializeField] private ParticleSystem pelletCollectionEffect;
        [SerializeField] private ParticleSystem powerPelletEffect;
        [SerializeField] private ParticleSystem ghostEatenEffect;
        [SerializeField] private ParticleSystem playerDeathEffect;
        [SerializeField] private ParticleSystem levelCompleteEffect;
        
        [Header("Screen Effects")]
        [SerializeField] private Animator screenEffectAnimator;
        [SerializeField] private string powerUpTrigger = "PowerUp";
        [SerializeField] private string ghostEatenTrigger = "GhostEaten";
        [SerializeField] private string playerDeathTrigger = "PlayerDeath";
        [SerializeField] private string levelCompleteTrigger = "LevelComplete";
        
        [Header("Color Effects")]
        [SerializeField] private Material playerMaterial;
        [SerializeField] private Color normalColor = Color.yellow;
        [SerializeField] private Color powerUpColor = Color.blue;
        [SerializeField] private Color ghostEatenColor = Color.green;
        [SerializeField] private float colorTransitionTime = 0.5f;
        
        [Header("Camera Effects")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float screenShakeIntensity = 0.1f;
        [parameter=end_line>
        [SerializeField] private float screenShakeDuration = 0.2f;
        
        private GameManager _gameManager;
        private PlayerController _playerController;
        private bool _isPowerUpActive = false;
        private float _powerUpTimer = 0f;
        
        private void Start()
        {
            // Get references to game systems
            _gameManager = GameManager.Instance;
            _playerController = FindObjectOfType<PlayerController>();
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            
            // Subscribe to events
            SubscribeToEvents();
        }
        
        /// <summary>
        /// Subscribe to all relevant events
        /// </summary>
        private void SubscribeToEvents()
        {
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged += HandleGameStateChanged;
            }
            
            if (_playerController != null)
            {
                _playerController.OnPelletCollected += PlayPelletCollectionEffect;
                _playerController.OnPowerPelletCollected += PlayPowerPelletEffect;
                _playerController.OnGhostEaten += PlayGhostEatenEffect;
                _playerController.OnPlayerCaught += PlayPlayerDeathEffect;
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
        /// Play pellet collection effect
        /// </summary>
        private void PlayPelletCollectionEffect()
        {
            if (pelletCollectionEffect != null)
            {
                pelletCollectionEffect.Play();
            }
            
            // Apply screen shake for feedback
            ApplyScreenShake(screenShakeIntensity * 0.5f, screenShakeDuration * 0.5f);
        }
        
        /// <summary>
        /// Play power pellet effect
        /// </summary>
        private void PlayPowerPelletEffect()
        {
            if (powerPelletEffect != null)
            {
                powerPelletEffect.Play();
            }
            
            // Trigger screen effect
            if (screenEffectAnimator != null)
            {
                screenEffectAnimator.SetTrigger(powerUpTrigger);
            }
            
            // Change player color
            ChangePlayerColor(powerUpColor);
            _isPowerUpActive = true;
            
            // Apply screen shake for feedback
            ApplyScreenShake(screenShakeIntensity, screenShakeDuration);
        }
        
        /// <summary>
        /// Play ghost eaten effect
        /// </summary>
        private void PlayGhostEatenEffect()
        {
            if (ghostEatenEffect != null)
            {
                ghostEatenEffect.Play();
            }
            
            // Trigger screen effect
            if (screenEffectAnimator != null)
            {
                screenEffectAnimator.SetTrigger(ghostEatenTrigger);
            }
            
            // Change player color temporarily
            ChangePlayerColor(ghostEatenColor);
            Invoke(nameof(ResetPlayerColor), colorTransitionTime * 2);
            
            // Apply strong screen shake for feedback
            ApplyScreenShake(screenShakeIntensity * 2, screenShakeDuration * 2);
        }
        
        /// <summary>
        /// Play player death effect
        /// </summary>
        private void PlayPlayerDeathEffect()
        {
            if (playerDeathEffect != null)
            {
                playerDeathEffect.Play();
            }
            
            // Trigger screen effect
            if (screenEffectAnimator != null)
            {
                screenEffectAnimator.SetTrigger(playerDeathTrigger);
            }
            
            // Apply strong screen shake for feedback
            ApplyScreenShake(screenShakeIntensity * 3, screenShakeDuration * 3);
        }
        
        /// <summary>
        /// Play level complete effect
        /// </summary>
        private void PlayLevelCompleteEffect()
        {
            if (levelCompleteEffect != null)
            {
                levelCompleteEffect.Play();
            }
            
            // Trigger screen effect
            if (screenEffectAnimator != null)
            {
                screenEffectAnimator.SetTrigger(levelCompleteTrigger);
            }
            
            // Apply screen shake for feedback
            ApplyScreenShake(screenShakeIntensity * 1.5f, screenShakeDuration * 1.5f);
        }
        
        /// <summary>
        /// Change player color with transition
        /// </summary>
        /// <param name="newColor">New color to apply</param>
        private void ChangePlayerColor(Color newColor)
        {
            if (playerMaterial != null)
            {
                playerMaterial.color = newColor;
            }
        }
        
        /// <summary>
        /// Reset player color to normal
        /// </summary>
        private void ResetPlayerColor()
        {
            if (playerMaterial != null && !_isPowerUpActive)
            {
                playerMaterial.color = normalColor;
            }
        }
        
        /// <summary>
        /// Apply screen shake effect
        /// </summary>
        /// <param name="intensity">Shake intensity</param>
        /// <param name="duration">Shake duration</param>
        private void ApplyScreenShake(float intensity, float duration)
        {
            if (mainCamera != null)
            {
                StartCoroutine(ScreenShake(intensity, duration));
            }
        }
        
        /// <summary>
        /// Screen shake coroutine
        /// </summary>
        /// <param name="intensity">Shake intensity</param>
        /// <param name="duration">Shake duration</param>
        /// <returns>IEnumerator for coroutine</returns>
        private System.Collections.IEnumerator ScreenShake(float intensity, float duration)
        {
            Vector3 originalPosition = mainCamera.transform.localPosition;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * intensity;
                float y = Random.Range(-1f, 1f) * intensity;
                
                mainCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0);
                
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            mainCamera.transform.localPosition = originalPosition;
        }
        
        /// <summary>
        /// Update power-up timer and reset player color when it expires
        /// </summary>
        private void Update()
        {
            if (_isPowerUpActive)
            {
                _powerUpTimer += Time.deltaTime;
                // In a full implementation, this would be linked to the actual power-up duration
                // For now, we'll just reset after 10 seconds
                if (_powerUpTimer >= 10f)
                {
                    _isPowerUpActive = false;
                    _powerUpTimer = 0f;
                    ResetPlayerColor();
                }
            }
        }
        
        /// <summary>
        /// Set whether power-up effect is active
        /// </summary>
        /// <param name="active">True if power-up is active</param>
        public void SetPowerUpActive(bool active)
        {
            _isPowerUpActive = active;
            if (!active)
            {
                _powerUpTimer = 0f;
                ResetPlayerColor();
            }
        }
        
        /// <summary>
        /// Get whether power-up effect is active
        /// </summary>
        /// <returns>True if power-up is active</returns>
        public bool IsPowerUpActive()
        {
            return _isPowerUpActive;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged -= HandleGameStateChanged;
            }
            
            if (_playerController != null)
            {
                _playerController.OnPelletCollected -= PlayPelletCollectionEffect;
                _playerController.OnPowerPelletCollected -= PlayPowerPelletEffect;
                _playerController.OnGhostEaten -= PlayGhostEatenEffect;
                _playerController.OnPlayerCaught -= PlayPlayerDeathEffect;
            }
        }
    }
}