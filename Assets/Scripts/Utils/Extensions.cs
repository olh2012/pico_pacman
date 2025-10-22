using UnityEngine;
using System.Collections.Generic;

namespace PacMan.Utils
{
    /// <summary>
    /// Extension methods and utility functions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Shuffle a list using Fisher-Yates shuffle algorithm
        /// </summary>
        /// <typeparam name="T">Type of elements in the list</typeparam>
        /// <param name="list">List to shuffle</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        
        /// <summary>
        /// Get a random element from a list
        /// </summary>
        /// <typeparam name="T">Type of elements in the list</typeparam>
        /// <param name="list">List to get element from</param>
        /// <returns>Random element from the list</returns>
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                return default(T);
                
            return list[Random.Range(0, list.Count)];
        }
        
        /// <summary>
        /// Check if a layer mask contains a specific layer
        /// </summary>
        /// <param name="layerMask">Layer mask to check</param>
        /// <param name="layer">Layer to check for</param>
        /// <returns>True if layer mask contains the layer</returns>
        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return (layerMask.value & (1 << layer)) != 0;
        }
        
        /// <summary>
        /// Add a layer to a layer mask
        /// </summary>
        /// <param name="layerMask">Layer mask to modify</param>
        /// <param name="layer">Layer to add</param>
        /// <returns>Modified layer mask</returns>
        public static LayerMask AddLayer(this LayerMask layerMask, int layer)
        {
            return layerMask | (1 << layer);
        }
        
        /// <summary>
        /// Remove a layer from a layer mask
        /// </summary>
        /// <param name="layerMask">Layer mask to modify</param>
        /// <param name="layer">Layer to remove</param>
        /// <returns>Modified layer mask</returns>
        public static LayerMask RemoveLayer(this LayerMask layerMask, int layer)
        {
            return layerMask & ~(1 << layer);
        }
        
        /// <summary>
        /// Clamp a vector's magnitude to a maximum value
        /// </summary>
        /// <param name="vector">Vector to clamp</param>
        /// <param name="maxLength">Maximum length</param>
        /// <returns>Clamped vector</returns>
        public static Vector3 ClampMagnitude(this Vector3 vector, float maxLength)
        {
            if (vector.sqrMagnitude > maxLength * maxLength)
            {
                return vector.normalized * maxLength;
            }
            return vector;
        }
        
        /// <summary>
        /// Get the closest point on a line segment to a given point
        /// </summary>
        /// <param name="point">Point to find closest point to</param>
        /// <param name="lineStart">Start of line segment</param>
        /// <param name="lineEnd">End of line segment</param>
        /// <returns>Closest point on line segment</returns>
        public static Vector3 ClosestPointOnLineSegment(this Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 line = lineEnd - lineStart;
            float lineLength = line.magnitude;
            line.Normalize();
            
            float dot = Vector3.Dot(point - lineStart, line);
            dot = Mathf.Clamp(dot, 0f, lineLength);
            
            return lineStart + line * dot;
        }
        
        /// <summary>
        /// Rotate a vector around the Y axis
        /// </summary>
        /// <param name="vector">Vector to rotate</param>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Rotated vector</returns>
        public static Vector3 RotateAroundY(this Vector3 vector, float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(radians);
            float sin = Mathf.Sin(radians);
            
            return new Vector3(
                vector.x * cos - vector.z * sin,
                vector.y,
                vector.x * sin + vector.z * cos
            );
        }
        
        /// <summary>
        /// Convert a 3D position to a 2D grid position
        /// </summary>
        /// <param name="position">3D position</param>
        /// <param name="cellSize">Size of each grid cell</param>
        /// <returns>Grid position</returns>
        public static Vector2Int ToGridPosition(this Vector3 position, float cellSize)
        {
            return new Vector2Int(
                Mathf.RoundToInt(position.x / cellSize),
                Mathf.RoundToInt(position.z / cellSize)
            );
        }
        
        /// <summary>
        /// Convert a 2D grid position to a 3D world position
        /// </summary>
        /// <param name="gridPosition">Grid position</param>
        /// <param name="cellSize">Size of each grid cell</param>
        /// <returns>World position</returns>
        public static Vector3 ToWorldPosition(this Vector2Int gridPosition, float cellSize)
        {
            return new Vector3(
                gridPosition.x * cellSize,
                0f,
                gridPosition.y * cellSize
            );
        }
    }
}