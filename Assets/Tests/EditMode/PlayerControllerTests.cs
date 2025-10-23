using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.Player;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the PlayerController class
    /// </summary>
    public class PlayerControllerTests
    {
        private PlayerController _playerController;
        private GameObject _playerObject;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with PlayerController component
            _playerObject = new GameObject("Player");
            _playerObject.AddComponent<Rigidbody>();
            _playerController = _playerObject.AddComponent<PlayerController>();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up
            if (_playerObject != null)
            {
                Object.Destroy(_playerObject);
            }
        }
        
        [Test]
        public void PlayerController_InstantiatesSuccessfully()
        {
            // Arrange
            // Act
            // Assert
            Assert.IsNotNull(_playerController);
        }
        
        [Test]
        public void PlayerController_SetPoweredUp_ChangesPoweredUpState()
        {
            // Arrange
            // Act
            _playerController.SetPoweredUp(true);
            bool isPoweredUp = _playerController.IsPoweredUp();
            
            // Assert
            Assert.IsTrue(isPoweredUp);
        }
        
        [Test]
        public void PlayerController_IsPoweredUp_ReturnsFalseByDefault()
        {
            // Arrange
            // Act
            bool isPoweredUp = _playerController.IsPoweredUp();
            
            // Assert
            Assert.IsFalse(isPoweredUp);
        }
    }
}