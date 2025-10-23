using UnityEngine;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Wall component for maze walls
    /// </summary>
    public class Wall : MonoBehaviour
    {
        [Header("Wall Settings")]
        [SerializeField] private bool isDestructible = false;
        
        private void Start()
        {
            // Ensure the wall has a collider
            if (GetComponent<Collider>() == null)
            {
                gameObject.AddComponent<BoxCollider>();
            }
            
            // Set the tag for easy identification
            gameObject.tag = "Wall";
        }
        
        /// <summary>
        /// Check if the wall is destructible
        /// </summary>
        /// <returns>True if wall is destructible</returns>
        public bool IsDestructible()
        {
            return isDestructible;
        }
        
        /// <summary>
        /// Destroy the wall if it's destructible
        /// </summary>
        public void DestroyWall()
        {
            if (isDestructible)
            {
                Destroy(gameObject);
            }
        }
    }
}