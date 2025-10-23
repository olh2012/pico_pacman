using UnityEngine;
using System.Collections.Generic;

namespace PacMan.Utils
{
    /// <summary>
    /// Generic object pooler for reusing objects to reduce garbage collection
    /// </summary>
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }
        
        [Header("Pool Settings")]
        [SerializeField] private List<Pool> pools;
        [SerializeField] private bool autoExpand = true;
        
        private Dictionary<string, Queue<GameObject>> _poolDictionary;
        private static ObjectPooler _instance;
        
        public static ObjectPooler Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("ObjectPooler");
                    _instance = singletonObject.AddComponent<ObjectPooler>();
                }
                return _instance;
            }
        }
        
        private void Awake()
        {
            // Ensure singleton pattern
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Initialize pools
            InitializePools();
        }
        
        /// <summary>
        /// Initialize all pools
        /// </summary>
        private void InitializePools()
        {
            _poolDictionary = new Dictionary<string, Queue<GameObject>>();
            
            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();
                
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                
                _poolDictionary.Add(pool.tag, objectPool);
            }
        }
        
        /// <summary>
        /// Spawn an object from the pool
        /// </summary>
        /// <param name="tag">Tag of the object pool</param>
        /// <param name="position">Position to spawn at</param>
        /// <param name="rotation">Rotation to spawn with</param>
        /// <returns>Spawned object or null if pool doesn't exist</returns>
        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return null;
            }
            
            GameObject objectToSpawn = _poolDictionary[tag].Dequeue();
            
            // If pool is empty and autoExpand is enabled, create new object
            if (objectToSpawn == null && autoExpand)
            {
                Pool pool = pools.Find(p => p.tag == tag);
                if (pool != null)
                {
                    objectToSpawn = Instantiate(pool.prefab);
                }
            }
            
            if (objectToSpawn != null)
            {
                objectToSpawn.SetActive(true);
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
                
                // Return object to pool when it's deactivated
                objectToSpawn.GetComponent<PooledObject>().poolTag = tag;
            }
            
            return objectToSpawn;
        }
        
        /// <summary>
        /// Return an object to the pool
        /// </summary>
        /// <param name="tag">Tag of the object pool</param>
        /// <param name="obj">Object to return</param>
        public void ReturnToPool(string tag, GameObject obj)
        {
            if (!_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return;
            }
            
            obj.SetActive(false);
            _poolDictionary[tag].Enqueue(obj);
        }
        
        /// <summary>
        /// Preload objects into a pool
        /// </summary>
        /// <param name="tag">Tag of the object pool</param>
        /// <param name="count">Number of objects to preload</param>
        public void PreloadPool(string tag, int count)
        {
            Pool pool = pools.Find(p => p.tag == tag);
            if (pool == null)
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return;
            }
            
            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                _poolDictionary[tag].Enqueue(obj);
            }
        }
        
        /// <summary>
        /// Get current pool size
        /// </summary>
        /// <param name="tag">Tag of the object pool</param>
        /// <returns>Current pool size</returns>
        public int GetPoolSize(string tag)
        {
            if (!_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return 0;
            }
            
            return _poolDictionary[tag].Count;
        }
        
        /// <summary>
        /// Add a new pool at runtime
        /// </summary>
        /// <param name="tag">Tag for the pool</param>
        /// <param name="prefab">Prefab to pool</param>
        /// <param name="size">Initial pool size</param>
        public void AddPool(string tag, GameObject prefab, int size)
        {
            if (_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} already exists.");
                return;
            }
            
            Pool newPool = new Pool
            {
                tag = tag,
                prefab = prefab,
                size = size
            };
            
            pools.Add(newPool);
            
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            _poolDictionary.Add(tag, objectPool);
        }
    }
    
    /// <summary>
    /// Component to track pooled objects
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        [HideInInspector]
        public string poolTag;
        
        private void OnDisable()
        {
            if (!string.IsNullOrEmpty(poolTag))
            {
                ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
            }
        }
    }
}