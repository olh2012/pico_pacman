using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System.Collections.Generic;

namespace PacMan.Utils
{
    /// <summary>
    /// Verifies device configuration and compatibility for PICO VR devices
    /// </summary>
    public class DeviceVerification : MonoBehaviour
    {
        [Header("Verification Settings")]
        [SerializeField] private bool autoVerifyOnStart = true;
        [SerializeField] private bool showVerificationUI = true;
        [SerializeField] private float verificationInterval = 5f;
        
        [Header("Performance Targets")]
        [SerializeField] private int targetFrameRate = 90;
        [SerializeField] private int minFrameRate = 72;
        [SerializeField] private float maxFrameTime = 1f / 72f; // In seconds
        
        private float _lastVerificationTime = 0f;
        private bool _isVerified = false;
        private string _verificationReport = "";
        private GUIStyle _verificationStyle;
        
        private void Start()
        {
            if (autoVerifyOnStart)
            {
                VerifyDeviceConfiguration();
            }
            
            // Set up verification style for UI
            _verificationStyle = new GUIStyle();
            _verificationStyle.normal.textColor = Color.white;
            _verificationStyle.fontSize = 24;
        }
        
        private void Update()
        {
            // Periodic verification
            if (Time.time - _lastVerificationTime > verificationInterval)
            {
                VerifyDeviceConfiguration();
                _lastVerificationTime = Time.time;
            }
        }
        
        /// <summary>
        /// Verify device configuration and compatibility
        /// </summary>
        public void VerifyDeviceConfiguration()
        {
            System.Text.StringBuilder report = new System.Text.StringBuilder();
            report.AppendLine("=== PICO Device Verification Report ===");
            
            // Device information
            string deviceModel = SystemInfo.deviceModel;
            string deviceName = SystemInfo.deviceName;
            string operatingSystem = SystemInfo.operatingSystem;
            
            report.AppendLine($"Device Model: {deviceModel}");
            report.AppendLine($"Device Name: {deviceName}");
            report.AppendLine($"Operating System: {operatingSystem}");
            
            // Check if device is PICO
            bool isPicoDevice = deviceModel.Contains("PICO") || deviceName.Contains("PICO");
            report.AppendLine($"Is PICO Device: {isPicoDevice}");
            
            // XR system verification
            bool xrSystemActive = VerifyXRSystem();
            report.AppendLine($"XR System Active: {xrSystemActive}");
            
            // Performance verification
            bool performanceOk = VerifyPerformance();
            report.AppendLine($"Performance OK: {performanceOk}");
            
            // Input system verification
            bool inputSystemOk = VerifyInputSystem();
            report.AppendLine($"Input System OK: {inputSystemOk}");
            
            // Audio system verification
            bool audioSystemOk = VerifyAudioSystem();
            report.AppendLine($"Audio System OK: {audioSystemOk}");
            
            // Overall verification result
            _isVerified = isPicoDevice && xrSystemActive && performanceOk && inputSystemOk && audioSystemOk;
            report.AppendLine($"Overall Verification: {_isVerified ? "PASSED" : "FAILED"}");
            
            _verificationReport = report.ToString();
            
            // Log the report
            if (_isVerified)
            {
                Debug.Log(_verificationReport);
            }
            else
            {
                Debug.LogWarning(_verificationReport);
            }
        }
        
