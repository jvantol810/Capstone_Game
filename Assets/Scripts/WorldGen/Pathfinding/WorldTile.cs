using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[System.Serializable]
public class WorldTile 
{
    public bool walkable;
    public bool occupied = false;
    public Transform parentTilemap;
    public AStarGrid parentGrid;
    public float gCost; //walking cost from the start node -- 10 for nondiagonal movement, 14 for diagonal movement
    public float hCost; //heuristic cost to reach the end node, basically an estimate to the goal
    //f cost represents the sum of gCost and hCost
    public int gridX, gridY, cellX, cellY;

    public WorldTile(bool walkable, int gridX, int gridY, AStarGrid parentGrid)
    {
        this.walkable = walkable;
        this.gridX = Mathf.Clamp(gridX, 0, parentGrid.tileRow.Length - 1);
        this.gridY = Mathf.Clamp(gridY, 0, parentGrid.tileRow[0].tileColumn.Length - 1);
        this.parentGrid = parentGrid;
        this.parentTilemap = parentGrid.map.transform;

        List<Vector2Int> tempNeighborLocations = new List<Vector2Int>();
        Vector2Int[] directions = new Vector2Int[8]{Vector2Int.up, new Vector2Int(1, 1), Vector2Int.right, new Vector2Int(1, -1),
            Vector2Int.down, new Vector2Int(-1, -1), Vector2Int.left, new Vector2Int(-1, 1)};
        //Vector2Int[] directions = new Vector2Int[4]{Vector2Int.up, Vector2Int.right,
        //    Vector2Int.down, Vector2Int.left};
        for (int i = 0; i < directions.Length; i++)
        {
            if (!parentGrid.tileIsOutOfBounds(this.gridPosition + directions[i]))
            {
                //WorldTile neighboringTile = parentGrid.GetTileAt(this.gridPosition + directions[i]);
                //if (neighboringTile.walkable) { tempNeighborLocations.Add(neighboringTile.gridPosition); }
                tempNeighborLocations.Add(this.gridPosition + directions[i]);
                //Debug.Log("Adding direction: " + directions[i]);
                //parentGrid.PlaceMarker(tempNeighborLocations[i], Color.yellow);
            }
            
        }
        this.neighborLocations = tempNeighborLocations.ToArray();
    }


    public bool TileEquals(WorldTile tile)
    {
        return gridX == tile.gridX && gridY == tile.gridY;
    }
    public Vector2Int gridPosition { get { return new Vector2Int(gridX, gridY); } }
    public Vector2 worldPosition { get { return new Vector2(parentTilemap.position.x + gridX, parentTilemap.position.y + gridY); } }
    public Vector2 centerWorldPosition { get { return parentTilemap.GetComponent<Tilemap>().GetCellCenterWorld((Vector3Int)gridPosition); } }
   
    public float fCost { get { return gCost + hCost; } }
    public Vector2Int[] neighborLocations;
    //public List<WorldTile> neighborTiles = new List<WorldTile>();
    public WorldTile[] neighborTiles { get { return GetNeighborTiles(); } }
    public WorldTile GetCheapestNeighbor()
    {
        WorldTile cheapestNeighbor = neighborTiles[0];
        for (int i = 0; i < neighborTiles.Length - 1; i++)
        {
            //Debug.Log("Neighbor fCost: " + neighborTiles[i].fCost);
            if(neighborTiles[i+1].fCost < cheapestNeighbor.fCost)
            {
                //Debug.Log("Next tile cheaper, updating!");
                cheapestNeighbor = neighborTiles[i + 1];
            }
        }
        return cheapestNeighbor;
    }
    public string neighborsToString()
    {
        string neighborString = "Neighboring Tiles: \n" + neighborLocations.Length;
        for (int i = 0; i < neighborLocations.Length; i++)
        {
            neighborString += "( " + neighborLocations[i].x + ", " + neighborLocations[i].y + " ) \n";
        }
        return neighborString;
    }
    private WorldTile[] GetNeighborTiles()
    {
        //WorldTile[] neighborTiles = new WorldTile[neighborLocations.Length];
        List<WorldTile> tmpNeighborTileList = new List<WorldTile>();
        for (int i = 0; i < neighborLocations.Length - 1; i++)
        {
            /*neighborTiles[i] = (parentGrid.GetTileAt(neighborLocations[i])); */
            WorldTile potentialNeighbor = (parentGrid.GetTileAt(neighborLocations[i]));
            if(potentialNeighbor != null)
            {
                tmpNeighborTileList.Add(potentialNeighbor);
            }
        }
        return tmpNeighborTileList.ToArray();
        //return neighborTiles;
    }

    public void RemoveNeighborTile(WorldTile neighborToRemove)
    {
        //Create a temporary copy of the neighbor locations array as a list
        List<Vector2Int> tmpNeighborLocationList = new List<Vector2Int>();
        for (int i = 0; i < neighborLocations.Length - 1; i++)
        {
            tmpNeighborLocationList.Add(neighborLocations[i]);
        }
        //If the list of neighbor locations contains the grid position of the neighbor to remove, remove it from the list of neighbor locations
        if (tmpNeighborLocationList.Contains(neighborToRemove.gridPosition))
        {
            tmpNeighborLocationList.Remove(neighborToRemove.gridPosition);
        }
        //Set the neighbor locations to be the temporary list (which has the appropriate neighbor removed)
        neighborLocations = tmpNeighborLocationList.ToArray();
    }
    public override string ToString()
    {
        string tileStr = "Tile at grid position: ( " + gridX + ", " + gridY + " ) \n";
        tileStr += "Tile at world position : ( " + worldPosition.x + ", " + worldPosition.y + " ) \n";
        return tileStr;
    }
    public WorldTile parent;
}
