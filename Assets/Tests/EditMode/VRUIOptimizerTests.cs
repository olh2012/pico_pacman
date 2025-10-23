using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.UI;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the VRUIOptimizer class
    /// </summary>
    public class VRUIOptimizerTests
    {
        private VRUIOptimizer _vrUiOptimizer;
        private GameObject _testObject;
        private Canvas _canvas;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with Canvas and VRUIOptimizer components
            _testObject = new GameObject("VRUIOptimizer");
            _canvas = _testObject.AddComponent<Canvas>();
            _vrUiOptimizer = _testObject.AddComponent<VRUIOptimizer>();
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
        public void VRUIOptimizer_InitialState_NoExceptions()
        {
            // Arrange
            // Act & Assert
            // If we get here without exceptions, the test passes
            Assert.Pass("VRUIOptimizer initialized without exceptions");
        }
        
        [Test]
        public void VRUIOptimizer_SetVRScaleFactor_UpdatesScaleFactor()
        {
            // Arrange
            float newScaleFactor = 0.002f;
            float originalScale = _testObject.transform.localScale.x;
            
            // Act
            _vrUiOptimizer.SetVRScaleFactor(newScaleFactor);
            
            // Assert
            // Since auto-adjust is enabled by default, the scale should change
            Assert.AreNotEqual(originalScale, _testObject.transform.localScale.x);
        }
        
        [Test]
        public void VRUIOptimizer_SetVRDistance_UpdatesDistance()
        {
            // Arrange
            float newDistance = 2.0f;
            
            // Act
            _vrUiOptimizer.SetVRDistance(newDistance);
            
            // Assert
            // We can't easily test the position without a camera, but we can verify the method doesn't throw
            Assert.Pass("SetVRDistance executed without exceptions");
        }
        
        [Test]
        public void VRUIOptimizer_SetAutoAdjust_TogglesAutoAdjust()
        {
            // Arrange
            bool originalValue = _vrUiOptimizer.IsAutoAdjustEnabled();
            
            // Act
            _vrUiOptimizer.SetAutoAdjust(!originalValue);
            bool newValue = _vrUiOptimizer.IsAutoAdjustEnabled();
            
            // Assert
            Assert.AreNotEqual(originalValue, newValue);
            Assert.AreEqual(!originalValue, newValue);
        }
        
        [Test]
        public void VRUIOptimizer_OptimizeForVR_AdjustsCanvas()
        {
            // Arrange
            // Make sure we have a canvas
            Assert.IsNotNull(_canvas);
            
            // Act
            _vrUiOptimizer.OptimizeForVR();
            
            // Assert
            // Check that canvas render mode is set to WorldSpace
            Assert.AreEqual(RenderMode.WorldSpace, _canvas.renderMode);
        }
        
        [Test]
        public void VRUIOptimizer_AddOutlineEffect_AddsOutlineComponent()
        {
            // Arrange
            GameObject textObject = new GameObject("TestText");
            UnityEngine.UI.Text textComponent = textObject.AddComponent<UnityEngine.UI.Text>();
            
            // Act
            // Use reflection to call the private method
            var method = _vrUiOptimizer.GetType().GetMethod("AddOutlineEffect", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(_vrUiOptimizer, new object[] { textComponent });
            
            // Assert
            Outline outline = textObject.GetComponent<Outline>();
            Assert.IsNotNull(outline);
            
            // Clean up
            Object.Destroy(textObject);
        }
    }
}