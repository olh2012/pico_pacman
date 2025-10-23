using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.Utils;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the DeviceVerification class
    /// </summary>
    public class DeviceVerificationTests
    {
        private DeviceVerification _deviceVerification;
        private GameObject _testObject;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with DeviceVerification component
            _testObject = new GameObject("DeviceVerification");
            _deviceVerification = _testObject.AddComponent<DeviceVerification>();
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
        public void DeviceVerification_InitialState_NoExceptions()
        {
            // Arrange
            // Act & Assert
            // If we get here without exceptions, the test passes
            Assert.Pass("DeviceVerification initialized without exceptions");
        }
        
        [Test]
        public void DeviceVerification_VerifyDeviceConfiguration_NoExceptions()
        {
            // Arrange
            // Act & Assert
            Assert.DoesNotThrow(() => _deviceVerification.VerifyDeviceConfiguration());
        }
        
        [Test]
        public void DeviceVerification_ForceVerification_NoExceptions()
        {
            // Arrange
            // Act & Assert
            Assert.DoesNotThrow(() => _deviceVerification.ForceVerification());
        }
        
        [Test]
        public void DeviceVerification_GetVerificationReport_ReturnsString()
        {
            // Arrange
            // Act
            string report = _deviceVerification.GetVerificationReport();
            
            // Assert
            Assert.IsNotNull(report);
            Assert.IsNotEmpty(report);
        }
        
        [Test]
        public void DeviceVerification_IsVerified_ReturnsBoolean()
        {
            // Arrange
            // Act
            bool isVerified = _deviceVerification.IsVerified();
            
            // Assert
            Assert.IsFalse(isVerified); // Should be false in editor
        }
        
        [Test]
        public void DeviceVerification_VerifyXRSystem_NoExceptions()
        {
            // Arrange
            // Use reflection to call private method
            var method = _deviceVerification.GetType().GetMethod("VerifyXRSystem", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            // Act & Assert
            Assert.DoesNotThrow(() => method.Invoke(_deviceVerification, null));
        }
        
        [Test]
        public void DeviceVerification_VerifyPerformance_NoExceptions()
        {
            // Arrange
            // Use reflection to call private method
            var method = _deviceVerification.GetType().GetMethod("VerifyPerformance", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            // Act & Assert
            Assert.DoesNotThrow(() => method.Invoke(_deviceVerification, null));
        }
        
        [Test]
        public void DeviceVerification_VerifyInputSystem_NoExceptions()
        {
            // Arrange
            // Use reflection to call private method
            var method = _deviceVerification.GetType().GetMethod("VerifyInputSystem", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            // Act & Assert
            Assert.DoesNotThrow(() => method.Invoke(_deviceVerification, null));
        }
        
        [Test]
        public void DeviceVerification_VerifyAudioSystem_NoExceptions()
        {
            // Arrange
            // Use reflection to call private method
            var method = _deviceVerification.GetType().GetMethod("VerifyAudioSystem", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            // Act & Assert
            Assert.DoesNotThrow(() => method.Invoke(_deviceVerification, null));
        }
    }
}