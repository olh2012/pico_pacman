using UnityEngine;
using UnityEngine.Rendering;

namespace PacMan.Utils
{
    /// <summary>
    /// Optimizes game performance for PICO VR devices
    /// </summary>
    public class PerformanceOptimizer : MonoBehaviour
    {
        [Header("Target Frame Rate")]
        [SerializeField] private int targetFrameRate = 90;
        [SerializeField] private bool adaptToDeviceInfo = true;
        
        [Header("LOD Settings")]
        [SerializeField] private bool enableLOD = true;
        [SerializeField] private float lodDistance = 10f;
        
        [Header("Occlusion Culling")]
        [SerializeField] private bool enableOcclusionCulling = true;
        
        [Header("Rendering Optimization")]
        [SerializeField] private bool enableDynamicBatching = true;
        [SerializeField] private bool enableGPUInstancing = true;
        [SerializeField] private bool reduceDrawCalls = true;
        
        [Header("Physics Optimization")]
        [SerializeField] private bool optimizePhysics = true;
        [SerializeField] private float fixedTimestep = 0.02f;
        [SerializeField] private int maxPhysicsFrameRate = 60;
        
        [Header("Memory Management")]
        [SerializeField] private bool enableObjectPooling = true;
        [SerializeField] private bool enableGarbageCollection = true;
        [SerializeField] private float gcInterval = 30f;
        
        [Header("Quality Settings")]
        [SerializeField] private bool adjustQualitySettings = true;
        [SerializeField] private int maxLODLevel = 2;
        [SerializeField] private bool disableShadows = false;
        [SerializeField] private bool reduceParticleCount = true;
        
        private float _lastGCTime = 0f;
        private Camera _mainCamera;
        
        private void Start()
        {
            // Get main camera reference
            _mainCamera = Camera.main;
            
            // Apply optimizations
            ApplyPerformanceOptimizations();
        }
        
        /// <summary>
        /// Apply all performance optimizations
        /// </summary>
        public void ApplyPerformanceOptimizations()
        {
            SetTargetFrameRate();
            OptimizeRendering();
            OptimizePhysics();
            OptimizeQualitySettings();
            SetupObjectPooling();
        }
        
        /// <summary>
        /// Set target frame rate based on device
        /// </summary>
        private void SetTargetFrameRate()
        {
            if (adaptToDeviceInfo)
            {
                // Detect PICO device model and set appropriate frame rate
                string deviceModel = SystemInfo.deviceModel;
                if (deviceModel.Contains("PICO 4"))
                {
                    targetFrameRate = 90;
                }
                else if (deviceModel.Contains("PICO 3"))
                {
                    targetFrameRate = 90;
                }
                else
                {
                    // Default to 72fps for other devices
                    targetFrameRate = 72;
                }
            }
            
            // Set target frame rate
            Application.targetFrameRate = targetFrameRate;
            
            // For VR, we also need to set vSyncCount to 0
            QualitySettings.vSyncCount = 0;
        }
        
        /// <summary>
        /// Optimize rendering settings
        /// </summary>
        private void OptimizeRendering()
        {
            // Enable dynamic batching
            if (enableDynamicBatching)
            {
                PlayerSettings.graphicsJobs = true;
            }
            
            // Enable GPU instancing
            if (enableGPUInstancing)
            {
                // This would typically be set in materials, but we can enable it globally
                Shader.EnableKeyword("GPU_INSTANCING");
            }
            
            // Reduce draw calls
            if (reduceDrawCalls)
            {
                // Combine meshes where possible
                // This would be done at design time rather than runtime
            }
            
            // Set up LOD system
            if (enableLOD)
            {
                SetupLODSystem();
            }
            
            // Enable occlusion culling
            if (enableOcclusionCulling)
            {
                // This is typically set up in the editor, but we can ensure it's enabled
                Camera.main.useOcclusionCulling = true;
            }
        }
        
        /// <summary>
        /// Set up LOD system for performance optimization
        /// </summary>
        private void SetupLODSystem()
        {
            // Find all renderers in the scene
            Renderer[] renderers = FindObjectsOfType<Renderer>();
            
            foreach (Renderer renderer in renderers)
            {
                // Skip UI elements and other special renderers
                if (renderer.GetComponent<CanvasRenderer>() != null)
                    continue;
                
                // Set up LOD group if not already present
                LODGroup lodGroup = renderer.GetComponentInParent<LODGroup>();
                if (lodGroup == null)
                {
                    // Create LOD group for optimization
                    // In a full implementation, this would be set up with proper LOD levels
                }
            }
        }
        
        /// <summary>
        /// Optimize physics settings
        /// </summary>
        private void OptimizePhysics()
        {
            if (optimizePhysics)
            {
                // Set fixed timestep for physics
                Time.fixedDeltaTime = fixedTimestep;
                
                // Limit physics frame rate
                Application.targetFrameRate = Mathf.Min(targetFrameRate, maxPhysicsFrameRate);
            }
        }
        
        /// <summary>
        /// Optimize quality settings for VR
        /// </summary>
        private void OptimizeQualitySettings()
        {
            if (adjustQualitySettings)
            {
                // Set maximum LOD level
                QualitySettings.maximumLODLevel = maxLODLevel;
                
                // Disable shadows for performance
                if (disableShadows)
                {
                    QualitySettings.shadows = ShadowQuality.Disable;
                }
                
                // Reduce particle count
                if (reduceParticleCount)
                {
                    QualitySettings.particleRaycastBudget = 64;
                }
                
                // Optimize texture quality
                QualitySettings.masterTextureLimit = 1;
                
                // Reduce anti-aliasing
                QualitySettings.antiAliasing = 2;
            }
        }
        
        /// <summary>
        /// Set up object pooling system
        /// </summary>
        private void SetupObjectPooling()
        {
            if (enableObjectPooling)
            {
                // In a full implementation, this would set up object pools for:
                // - Pellets
                // - Particle effects
                // - UI elements
                // - Ghosts (if dynamically created/destroyed)
            }
        }
        
        /// <summary>
        /// Perform garbage collection at intervals
        /// </summary>
        private void Update()
        {
            if (enableGarbageCollection)
            {
                _lastGCTime += Time.deltaTime;
                if (_lastGCTime >= gcInterval)
                {
                    System.GC.Collect();
                    _lastGCTime = 0f;
                }
            }
        }
        
        /// <summary>
        /// Force garbage collection
        /// </summary>
        public void ForceGarbageCollection()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
        }
        
        /// <summary>
        /// Set target frame rate
        /// </summary>
        /// <param name="frameRate">Target frame rate</param>
        public void SetTargetFrameRate(int frameRate)
        {
            targetFrameRate = frameRate;
            Application.targetFrameRate = targetFrameRate;
        }
        
        /// <summary>
        /// Get current target frame rate
        /// </summary>
        /// <returns>Target frame rate</returns>
        public int GetTargetFrameRate()
        {
            return targetFrameRate;
        }
        
        /// <summary>
        /// Enable or disable LOD system
        /// </summary>
        /// <param name="enable">True to enable LOD</param>
        public void SetLODEnabled(bool enable)
        {
            enableLOD = enable;
            if (enable)
            {
                SetupLODSystem();
            }
        }
        
        /// <summary>
        /// Get whether LOD system is enabled
        /// </summary>
        /// <returns>True if LOD is enabled</returns>
        public bool IsLODEnabled()
        {
            return enableLOD;
        }
    }
}