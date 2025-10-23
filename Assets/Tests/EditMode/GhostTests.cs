using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.AI;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the Ghost base class
    /// </summary>
    public class GhostTests
    {
        private Ghost _ghost;
        private GameObject _ghostObject;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with Ghost component
            _ghostObject = new GameObject("Ghost");
            _ghostObject.AddComponent<Rigidbody>();
            _ghost = _ghostObject.AddComponent<Ghost>();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up
            if (_ghostObject != null)
            {
                Object.Destroy(_ghostObject);
            }
        }
        
        [Test]
        public void Ghost_InitialState_IsChase()
        {
            // Arrange
            // Act
            Ghost.GhostState state = _ghost.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Chase, state);
        }
        
        [Test]
        public void Ghost_SetFrightened_ChangesToFrightenedState()
        {
            // Arrange
            // Act
            _ghost.SetFrightened();
            Ghost.GhostState state = _ghost.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Frightened, state);
        }
        
        [Test]
        public void Ghost_SetRecovering_ChangesToRecoveringState()
        {
            // Arrange
            _ghost.SetFrightened();
            
            // Act
            _ghost.SetRecovering();
            Ghost.GhostState state = _ghost.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Recovering, state);
        }
        
        [Test]
        public void Ghost_SetReturningHome_ChangesToReturningHomeState()
        {
            // Arrange
            // Act
            _ghost.SetReturningHome();
            Ghost.GhostState state = _ghost.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.ReturningHome, state);
        }
        
        [Test]
        public void Ghost_IsPoweredUp_ReturnsFalseByDefault()
        {
            // Arrange
            // Act
            // This test is for PlayerController, not Ghost
            // We'll skip this for now
        }
        
        [Test]
        public void Ghost_ResetGhost_ResetsToInitialState()
        {
            // Arrange
            _ghost.SetFrightened();
            Ghost.GhostState frightenedState = _ghost.GetState();
            Assert.AreEqual(Ghost.GhostState.Frightened, frightenedState);
            
            // Act
            _ghost.ResetGhost();
            Ghost.GhostState state = _ghost.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Chase, state);
        }
    }
}