        /// <summary>
        /// Verify XR system configuration
        /// </summary>
        /// <returns>True if XR system is properly configured</returns>
        private bool VerifyXRSystem()
        {
            try
            {
                // Check if XR is enabled
                if (!XRSettings.enabled)
                {
                    Debug.LogWarning("XR is not enabled");
                    return false;
                }
                
                // Check loaded XR loaders
                var xrManager = XRGeneralSettings.Instance?.Manager;
                if (xrManager == null)
                {
                    Debug.LogWarning("XR Manager is not available");
                    return false;
                }
                
                var loaders = xrManager.activeLoaders;
                if (loaders == null || loaders.Count == 0)
                {
                    Debug.LogWarning("No XR loaders are active");
                    return false;
                }
                
                // Check for OpenXR loader (required for PICO)
                bool hasOpenXRLoader = false;
                foreach (var loader in loaders)
                {
                    if (loader.name.Contains("OpenXR") || loader.GetType().Name.Contains("OpenXR"))
                    {
                        hasOpenXRLoader = true;
                        break;
                    }
                }
                
                if (!hasOpenXRLoader)
                {
                    Debug.LogWarning("OpenXR loader is not active (required for PICO devices)");
                    return false;
                }
                
                // Check stereo rendering mode
                if (XRSettings.stereoRenderingMode != XRSettings.StereoRenderingMode.MultiPass)
                {
                    Debug.LogWarning("Stereo rendering mode should be MultiPass for PICO devices");
                    // Not critical, so we won't fail the verification
                }
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"XR system verification failed: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Verify performance settings
        /// </summary>
        /// <returns>True if performance settings are appropriate</returns>
        private bool VerifyPerformance()
        {
            try
            {
                // Check target frame rate
                if (Application.targetFrameRate != targetFrameRate)
                {
                    Debug.LogWarning($"Target frame rate is {Application.targetFrameRate}, expected {targetFrameRate}");
                    // Not critical, so we won't fail the verification
                }
                
                // Check vSync count
                if (QualitySettings.vSyncCount != 0)
                {
                    Debug.LogWarning("vSyncCount should be 0 for VR applications");
                    // Not critical, so we won't fail the verification
                }
                
                // Check graphics API
                string graphicsDeviceName = SystemInfo.graphicsDeviceName;
                if (!graphicsDeviceName.Contains("Adreno") && !graphicsDeviceName.Contains("Mali"))
                {
                    Debug.Log($"Graphics device: {graphicsDeviceName}");
                }
                
                // Check recommended quality settings
                if (QualitySettings.GetQualityLevel() > 3)
                {
                    Debug.Log("Consider using a lower quality level for better performance on mobile VR");
                }
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Performance verification failed: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Verify input system configuration
        /// </summary>
        /// <returns>True if input system is properly configured</returns>
        private bool VerifyInputSystem()
        {
            try
            {
                // Check for input devices
                List<InputDevice> devices = new List<InputDevice>();
                InputDevices.GetDevices(devices);
                
                if (devices.Count == 0)
                {
                    Debug.LogWarning("No input devices detected");
                    return false;
                }
                
                // Check for VR controllers
                int controllerCount = 0;
                foreach (var device in devices)
                {
                    if (device.characteristics.HasFlag(InputDeviceCharacteristics.Controller))
                    {
                        controllerCount++;
                    }
                }
                
                if (controllerCount < 2)
                {
                    Debug.LogWarning($"Only {controllerCount} controllers detected, expected 2");
                    // Not critical, so we won't fail the verification
                }
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Input system verification failed: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Verify audio system configuration
        /// </summary>
        /// <returns>True if audio system is properly configured</returns>
        private bool VerifyAudioSystem()
        {
            try
            {
                // Check audio spatializer
                string spatializerPlugin = AudioSettings.GetSpatializerPluginName();
                if (string.IsNullOrEmpty(spatializerPlugin))
                {
                    Debug.LogWarning("No spatializer plugin detected");
                    // Not critical, so we won't fail the verification
                }
                else
                {
                    Debug.Log($"Spatializer plugin: {spatializerPlugin}");
                }
                
                // Check audio sample rate
                int sampleRate = AudioSettings.outputSampleRate;
                if (sampleRate != 48000)
                {
                    Debug.Log($"Audio sample rate: {sampleRate}Hz (48000Hz recommended)");
                }
                
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Audio system verification failed: {e.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Get verification report
        /// </summary>
        /// <returns>Verification report string</returns>
        public string GetVerificationReport()
        {
            return _verificationReport;
        }
        
        /// <summary>
        /// Get verification status
        /// </summary>
        /// <returns>True if device is verified</returns>
        public bool IsVerified()
        {
            return _isVerified;
        }
        
        /// <summary>
        /// Force verification
        /// </summary>
        public void ForceVerification()
        {
            VerifyDeviceConfiguration();
        }
        
        private void OnGUI()
        {
            if (showVerificationUI && !_isVerified)
            {
                GUI.Label(new Rect(10, 10, 800, 600), _verificationReport, _verificationStyle);
            }
        }
    }
}