using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.GameSystem;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the GameManager class
    /// </summary>
    public class GameManagerTests
    {
        private GameManager _gameManager;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with GameManager component
            GameObject gameManagerObject = new GameObject("GameManager");
            _gameManager = gameManagerObject.AddComponent<GameManager>();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up
            if (_gameManager != null && _gameManager.gameObject != null)
            {
                Object.Destroy(_gameManager.gameObject);
            }
        }
        
        [Test]
        public void GameManager_InitialState_IsMenu()
        {
            // Arrange
            // Act
            GameState state = _gameManager.GetGameState();
            
            // Assert
            Assert.AreEqual(GameState.Menu, state);
        }
        
        [Test]
        public void GameManager_StartNewGame_SetsPlayingState()
        {
            // Arrange
            // Act
            _gameManager.StartNewGame();
            GameState state = _gameManager.GetGameState();
            
            // Assert
            Assert.AreEqual(GameState.Playing, state);
        }
        
        [Test]
        public void GameManager_StartNewGame_ResetsScore()
        {
            // Arrange
            _gameManager.AddScore(1000);
            int initialScore = _gameManager.GetScore();
            Assert.Greater(initialScore, 0);
            
            // Act
            _gameManager.StartNewGame();
            int newScore = _gameManager.GetScore();
            
            // Assert
            Assert.AreEqual(0, newScore);
        }
        
        [Test]
        public void GameManager_AddScore_IncreasesScore()
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
        public void GameManager_LoseLife_DecreasesLives()
        {
            // Arrange
            _gameManager.StartNewGame();
            int initialLives = _gameManager.GetRemainingLives();
            
            // Act
            _gameManager.LoseLife();
            int remainingLives = _gameManager.GetRemainingLives();
            
            // Assert
            Assert.AreEqual(initialLives - 1, remainingLives);
        }
        
        [Test]
        public void GameManager_GameOver_SetsGameOverState()
        {
            // Arrange
            // Act
            _gameManager.GameOver();
            GameState state = _gameManager.GetGameState();
            
            // Assert
            Assert.AreEqual(GameState.GameOver, state);
        }
        
        [Test]
        public void GameManager_ChangeGameState_UpdatesState()
        {
            // Arrange
            // Act
            _gameManager.ChangeGameState(GameState.Paused);
            GameState state = _gameManager.GetGameState();
            
            // Assert
            Assert.AreEqual(GameState.Paused, state);
        }
        
        [Test]
        public void GameManager_GetHighScore_ReturnsValidScore()
        {
            // Arrange
            // Act
            int highScore = _gameManager.GetHighScore();
            
            // Assert
            Assert.GreaterOrEqual(highScore, 0);
        }
        
        [Test]
        public void GameManager_GetLevel_ReturnsValidLevel()
        {
            // Arrange
            // Act
            int level = _gameManager.GetLevel();
            
            // Assert
            Assert.GreaterOrEqual(level, 1);
        }
    }
}