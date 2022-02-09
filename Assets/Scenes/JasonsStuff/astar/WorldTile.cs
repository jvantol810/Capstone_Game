using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldTile 
{
    public bool walkable;
    public Vector3 worldPosition;
    public Transform parentTilemap;
    public AStarGrid parentGrid;
    public int gCost; //walking cost from the start node -- 10 for nondiagonal movement, 14 for diagonal movement
    public int hCost; //heuristic cost to reach the end node, basically an estimate to the goal
    //f cost represents the sum of gCost and hCost
    public int gridX, gridY, cellX, cellY;

    public WorldTile(bool walkable, int gridX, int gridY, AStarGrid parentGrid)
    {
        this.walkable = walkable;
        this.gridX = gridX;
        this.gridY = gridY;
        this.parentGrid = parentGrid;
        this.parentTilemap = parentGrid.map.transform;
        //Add top neighbor
        neighborLocations.Add(new Vector2Int(gridX + 1, gridY));
        //Add top-right neighbor
        neighborLocations.Add(new Vector2Int(gridX + 1, gridY + 1));
        //Add right neighbor
        neighborLocations.Add(new Vector2Int(gridX + 1, gridY));
        //Add lower-right neighbor
        neighborLocations.Add(new Vector2Int(gridX + 1, gridY - 1));
        //Add lower neighbor
        neighborLocations.Add(new Vector2Int(gridX, gridY - 1));
        //Add lower-left neighbor
        neighborLocations.Add(new Vector2Int(gridX - 1, gridY - 1));
        //Add left neighbor
        neighborLocations.Add(new Vector2Int(gridX - 1, gridY));
        //Add top-left neighbor
        neighborLocations.Add(new Vector2Int(gridX - 1, gridY + 1));
    }

    public Vector2Int globalPosition { get { return new Vector2Int((int)parentTilemap.position.x + gridX, (int)parentTilemap.position.y + gridY); } }
    

    public int fCost { get { return gCost + hCost; } }
    public List<Vector2Int> neighborLocations = new List<Vector2Int>();
    public WorldTile parent;
}
