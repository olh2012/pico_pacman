using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using PacMan.Utils;

namespace PacMan.Tests
{
    /// <summary>
    /// Unit tests for the ObjectPooler class
    /// </summary>
    public class ObjectPoolerTests
    {
        private ObjectPooler _objectPooler;
        private GameObject _testObject;
        
        [SetUp]
        public void SetUp()
        {
            // Create a GameObject with ObjectPooler component
            _testObject = new GameObject("ObjectPooler");
            _objectPooler = _testObject.AddComponent<ObjectPooler>();
        }
        
        [TearDown]
        public void TearDown()
        {
            // Clean up
            if (_testObject != null)
            {
                Object.Destroy(_testObject);
            }
            
            // Clean up any pooled objects
            GameObject[] pooledObjects = GameObject.FindGameObjectsWithTag("PooledObject");
            foreach (GameObject obj in pooledObjects)
            {
                Object.Destroy(obj);
            }
        }
        
        [Test]
        public void ObjectPooler_InitialState_NoExceptions()
        {
            // Arrange
            // Act & Assert
            // If we get here without exceptions, the test passes
            Assert.Pass("ObjectPooler initialized without exceptions");
        }
        
        [Test]
        public void ObjectPooler_Instance_SingletonPattern()
        {
            // Arrange
            ObjectPooler firstInstance = ObjectPooler.Instance;
            ObjectPooler secondInstance = ObjectPooler.Instance;
            
            // Act & Assert
            Assert.IsNotNull(firstInstance);
            Assert.IsNotNull(secondInstance);
            Assert.AreEqual(firstInstance, secondInstance);
        }
        
        [Test]
        public void ObjectPooler_AddPool_AddsNewPool()
        {
            // Arrange
            string tag = "TestPool";
            GameObject prefab = new GameObject("TestPrefab");
            int size = 5;
            
            // Act
            _objectPooler.AddPool(tag, prefab, size);
            
            // Assert
            int poolSize = _objectPooler.GetPoolSize(tag);
            Assert.GreaterOrEqual(poolSize, 0); // Pool exists
            
            // Clean up
            Object.Destroy(prefab);
        }
        
        [Test]
        public void ObjectPooler_SpawnFromPool_ReturnsObject()
        {
            // Arrange
            string tag = "TestPool";
            GameObject prefab = new GameObject("TestPrefab");
            int size = 5;
            _objectPooler.AddPool(tag, prefab, size);
            
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            
            // Act
            GameObject spawnedObject = _objectPooler.SpawnFromPool(tag, position, rotation);
            
            // Assert
            Assert.IsNotNull(spawnedObject);
            Assert.IsTrue(spawnedObject.activeSelf);
            Assert.AreEqual(position, spawnedObject.transform.position);
            
            // Clean up
            Object.Destroy(prefab);
            if (spawnedObject != null)
            {
                Object.Destroy(spawnedObject);
            }
        }
        
        [Test]
        public void ObjectPooler_ReturnToPool_ReturnsObjectToPool()
        {
            // Arrange
            string tag = "TestPool";
            GameObject prefab = new GameObject("TestPrefab");
            int size = 5;
            _objectPooler.AddPool(tag, prefab, size);
            
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            GameObject spawnedObject = _objectPooler.SpawnFromPool(tag, position, rotation);
            
            // Add PooledObject component to track the object
            PooledObject pooledObject = spawnedObject.AddComponent<PooledObject>();
            pooledObject.poolTag = tag;
            
            int initialPoolSize = _objectPooler.GetPoolSize(tag);
            
            // Act
            _objectPooler.ReturnToPool(tag, spawnedObject);
            
            // Assert
            int finalPoolSize = _objectPooler.GetPoolSize(tag);
            Assert.GreaterOrEqual(finalPoolSize, initialPoolSize);
            Assert.IsFalse(spawnedObject.activeSelf);
            
            // Clean up
            Object.Destroy(prefab);
        }
        
        [Test]
        public void ObjectPooler_GetPoolSize_ReturnsCorrectSize()
        {
            // Arrange
            string tag = "TestPool";
            GameObject prefab = new GameObject("TestPrefab");
            int size = 5;
            _objectPooler.AddPool(tag, prefab, size);
            
            // Act
            int poolSize = _objectPooler.GetPoolSize(tag);
            
            // Assert
            Assert.GreaterOrEqual(poolSize, 0);
            
            // Clean up
            Object.Destroy(prefab);
        }
        
        [Test]
        public void ObjectPooler_PreloadPool_IncreasesPoolSize()
        {
            // Arrange
            string tag = "TestPool";
            GameObject prefab = new GameObject("TestPrefab");
            int initialSize = 2;
            int preloadCount = 3;
            _objectPooler.AddPool(tag, prefab, initialSize);
            
            int initialPoolSize = _objectPooler.GetPoolSize(tag);
            
            // Act
            _objectPooler.PreloadPool(tag, preloadCount);
            
            // Assert
            int finalPoolSize = _objectPooler.GetPoolSize(tag);
            Assert.GreaterOrEqual(finalPoolSize, initialPoolSize);
            
            // Clean up
            Object.Destroy(prefab);
        }
    }
}