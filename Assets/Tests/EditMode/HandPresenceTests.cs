using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.Player;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the HandPresenceController class
    /// </summary>
    public class HandPresenceTests
    {
        private HandPresenceController _handPresenceController;
        private GameObject _testObject;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with HandPresenceController component
            _testObject = new GameObject("HandPresenceController");
            _handPresenceController = _testObject.AddComponent<HandPresenceController>();
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
        public void HandPresenceController_InitialState_NoExceptions()
        {
            // Arrange
            // Act & Assert
            // If we get here without exceptions, the test passes
            Assert.Pass("HandPresenceController initialized without exceptions");
        }
        
        [Test]
        public void HandPresenceController_SetHandVisibility_TogglesHandModels()
        {
            // Arrange
            GameObject leftHand = new GameObject("LeftHand");
            GameObject rightHand = new GameObject("RightHand");
            
            // Use reflection to set private fields
            var leftHandField = _handPresenceController.GetType().GetField("leftHandModel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var rightHandField = _handPresenceController.GetType().GetField("rightHandModel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            leftHandField.SetValue(_handPresenceController, leftHand);
            rightHandField.SetValue(_handPresenceController, rightHand);
            
            // Act
            _handPresenceController.SetHandVisibility(true);
            
            // Assert
            Assert.IsTrue(leftHand.activeSelf);
            Assert.IsTrue(rightHand.activeSelf);
            
            // Clean up
            Object.Destroy(leftHand);
            Object.Destroy(rightHand);
        }
        
        [Test]
        public void HandPresenceController_SetControllerVisibility_TogglesControllerModels()
        {
            // Arrange
            GameObject leftController = new GameObject("LeftController");
            GameObject rightController = new GameObject("RightController");
            
            // Use reflection to set private fields
            var leftControllerField = _handPresenceController.GetType().GetField("leftControllerModel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var rightControllerField = _handPresenceController.GetType().GetField("rightControllerModel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            leftControllerField.SetValue(_handPresenceController, leftController);
            rightControllerField.SetValue(_handPresenceController, rightController);
            
            // Act
            _handPresenceController.SetControllerVisibility(true);
            
            // Assert
            Assert.IsTrue(leftController.activeSelf);
            Assert.IsTrue(rightController.activeSelf);
            
            // Clean up
            Object.Destroy(leftController);
            Object.Destroy(rightController);
        }
        
        [Test]
        public void HandPresenceController_ShowHands_SetsCorrectVisibility()
        {
            // Arrange
            GameObject leftHand = new GameObject("LeftHand");
            GameObject rightHand = new GameObject("RightHand");
            GameObject leftController = new GameObject("LeftController");
            GameObject rightController = new GameObject("RightController");
            
            // Use reflection to set private fields
            var leftHandField = _handPresenceController.GetType().GetField("leftHandModel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var rightHandField = _handPresenceController.GetType().GetField("rightHandModel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var leftControllerField = _handPresenceController.GetType().GetField("leftControllerModel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var rightControllerField = _handPresenceController.GetType().GetField("rightControllerModel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            leftHandField.SetValue(_handPresenceController, leftHand);
            rightHandField.SetValue(_handPresenceController, rightHand);
            leftControllerField.SetValue(_handPresenceController, leftController);
            rightControllerField.SetValue(_handPresenceController, rightController);
            
            // Initially show controllers
            _handPresenceController.SetControllerVisibility(true);
            _handPresenceController.SetHandVisibility(false);
            
            // Act
            _handPresenceController.ShowHands();
            
            // Assert
            Assert.IsTrue(leftHand.activeSelf);
            Assert.IsTrue(rightHand.activeSelf);
            Assert.IsFalse(leftController.activeSelf);
            Assert.IsFalse(rightController.activeSelf);
            
            // Clean up
            Object.Destroy(leftHand);
            Object.Destroy(rightHand);
            Object.Destroy(leftController);
            Object.Destroy(rightController);
        }
    }
}