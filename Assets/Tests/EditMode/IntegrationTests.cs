using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.GameSystem;
using PacMan.AI;
using PacMan.Player;

namespace PacMan.Tests
{
    /// <summary>
    /// Integration tests for system interactions
    /// </summary>
    public class IntegrationTests
    {
        private GameManager _gameManager;
        private ScoreManager _scoreManager;
        private PlayerController _playerController;
        private Ghost _ghost;
        private GameObject _gameManagerObject;
        private GameObject _playerObject;
        private GameObject _ghostObject;
        
        [SetUp]
        public void SetUp()
        {
            // Create GameManager
            _gameManagerObject = new GameObject("GameManager");
            _gameManager = _gameManagerObject.AddComponent<GameManager>();
            
            // Create ScoreManager
            GameObject scoreManagerObject = new GameObject("ScoreManager");
            _scoreManager = scoreManagerObject.AddComponent<ScoreManager>();
            
            // Create Player
            _playerObject = new GameObject("Player");
            _playerObject.AddComponent<Rigidbody>();
            _playerController = _playerObject.AddComponent<PlayerController>();
            
            // Create Ghost
            _ghostObject = new GameObject("Ghost");
            _ghostObject.AddComponent<Rigidbody>();
            _ghost = _ghostObject.AddComponent<Ghost>();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up
            if (_gameManagerObject != null)
            {
                Object.Destroy(_gameManagerObject);
            }
            
            if (_playerObject != null)
            {
                Object.Destroy(_playerObject);
            }
            
            if (_ghostObject != null)
            {
                Object.Destroy(_ghostObject);
            }
        }
        
        [Test]
        public void GameManagerAndScoreManager_Integration_ScoreUpdatesGameState()
        {
            // Arrange
            int initialScore = _gameManager.GetScore();
            int pointsToAdd = 100;
            
            // Act
            _gameManager.AddScore(pointsToAdd);
            int newScore = _gameManager.GetScore();
            
            // Assert
            Assert.AreEqual(initialScore + pointsToAdd, newScore);
        }
        
        [Test]
        public void PlayerAndGhost_Integration_PoweredUpState()
        {
            // Arrange
            bool initialPoweredUpState = _playerController.IsPoweredUp();
            
            // Act
            _playerController.SetPoweredUp(true);
            bool newPoweredUpState = _playerController.IsPoweredUp();
            
            // Assert
            Assert.IsFalse(initialPoweredUpState);
            Assert.IsTrue(newPoweredUpState);
        }
        
        [Test]
        public void GhostStateTransitions_Integration_FrightenedToRecovering()
        {
            // Arrange
            _ghost.SetFrightened();
            Ghost.GhostState frightenedState = _ghost.GetState();
            Assert.AreEqual(Ghost.GhostState.Frightened, frightenedState);
            
            // Act
            _ghost.SetRecovering();
            Ghost.GhostState recoveringState = _ghost.GetState();
            
            // Assert
            Assert.AreEqual(Ghost.GhostState.Recovering, recoveringState);
        }
        
        [Test]
        public void GameSystem_Integration_StartNewGameResetsSystems()
        {
            // Arrange
            _gameManager.AddScore(1000);
            int scoreBeforeReset = _gameManager.GetScore();
            Assert.Greater(scoreBeforeReset, 0);
            
            // Act
            _gameManager.StartNewGame();
            int scoreAfterReset = _gameManager.GetScore();
            GameState gameState = _gameManager.GetGameState();
            
            // Assert
            Assert.AreEqual(0, scoreAfterReset);
            Assert.AreEqual(GameState.Playing, gameState);
        }
    }
}