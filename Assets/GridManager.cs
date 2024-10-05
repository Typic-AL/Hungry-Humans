using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // Grid size
    public int gridSize = 10;

    // Grid cell size
    public float cellSize = 10f;

    // 2D array to store grid cells
    private GridCell[,] gridCells;

    // Dictionary to store food items
    //private Dictionary<GameObject, GridCell> foodItems = new Dictionary<GameObject, GridCell>();

    private void Start()
    {
        // Initialize the grid cells
        int gridSizeX = (int)(59 / cellSize);
        int gridSizeZ = (int)(59 / cellSize);
        gridCells = new GridCell[gridSizeX, gridSizeZ];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                gridCells[x, z] = new GridCell(x, z);
            }
        }
    }

    // Add a food item to the grid
    public void AddFoodItem(GameObject foodItem)
    {
        // Calculate the grid cell coordinates
        int gridX = (int)(foodItem.transform.position.x / cellSize);
        int gridZ = (int)(foodItem.transform.position.z / cellSize);

        // Add the food item to the grid cell
        gridCells[gridX, gridZ].AddFoodItem(foodItem);
    }

    // Remove a food item from the grid

    public GridCell GetEmptyGridCell()
    {
        for (int x = 0; x < gridCells.GetLength(0); x++)
        {
            for (int z = 0; z < gridCells.GetLength(1); z++)
            {
                if (gridCells[x, z].foodItems.Count == 0)
                {
                    return gridCells[x, z];
                }
            }
        }
        return null;
    }

    public GridCell GetGridCell(Vector3 position)
    {
        int gridX = (int)(position.x / cellSize);
        int gridZ = (int)(position.z / cellSize);
        if (gridX >= 0 && gridX < gridCells.GetLength(0) && gridZ >= 0 && gridZ < gridCells.GetLength(1))
        {
            return gridCells[gridX, gridZ];
        }
        else
        {
            return null;
        }
    }

    public GridCell GetCellAtPosition(Vector3 position)
    {
        // Calculate the grid cell coordinates
        int gridX = (int)(position.x / cellSize);
        int gridZ = (int)(position.z / cellSize);

        // Return the grid cell at the calculated coordinates
        return gridCells[gridX, gridZ];
    }
}

// Simple grid cell class
public class GridCell
{
    public int x, z;
    public List<GameObject> foodItems = new List<GameObject>();

    public GridCell(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    public void AddFoodItem(GameObject foodItem)
    {
        foodItems.Add(foodItem);
    }

    public void RemoveFoodItem(GameObject foodItem)
    {
        foodItems.Remove(foodItem);
    }

    
}
