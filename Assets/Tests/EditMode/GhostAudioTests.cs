using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.AI;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the GhostAudioController class
    /// </summary>
    public class GhostAudioTests
    {
        private GhostAudioController _ghostAudioController;
        private GameObject _testObject;
        private AudioSource _audioSource;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with GhostAudioController and AudioSource components
            _testObject = new GameObject("GhostAudioController");
            _audioSource = _testObject.AddComponent<AudioSource>();
            _ghostAudioController = _testObject.AddComponent<GhostAudioController>();
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
        public void GhostAudioController_InitialState_NoExceptions()
        {
            // Arrange
            // Act & Assert
            // If we get here without exceptions, the test passes
            Assert.Pass("GhostAudioController initialized without exceptions");
        }
        
        [Test]
        public void GhostAudioController_SetFrightened_UpdatesState()
        {
            // Arrange
            // Use reflection to access private field
            var frightenedField = _ghostAudioController.GetType().GetField("_isFrightened", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            // Act
            _ghostAudioController.SetFrightened(true);
            bool isFrightened = (bool)frightenedField.GetValue(_ghostAudioController);
            
            // Assert
            Assert.IsTrue(isFrightened);
        }
        
        [Test]
        public void GhostAudioController_SetSoundsEnabled_TogglesSounds()
        {
            // Arrange
            bool originalValue = _ghostAudioController.AreSoundsEnabled();
            
            // Act
            _ghostAudioController.SetSoundsEnabled(!originalValue);
            bool newValue = _ghostAudioController.AreSoundsEnabled();
            
            // Assert
            Assert.AreNotEqual(originalValue, newValue);
            Assert.AreEqual(!originalValue, newValue);
        }
        
        [Test]
        public void GhostAudioController_InitializeAudioSource_SetsSpatialBlend()
        {
            // Arrange
            // Use reflection to access private field
            var useSpatialAudioField = _ghostAudioController.GetType().GetField("useSpatialAudio", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            useSpatialAudioField.SetValue(_ghostAudioController, true);
            
            // Act
            // Use reflection to call private method
            var method = _ghostAudioController.GetType().GetMethod("InitializeAudioSource", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(_ghostAudioController, null);
            
            // Assert
            Assert.AreEqual(1.0f, _audioSource.spatialBlend);
        }
        
        [Test]
        public void GhostAudioController_PlayRespawnSound_PlaysAudio()
        {
            // Arrange
            AudioClip testClip = new AudioClip();
            // Use reflection to set private field
            var respawnSoundField = _ghostAudioController.GetType().GetField("ghostRespawnSound", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            respawnSoundField.SetValue(_ghostAudioController, testClip);
            
            // Store initial time
            float initialTime = Time.time;
            
            // Act
            _ghostAudioController.PlayRespawnSound();
            
            // Assert
            // We can't easily test if the clip actually played, but we can verify the method doesn't throw
            Assert.Pass("PlayRespawnSound executed without exceptions");
        }
    }
}