using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorldTile 
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gCost; //walking cost from the start node -- 10 for nondiagonal movement, 14 for diagonal movement
    public int hCost; //heuristic cost to reach the end node, basically an estimate to the goal
    //f cost represents the sum of gCost and hCost
    public int gridX, gridY, cellX, cellY;

    public WorldTile(bool walkable, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int fCost { get { return gCost + hCost; } }
    public List<WorldTile> myNeighbours;
    public WorldTile parent;
}
