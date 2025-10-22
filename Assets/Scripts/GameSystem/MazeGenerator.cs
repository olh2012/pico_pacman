using UnityEngine;
using System.Collections.Generic;

namespace PacMan.GameSystem
{
    /// <summary>
    /// Generates a maze for the Pac-Man game
    /// </summary>
    public class MazeGenerator : MonoBehaviour
    {
        [Header("Maze Settings")]
        [SerializeField] private int width = 28;
        [SerializeField] private int height = 36;
        [SerializeField] private float cellSize = 1.0f;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject wallPrefab;
        [SerializeField] private GameObject pelletPrefab;
        [SerializeField] private GameObject powerPelletPrefab;
        [SerializeField] private GameObject playerSpawnPoint;
        [SerializeField] private GameObject ghostSpawnPoint;
        
        [Header("Layout")]
        [SerializeField] private TextAsset mazeLayoutFile; // Optional text file defining the maze layout
        
        private int[,] mazeGrid;
        private List<Vector3> pelletPositions = new List<Vector3>();
        private List<Vector3> powerPelletPositions = new List<Vector3>();
        
        // Events
        public System.Action OnMazeGenerated;
        
        private void Start()
        {
            GenerateMaze();
        }
        
        /// <summary>
        /// Generate the maze layout
        /// </summary>
        public void GenerateMaze()
        {
            // Initialize the grid
            mazeGrid = new int[width, height];
            
            // If we have a layout file, use it
            if (mazeLayoutFile != null)
            {
                LoadMazeFromText();
            }
            else
            {
                // Generate a simple maze pattern
                GenerateSimpleMaze();
            }
            
            // Create the visual maze
            CreateVisualMaze();
            
            // Place pellets
            PlacePellets();
            
            // Place power pellets
            PlacePowerPellets();
            
            // Notify that maze generation is complete
            OnMazeGenerated?.Invoke();
            
            Debug.Log("Maze generated");
        }
        
        /// <summary>
        /// Load maze layout from a text file
        /// </summary>
        private void LoadMazeFromText()
        {
            if (mazeLayoutFile == null) return;
            
            string[] lines = mazeLayoutFile.text.Split('\n');
            
            for (int y = 0; y < lines.Length && y < height; y++)
            {
                string line = lines[y];
                for (int x = 0; x < line.Length && x < width; x++)
                {
                    char c = line[x];
                    switch (c)
                    {
                        case '#': // Wall
                            mazeGrid[x, y] = 1;
                            break;
                        case '.': // Pellet
                            mazeGrid[x, y] = 2;
                            pelletPositions.Add(new Vector3(x * cellSize, 0, y * cellSize));
                            break;
                        case 'o': // Power pellet
                            mazeGrid[x, y] = 3;
                            powerPelletPositions.Add(new Vector3(x * cellSize, 0, y * cellSize));
                            break;
                        case 'P': // Player spawn
                            mazeGrid[x, y] = 4;
                            if (playerSpawnPoint != null)
                            {
                                Instantiate(playerSpawnPoint, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
                            }
                            break;
                        case 'G': // Ghost spawn
                            mazeGrid[x, y] = 5;
                            if (ghostSpawnPoint != null)
                            {
                                Instantiate(ghostSpawnPoint, new Vector3(x * cellSize, 0, y * cellSize), Quaternion.identity);
                            }
                            break;
                        default: // Empty space
                            mazeGrid[x, y] = 0;
                            break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Generate a simple maze pattern
        /// </summary>
        private void GenerateSimpleMaze()
        {
            // Create border walls
            for (int x = 0; x < width; x++)
            {
                mazeGrid[x, 0] = 1; // Top border
                mazeGrid[x, height - 1] = 1; // Bottom border
            }
            
            for (int y = 0; y < height; y++)
            {
                mazeGrid[0, y] = 1; // Left border
                mazeGrid[width - 1, y] = 1; // Right border
            }
            
            // Add some internal walls (simplified)
            for (int x = 4; x < width - 4; x += 4)
            {
                for (int y = 4; y < height - 4; y += 4)
                {
                    if (Random.value > 0.5f)
                    {
                        mazeGrid[x, y] = 1;
                    }
                }
            }
            
            // Add pellets in empty spaces
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (mazeGrid[x, y] == 0 && Random.value > 0.7f)
                    {
                        mazeGrid[x, y] = 2;
                        pelletPositions.Add(new Vector3(x * cellSize, 0, y * cellSize));
                    }
                }
            }
        }
        
        /// <summary>
        /// Create the visual representation of the maze
        /// </summary>
        private void CreateVisualMaze()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (mazeGrid[x, y] == 1) // Wall
                    {
                        if (wallPrefab != null)
                        {
                            Vector3 position = new Vector3(x * cellSize, 0.5f, y * cellSize);
                            Instantiate(wallPrefab, position, Quaternion.identity, transform);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// Place regular pellets in the maze
        /// </summary>
        private void PlacePellets()
        {
            foreach (Vector3 position in pelletPositions)
            {
                if (pelletPrefab != null)
                {
                    Instantiate(pelletPrefab, position, Quaternion.identity, transform);
                }
            }
        }
        
        /// <summary>
        /// Place power pellets in the maze
        /// </summary>
        private void PlacePowerPellets()
        {
            foreach (Vector3 position in powerPelletPositions)
            {
                if (powerPelletPrefab != null)
                {
                    Instantiate(powerPelletPrefab, position, Quaternion.identity, transform);
                }
            }
        }
        
        /// <summary>
        /// Get the maze grid data
        /// </summary>
        /// <returns>2D array representing the maze</returns>
        public int[,] GetMazeGrid()
        {
            return mazeGrid;
        }
        
        /// <summary>
        /// Get the width of the maze
        /// </summary>
        /// <returns>Maze width</returns>
        public int GetWidth()
        {
            return width;
        }
        
        /// <summary>
        /// Get the height of the maze
        /// </summary>
        /// <returns>Maze height</returns>
        public int GetHeight()
        {
            return height;
        }
        
        /// <summary>
        /// Get the cell size
        /// </summary>
        /// <returns>Cell size</returns>
        public float GetCellSize()
        {
            return cellSize;
        }
        
        /// <summary>
        /// Check if a position is walkable
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>True if position is walkable</returns>
        public bool IsWalkable(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return false;
                
            return mazeGrid[x, y] != 1; // Not a wall
        }
        
        /// <summary>
        /// Get a random walkable position in the maze
        /// </summary>
        /// <returns>Random walkable position</returns>
        public Vector3 GetRandomWalkablePosition()
        {
            List<Vector3> walkablePositions = new List<Vector3>();
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (IsWalkable(x, y))
                    {
                        walkablePositions.Add(new Vector3(x * cellSize, 0, y * cellSize));
                    }
                }
            }
            
            if (walkablePositions.Count > 0)
            {
                return walkablePositions[Random.Range(0, walkablePositions.Count)];
            }
            
            return Vector3.zero;
        }
    }
}