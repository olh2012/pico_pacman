using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.GameSystem;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the ScoreManager class
    /// </summary>
    public class ScoreManagerTests
    {
        private ScoreManager _scoreManager;
        private GameObject _scoreManagerObject;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with ScoreManager component
            _scoreManagerObject = new GameObject("ScoreManager");
            _scoreManager = _scoreManagerObject.AddComponent<ScoreManager>();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up
            if (_scoreManagerObject != null)
            {
                Object.Destroy(_scoreManagerObject);
            }
        }
        
        [Test]
        public void ScoreManager_InitialState_ScoreIsZero()
        {
            // Arrange
            // Act
            int score = _scoreManager.GetScore();
            
            // Assert
            Assert.AreEqual(0, score);
        }
        
        [Test]
        public void ScoreManager_AddScore_IncreasesScore()
        {
            // Arrange
            int initialScore = _scoreManager.GetScore();
            int pointsToAdd = 100;
            
            // Act
            _scoreManager.AddScore(pointsToAdd);
            int newScore = _scoreManager.GetScore();
            
            // Assert
            Assert.AreEqual(initialScore + pointsToAdd, newScore);
        }
        
        [Test]
        public void ScoreManager_ResetScore_SetsScoreToZero()
        {
            // Arrange
            _scoreManager.AddScore(500);
            int scoreBeforeReset = _scoreManager.GetScore();
            Assert.Greater(scoreBeforeReset, 0);
            
            // Act
            _scoreManager.ResetScore();
            int scoreAfterReset = _scoreManager.GetScore();
            
            // Assert
            Assert.AreEqual(0, scoreAfterReset);
        }
        
        [Test]
        public void ScoreManager_GetHighScore_ReturnsValidScore()
        {
            // Arrange
            // Act
            int highScore = _scoreManager.GetHighScore();
            
            // Assert
            Assert.GreaterOrEqual(highScore, 0);
        }
        
        [Test]
        public void ScoreManager_GetGhostComboCount_ReturnsValidCount()
        {
            // Arrange
            // Act
            int comboCount = _scoreManager.GetGhostComboCount();
            
            // Assert
            Assert.GreaterOrEqual(comboCount, 0);
        }
    }
}