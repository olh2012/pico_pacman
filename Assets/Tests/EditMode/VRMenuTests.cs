using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.UI;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the VRMenu class
    /// </summary>
    public class VRMenuTests
    {
        private VRMenu _vrMenu;
        private GameObject _testObject;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with VRMenu component
            _testObject = new GameObject("VRMenu");
            _vrMenu = _testObject.AddComponent<VRMenu>();
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
        public void VRMenu_InitialState_NoExceptions()
        {
            // Arrange
            // Act & Assert
            // If we get here without exceptions, the test passes
            Assert.Pass("VRMenu initialized without exceptions");
        }
        
        [Test]
        public void VRMenu_ShowMainMenu_ActivatesMainMenuPanel()
        {
            // Arrange
            GameObject mainMenuPanel = new GameObject("MainMenuPanel");
            // Use reflection to set private field
            var mainMenuPanelField = _vrMenu.GetType().GetField("mainMenuPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            mainMenuPanelField.SetValue(_vrMenu, mainMenuPanel);
            
            // Act
            // Use reflection to call private method
            var method = _vrMenu.GetType().GetMethod("ShowMainMenu", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(_vrMenu, null);
            
            // Assert
            Assert.IsTrue(mainMenuPanel.activeSelf);
            
            // Clean up
            Object.Destroy(mainMenuPanel);
        }
        
        [Test]
        public void VRMenu_ShowSettings_ActivatesSettingsPanel()
        {
            // Arrange
            GameObject settingsPanel = new GameObject("SettingsPanel");
            // Use reflection to set private field
            var settingsPanelField = _vrMenu.GetType().GetField("settingsPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            settingsPanelField.SetValue(_vrMenu, settingsPanel);
            
            // Act
            _vrMenu.ShowSettings();
            
            // Assert
            Assert.IsTrue(settingsPanel.activeSelf);
            
            // Clean up
            Object.Destroy(settingsPanel);
        }
        
        [Test]
        public void VRMenu_ShowCredits_ActivatesCreditsPanel()
        {
            // Arrange
            GameObject creditsPanel = new GameObject("CreditsPanel");
            // Use reflection to set private field
            var creditsPanelField = _vrMenu.GetType().GetField("creditsPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            creditsPanelField.SetValue(_vrMenu, creditsPanel);
            
            // Act
            _vrMenu.ShowCredits();
            
            // Assert
            Assert.IsTrue(creditsPanel.activeSelf);
            
            // Clean up
            Object.Destroy(creditsPanel);
        }
        
        [Test]
        public void VRMenu_HandleTriggerPressed_UpdatesTriggerState()
        {
            // Arrange
            // Use reflection to access private field
            var triggerPressedField = _vrMenu.GetType().GetField("_triggerPressed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            // Act
            // Use reflection to call private method
            var method = _vrMenu.GetType().GetMethod("HandleTriggerPressed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method.Invoke(_vrMenu, new object[] { true });
            
            // Assert
            bool triggerPressed = (bool)triggerPressedField.GetValue(_vrMenu);
            Assert.IsTrue(triggerPressed);
        }
        
        [Test]
        public void VRMenu_GetActivePanel_ReturnsCorrectPanel()
        {
            // Arrange
            GameObject mainMenuPanel = new GameObject("MainMenuPanel");
            mainMenuPanel.SetActive(true);
            
            // Use reflection to set private field
            var mainMenuPanelField = _vrMenu.GetType().GetField("mainMenuPanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            mainMenuPanelField.SetValue(_vrMenu, mainMenuPanel);
            
            // Act
            // Use reflection to call private method
            var method = _vrMenu.GetType().GetMethod("GetActivePanel", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            object result = method.Invoke(_vrMenu, null);
            
            // Assert
            Assert.AreEqual(mainMenuPanel, result);
            
            // Clean up
            Object.Destroy(mainMenuPanel);
        }
    }
}