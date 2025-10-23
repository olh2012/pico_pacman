using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.AI;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the specific ghost implementations
    /// </summary>
    public class SpecificGhostTests
    {
        private GameObject _ghostObject;
        private Blinky _blinky;
        private Pinky _pinky;
        private Inky _inky;
        private Clyde _clyde;
        
        [SetUp]
        public void SetUp()
        {
            // Create GameObjects with specific ghost components
            _ghostObject = new GameObject("Ghost");
            _ghostObject.AddComponent<Rigidbody>();
            
            // Blinky
            GameObject blinkyObject = Object.Instantiate(_ghostObject);
            _blinky = blinkyObject.AddComponent<Blinky>();
            
            // Pinky
            GameObject pinkyObject = Object.Instantiate(_ghostObject);
            _pinky = pinkyObject.AddComponent<Pinky>();
            
            // Inky
            GameObject inkyObject = Object.Instantiate(_ghostObject);
            _inky = inkyObject.AddComponent<Inky>();
            
            // Clyde
            GameObject clydeObject = Object.Instantiate(_ghostObject);
            _clyde = clydeObject.AddComponent<Clyde>();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up
            if (_blinky != null && _blinky.gameObject != null)
            {
                Object.Destroy(_blinky.gameObject);
            }
            
            if (_pinky != null && _pinky.gameObject != null)
            {
                Object.Destroy(_pinky.gameObject);
            }
            
            if (_inky != null && _inky.gameObject != null)
            {
                Object.Destroy(_inky.gameObject);
            }
            
            if (_clyde != null && _clyde.gameObject != null)
            {
                Object.Destroy(_clyde.gameObject);
            }
        }
        
        [Test]
        public void Blinky_InstantiatesSuccessfully()
        {
            // Arrange
            // Act
            // Assert
            Assert.IsNotNull(_blinky);
        }
        
        [Test]
        public void Pinky_InstantiatesSuccessfully()
        {
            // Arrange
            // Act
            // Assert
            Assert.IsNotNull(_pinky);
        }
        
        [Test]
        public void Inky_InstantiatesSuccessfully()
        {
            // Arrange
            // Act
            // Assert
            Assert.IsNotNull(_inky);
        }
        
        [Test]
        public void Clyde_InstantiatesSuccessfully()
        {
            // Arrange
            // Act
            // Assert
            Assert.IsNotNull(_clyde);
        }
        
        [Test]
        public void Blinky_SetFrightened_ChangesState()
        {
            // Arrange
            // Act
            _blinky.SetFrightened();
            Ghost.GhostState state = _blinky.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Frightened, state);
        }
        
        [Test]
        public void Pinky_SetFrightened_ChangesState()
        {
            // Arrange
            // Act
            _pinky.SetFrightened();
            Ghost.GhostState state = _pinky.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Frightened, state);
        }
        
        [Test]
        public void Inky_SetFrightened_ChangesState()
        {
            // Arrange
            // Act
            _inky.SetFrightened();
            Ghost.GhostState state = _inky.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Frightened, state);
        }
        
        [Test]
        public void Clyde_SetFrightened_ChangesState()
        {
            // Arrange
            // Act
            _clyde.SetFrightened();
            Ghost.GhostState state = _clyde.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Frightened, state);
        }
        
        [Test]
        public void Blinky_ResetGhost_ResetsToInitialState()
        {
            // Arrange
            _blinky.SetFrightened();
            Ghost.GhostState frightenedState = _blinky.GetState();
            Assert.AreEqual(Ghost.GhostState.Frightened, frightenedState);
            
            // Act
            _blinky.ResetGhost();
            Ghost.GhostState state = _blinky.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Chase, state);
        }
    }
}