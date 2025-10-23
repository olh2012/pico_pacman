using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using PacMan.Utils;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the Extensions utility class
    /// </summary>
    public class ExtensionsTests
    {
        [Test]
        public void Extensions_GetRandom_ReturnsValidElement()
        {
            // Arrange
            List<int> list = new List<int> { 1, 2, 3, 4, 5 };
            
            // Act
            int randomElement = list.GetRandom();
            
            // Assert
            Assert.Contains(randomElement, list);
        }
        
        [Test]
        public void Extensions_GetRandom_EmptyList_ReturnsDefault()
        {
            // Arrange
            List<int> emptyList = new List<int>();
            
            // Act
            int randomElement = emptyList.GetRandom();
            
            // Assert
            Assert.AreEqual(0, randomElement); // Default for int is 0
        }
        
        [Test]
        public void Extensions_ClampMagnitude_LimitsVectorLength()
        {
            // Arrange
            Vector3 vector = new Vector3(3, 4, 0); // Length = 5
            float maxLength = 3f;
            
            // Act
            Vector3 clampedVector = vector.ClampMagnitude(maxLength);
            
            // Assert
            Assert.LessOrEqual(clampedVector.magnitude, maxLength);
            Assert.AreEqual(maxLength, clampedVector.magnitude, 0.001f);
        }
        
        [Test]
        public void Extensions_ClampMagnitude_SmallVector_Unchanged()
        {
            // Arrange
            Vector3 vector = new Vector3(1, 1, 0); // Length = ~1.41
            float maxLength = 3f;
            float originalMagnitude = vector.magnitude;
            
            // Act
            Vector3 clampedVector = vector.ClampMagnitude(maxLength);
            
            // Assert
            Assert.AreEqual(originalMagnitude, clampedVector.magnitude, 0.001f);
        }
        
        [Test]
        public void Extensions_ToGridPosition_ConvertsWorldToGrid()
        {
            // Arrange
            Vector3 worldPosition = new Vector3(2.5f, 0, 3.7f);
            float cellSize = 1.0f;
            
            // Act
            Vector2Int gridPosition = worldPosition.ToGridPosition(cellSize);
            
            // Assert
            Assert.AreEqual(3, gridPosition.x); // Rounded from 2.5
            Assert.AreEqual(4, gridPosition.y); // Rounded from 3.7
        }
        
        [Test]
        public void Extensions_ToWorldPosition_ConvertsGridToWorld()
        {
            // Arrange
            Vector2Int gridPosition = new Vector2Int(3, 4);
            float cellSize = 1.0f;
            
            // Act
            Vector3 worldPosition = gridPosition.ToWorldPosition(cellSize);
            
            // Assert
            Assert.AreEqual(3.0f, worldPosition.x);
            Assert.AreEqual(0.0f, worldPosition.y);
            Assert.AreEqual(4.0f, worldPosition.z);
        }
    }
}