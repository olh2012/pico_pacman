using UnityEngine;
using UnityEngine.Audio;
using PacMan.GameSystem;

namespace PacMan.Audio
{
    /// <summary>
    /// Manages all audio in the game including sound effects and music
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource ambientSource;
        
        [Header("Audio Clips")]
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private AudioClip pelletCollectionSound;
        [SerializeField] private AudioClip powerPelletSound;
        [SerializeField] private AudioClip ghostEatenSound;
        [SerializeField] private AudioClip playerDeathSound;
        [SerializeField] private AudioClip ghostChaseSound;
        [SerializeField] private AudioClip ghostFrightenedSound;
        [SerializeField] private AudioClip levelCompleteSound;
        [SerializeField] private AudioClip menuNavigationSound;
        
        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private string masterVolumeParameter = "MasterVolume";
        [SerializeField] private string musicVolumeParameter = "MusicVolume";
        [SerializeField] private string sfxVolumeParameter = "SFXVolume";
        
        [Header("Spatial Audio Settings")]
        [SerializeField] private bool useSpatialAudio = true;
        [SerializeField] private float dopplerLevel = 1.0f;
        [SerializeField] private AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        [SerializeField] private float minDistance = 1.0f;
        [SerializeField] private float maxDistance = 50.0f;
        
        private GameManager _gameManager;
        private bool _isMusicPlaying = false;
        
        private void Start()
        {
            // Get references to game systems
            _gameManager = GameManager.Instance;
            
            // Subscribe to events
            if (_gameManager != null)
            {
                _gameManager.OnGameStateChanged += HandleGameStateChanged;
            }
            
            // Initialize audio sources
            InitializeAudioSources();
        }
        
        /// <summary>
        /// Initialize audio sources with proper settings
        /// </summary>
        private void InitializeAudioSources()
        {
            if (musicSource != null)
            {
                musicSource.loop = true;
                musicSource.spatialBlend = useSpatialAudio ? 1.0f : 0.0f;
                musicSource.dopplerLevel = dopplerLevel;
                musicSource.rolloffMode = rolloffMode;
                musicSource.minDistance = minDistance;
                musicSource.maxDistance = maxDistance;
            }
            
            if (sfxSource != null)
            {
                sfxSource.loop = false;
                sfxSource.spatialBlend = useSpatialAudio ? 1.0f : 0.0f;
                sfxSource.dopplerLevel = dopplerLevel;
                sfxSource.rolloffMode = rolloffMode;
                sfxSource.minDistance = minDistance;
                sfxSource.maxDistance = maxDistance;
            }
            
            if (ambientSource != null)
            {
                ambientSource.loop = true;
                ambientSource.spatialBlend = useSpatialAudio ? 1.0f : 0.0f;
                ambientSource.dopplerLevel = dopplerLevel;
                ambientSource.rolloffMode = rolloffMode;
                ambientSource.minDistance = minDistance;
                ambientSource.maxDistance = maxDistance;
            }
        }
        
        /// <summary>
        /// Handle game state changes for audio
        /// </summary>
        /// <param name="newState">New game state</param>
        private void HandleGameStateChanged(GameState newState)
        {
            switch (newState)
            {
                case GameState.Menu:
                    PlayMenuMusic();
                    break;
                case GameState.Playing:
                    PlayGameMusic();
                    break;
                case GameState.Paused:
                    PauseMusic();
                    break;
                case GameState.GameOver:
                    PlayGameOverMusic();
                    break;
                case GameState.LevelComplete:
                    PlayLevelCompleteSound();
                    break;
            }
        }
        
        /// <summary>
        /// Play menu music
        /// </summary>
        private void PlayMenuMusic()
        {
            if (musicSource != null && backgroundMusic != null)
            {
                musicSource.clip = backgroundMusic;
                musicSource.Play();
                _isMusicPlaying = true;
            }
        }
        
        /// <summary>
        /// Play game music
        /// </summary>
        private void PlayGameMusic()
        {
            if (musicSource != null && backgroundMusic != null)
            {
                if (!_isMusicPlaying)
                {
                    musicSource.clip = backgroundMusic;
                    musicSource.Play();
                    _isMusicPlaying = true;
                }
                else
                {
                    musicSource.UnPause();
                }
            }
        }
        
