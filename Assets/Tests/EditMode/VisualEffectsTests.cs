using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.VisualEffects;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the VisualEffectsManager class
    /// </summary>
    public class VisualEffectsTests
    {
        private VisualEffectsManager _visualEffectsManager;
        private GameObject _testObject;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with VisualEffectsManager component
            _testObject = new GameObject("VisualEffectsManager");
            _visualEffectsManager = _testObject.AddComponent<VisualEffectsManager>();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up
            if (_testObject != null)
            {
                Object.Destroy(_testObject);
            }
        }
        
        [Test]
        public void VisualEffectsManager_InitialState_NoExceptions()
        {
            // Arrange
            // Act & Assert
            // If we get here without exceptions, the test passes
            Assert.Pass("VisualEffectsManager initialized without exceptions");
        }
        
        [Test]
        public void VisualEffectsManager_SetPowerUpActive_UpdatesState()
        {
            // Arrange
            bool originalValue = _visualEffectsManager.IsPowerUpActive();
            
            // Act
            _visualEffectsManager.SetPowerUpActive(!originalValue);
            bool newValue = _visualEffectsManager.IsPowerUpActive();
            
            // Assert
            Assert.AreNotEqual(originalValue, newValue);
            Assert.AreEqual(!originalValue, newValue);
        }
        
        [Test]
        public void VisualEffectsManager_SetPowerUpActive_False_ResetsTimer()
        {
            // Arrange
            // Use reflection to set private fields
            var powerUpActiveField = _visualEffectsManager.GetType().GetField("_isPowerUpActive", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var powerUpTimerField = _visualEffectsManager.GetType().GetField("_powerUpTimer", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            powerUpActiveField.SetValue(_visualEffectsManager, true);
            powerUpTimerField.SetValue(_visualEffectsManager, 5.0f);
            
            // Act
            _visualEffectsManager.SetPowerUpActive(false);
            
            // Assert
            bool isPowerUpActive = (bool)powerUpActiveField.GetValue(_visualEffectsManager);
            float powerUpTimer = (float)powerUpTimerField.GetValue(_visualEffectsManager);
            
            Assert.IsFalse(isPowerUpActive);
            Assert.AreEqual(0f, powerUpTimer);
        }
        
        [Test]
        public void VisualEffectsManager_ScreenShake_CoroutineRuns()
        {
            // Arrange
            Camera testCamera = new GameObject("TestCamera").AddComponent<Camera>();
            // Use reflection to set private field
            var cameraField = _visualEffectsManager.GetType().GetField("mainCamera", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            cameraField.SetValue(_visualEffectsManager, testCamera);
            
            Vector3 originalPosition = testCamera.transform.localPosition;
            
            // Act
            // Use reflection to call private method
            var method = _visualEffectsManager.GetType().GetMethod("ScreenShake", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var coroutine = method.Invoke(_visualEffectsManager, new object[] { 0.1f, 0.1f });
            
            // Assert
            Assert.IsNotNull(coroutine);
            
            // Clean up
            Object.Destroy(testCamera.gameObject);
        }
        
        [Test]
        public void VisualEffectsManager_ChangePlayerColor_UpdatesMaterial()
        {
            // Arrange
            Material testMaterial = new Material(Shader.Find("Standard"));
            Color originalColor = testMaterial.color;
            Color newColor = Color.blue;
            
            // Use reflection to set private field
            var materialField = _visualEffectsManager.GetType().GetField("playerMaterial", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            materialField.SetValue(_visualEffectsManager, testMaterial);
            
            // Act
            // Use reflection to call private method
            var method = _visualEffectsManager.GetType().GetMethod("ChangePlayerColor", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(_visualEffectsManager, new object[] { newColor });
            
            // Assert
            Assert.AreNotEqual(originalColor, testMaterial.color);
            Assert.AreEqual(newColor, testMaterial.color);
            
            // Clean up
            Object.Destroy(testMaterial);
        }
    }
}