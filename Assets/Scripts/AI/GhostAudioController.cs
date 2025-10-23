using UnityEngine;
using PacMan.Audio;

namespace PacMan.AI
{
    /// <summary>
    /// Controls audio for individual ghosts with spatial audio support
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class GhostAudioController : MonoBehaviour
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioClip ghostMoveSound;
        [SerializeField] private AudioClip ghostFrightenedSound;
        [SerializeField] private AudioClip ghostEatenSound;
        [SerializeField] private AudioClip ghostRespawnSound;
        
        [Header("Audio Settings")]
        [SerializeField] private bool enableGhostSounds = true;
        [SerializeField] private float moveSoundInterval = 2.0f;
        [SerializeField] private float moveSoundVolume = 0.3f;
        [SerializeField] private float frightenedSoundVolume = 0.5f;
        [SerializeField] private float eatenSoundVolume = 0.7f;
        [SerializeField] private float respawnSoundVolume = 0.6f;
        
        [Header("Spatial Audio Settings")]
        [SerializeField] private bool useSpatialAudio = true;
        [SerializeField] private float dopplerLevel = 1.0f;
        [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        [SerializeField] private float minDistance = 1.0f;
        [SerializeField] private float maxDistance = 50.0f;
        
        private AudioSource _audioSource;
        private Ghost _ghost;
        private AudioManager _audioManager;
        private float _moveSoundTimer = 0f;
        private bool _isFrightened = false;
        
        private void Start()
        {
            // Get components
            _audioSource = GetComponent<AudioSource>();
            _ghost = GetComponent<Ghost>();
            
            // Find AudioManager in the scene
            _audioManager = FindObjectOfType<AudioManager>();
            
            // Initialize audio source
            InitializeAudioSource();
            
            // Subscribe to ghost events
            if (_ghost != null)
            {
                _ghost.OnGhostEaten += PlayEatenSound;
            }
        }
        
        private void Update()
        {
            if (!enableGhostSounds) return;
            
            // Handle move sounds
            HandleMoveSounds();
        }
        
        /// <summary>
        /// Initialize audio source with proper settings
        /// </summary>
        private void InitializeAudioSource()
        {
            if (_audioSource == null) return;
            
            // Configure spatial audio
            _audioSource.spatialBlend = useSpatialAudio ? 1.0f : 0.0f;
            _audioSource.dopplerLevel = dopplerLevel;
            _audioSource.rolloffMode = rolloffMode;
            _audioSource.minDistance = minDistance;
            _audioSource.maxDistance = maxDistance;
            
            // Set initial volume
            _audioSource.volume = moveSoundVolume;
        }
        
        /// <summary>
        /// Handle ghost movement sounds
        /// </summary>
        private void HandleMoveSounds()
        {
            if (_audioSource == null || ghostMoveSound == null) return;
            
            // Update move sound timer
            _moveSoundTimer += Time.deltaTime;
            
            // Play move sound at intervals
            if (_moveSoundTimer >= moveSoundInterval)
            {
                // Play move sound based on ghost state
                if (_isFrightened && ghostFrightenedSound != null)
                {
                    _audioSource.PlayOneShot(ghostFrightenedSound, frightenedSoundVolume);
                }
                else if (ghostMoveSound != null)
                {
                    _audioSource.PlayOneShot(ghostMoveSound, moveSoundVolume);
                }
                
                _moveSoundTimer = 0f;
            }
        }
        
        /// <summary>
        /// Play sound when ghost is eaten
        /// </summary>
        private void PlayEatenSound()
        {
            if (!enableGhostSounds || _audioSource == null || ghostEatenSound == null) return;
            
            _audioSource.PlayOneShot(ghostEatenSound, eatenSoundVolume);
        }
        
        /// <summary>
        /// Play sound when ghost respawns
        /// </summary>
        public void PlayRespawnSound()
        {
            if (!enableGhostSounds || _audioSource == null || ghostRespawnSound == null) return;
            
            _audioSource.PlayOneShot(ghostRespawnSound, respawnSoundVolume);
        }
        
        /// <summary>
        /// Set ghost frightened state for audio
        /// </summary>
        /// <param name="frightened">True if ghost is frightened</param>
        public void SetFrightened(bool frightened)
        {
            _isFrightened = frightened;
        }
        
        /// <summary>
        /// Set whether ghost sounds are enabled
        /// </summary>
        /// <param name="enabled">True to enable sounds</param>
        public void SetSoundsEnabled(bool enabled)
        {
            enableGhostSounds = enabled;
        }
        
        /// <summary>
        /// Get whether ghost sounds are enabled
        /// </summary>
        /// <returns>True if sounds are enabled</returns>
        public bool AreSoundsEnabled()
        {
            return enableGhostSounds;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            if (_ghost != null)
            {
                _ghost.OnGhostEaten -= PlayEatenSound;
            }
        }
    }
}