        /// <summary>
        /// Pause music
        /// </summary>
        private void PauseMusic()
        {
            if (musicSource != null && _isMusicPlaying)
            {
                musicSource.Pause();
            }
        }
        
        /// <summary>
        /// Play game over music
        /// </summary>
        private void PlayGameOverMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
                _isMusicPlaying = false;
                
                if (playerDeathSound != null)
                {
                    musicSource.PlayOneShot(playerDeathSound);
                }
            }
        }
        
        /// <summary>
        /// Play level complete sound
        /// </summary>
        private void PlayLevelCompleteSound()
        {
            if (sfxSource != null && levelCompleteSound != null)
            {
                sfxSource.PlayOneShot(levelCompleteSound);
            }
        }
        
        /// <summary>
        /// Play pellet collection sound
        /// </summary>
        public void PlayPelletCollectionSound()
        {
            if (sfxSource != null && pelletCollectionSound != null)
            {
                sfxSource.PlayOneShot(pelletCollectionSound);
            }
        }
        
        /// <summary>
        /// Play power pellet sound
        /// </summary>
        public void PlayPowerPelletSound()
        {
            if (sfxSource != null && powerPelletSound != null)
            {
                sfxSource.PlayOneShot(powerPelletSound);
            }
        }
        
        /// <summary>
        /// Play ghost eaten sound
        /// </summary>
        public void PlayGhostEatenSound()
        {
            if (sfxSource != null && ghostEatenSound != null)
            {
                sfxSource.PlayOneShot(ghostEatenSound);
            }
        }
        
        /// <summary>
        /// Play player death sound
        /// </summary>
        public void PlayPlayerDeathSound()
        {
            if (sfxSource != null && playerDeathSound != null)
            {
                sfxSource.PlayOneShot(playerDeathSound);
            }
        }
        
        /// <summary>
        /// Play ghost chase sound
        /// </summary>
        public void PlayGhostChaseSound()
        {
            if (ambientSource != null && ghostChaseSound != null)
            {
                ambientSource.PlayOneShot(ghostChaseSound);
            }
        }
        
        /// <summary>
        /// Play ghost frightened sound
        /// </summary>
        public void PlayGhostFrightenedSound()
        {
            if (ambientSource != null && ghostFrightenedSound != null)
            {
                ambientSource.PlayOneShot(ghostFrightenedSound);
            }
        }
        
        /// <summary>
        /// Play menu navigation sound
        /// </summary>
        public void PlayMenuNavigationSound()
        {
            if (sfxSource != null && menuNavigationSound != null)
            {
                sfxSource.PlayOneShot(menuNavigationSound);
            }
        }
        
        /// <summary>
        /// Set master volume
        /// </summary>
        /// <param name="volume">Volume level (0.0 to 1.0)</param>
        public void SetMasterVolume(float volume)
        {
            if (audioMixer != null)
            {
                float decibelVolume = Mathf.Log10(volume) * 20;
                audioMixer.SetFloat(masterVolumeParameter, decibelVolume);
            }
        }
        
        /// <summary>
        /// Set music volume
        /// </summary>
        /// <param name="volume">Volume level (0.0 to 1.0)</param>
        public void SetMusicVolume(float volume)
        {
            if (audioMixer != null)
            {
                float decibelVolume = Mathf.Log10(volume) * 20;
                audioMixer.SetFloat(musicVolumeParameter, decibelVolume);
            }
        }
        
        /// <summary>
        /// Set SFX volume
        /// </summary>
        /// <param name="volume">Volume level (0.0 to 1.0)</param>
        public void SetSFXVolume(float volume)
        {
            if (audioMixer != null)
            {
                float decibelVolume = Mathf.Log10(volume) * 20;
                audioMixer.SetFloat(sfxVolumeParameter, decibelVolume);
            }
        }
        
        /// <summary>
        /// Get whether music is currently playing
        /// </summary>
        /// <returns>True if music is playing</returns>
        public bool IsMusicPlaying()
        {
            return _isMusicPlaying;
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