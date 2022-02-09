using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class AStarGrid : MonoBehaviour
{
    //public Tilemap walkableMap;
    public Tilemap map;

    [SerializeField]
    public WorldTileRow[] tileRow;

    [System.Serializable]
    public struct WorldTileRow
    {
        public WorldTile[] tileColumn;
    }
    private void Awake()
    {
       
    }

    public void InitGrid()
    {
        //CreateWorldTiles();
        tileRow = new WorldTileRow[LevelSettings.MapData.width];
        //Go through tile array and create each column of height mapHeight
        for (int i = 0; i < tileRow.Length; i++)
        {
            tileRow[i].tileColumn = new WorldTile[LevelSettings.MapData.height];
        }
    }
    //Add a new WorldTile object to the two dimensional tileArray. This does not check if a tile already exists here, so it will replace it.
    public void AddTile(WorldTile newTile)
    {
        tileRow[newTile.gridX].tileColumn[newTile.gridY] = newTile;
        Debug.Log("Tile added at (x: " + newTile.gridX + ", y: " + newTile.gridY + ") -- Walkable: " + newTile.walkable);
    }

    public WorldTile GetTileAt(Vector2Int tilePosition)
    {
        return tileRow[tilePosition.x].tileColumn[tilePosition.y];
    }
    public int GetDistance(WorldTile tileA, WorldTile tileB)
    {
        int distanceX = Mathf.Abs(tileA.gridX - tileB.gridX);
        int distanceY = Mathf.Abs(tileA.gridY - tileB.gridY);

        if(distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }

    List<WorldTile> RetracePath(WorldTile start, WorldTile end)
    {
        //Create a list of the world tiles called the path, and set current to the ending tile
        List<WorldTile> path = new List<WorldTile>();
        WorldTile current = end;

        //While the current is not back at the start of the path, add the tile and move towards the start
        while(current != start)
        {
            path.Add(current);
            current = current.parent;
        }
        
        path.Reverse();
        return path;
    }

    //-- A STAR PATHFINDING PSEUDOCODE --//
    //    OPEN_LIST
    //CLOSED_LIST
    //ADD start_cell to OPEN_LIST

    //LOOP
    //    current_cell = cell in OPEN_LIST with the lowest F_COST
    //    REMOVE current_cell from OPEN_LIST
    //    ADD current_cell to CLOSED_LIST

    //IF current_cell is finish_cell
    //    RETURN

    //FOR EACH adjacent_cell to current_cell
    //    IF adjacent_cell is unwalkable OR adjacent_cell is in CLOSED_LIST
    //        SKIP to the next adjacent_cell

    //    IF new_path to adjacent_cell is shorter OR adjacent_cell is not in OPEN_LIST
    //        SET F_COST of adjacent_cell
    //        SET parent of adjacent_cell to current_cell
    //        IF adjacent_cell is not in OPEN_LIST
    //            ADD adjacent_cell to OPEN_LIST

    private void OnDrawGizmos()
    {
        DisplayTileCoordinates();
    }

    private void DisplayTileCoordinates()
    {
        //Go through tile array and create each column of height mapHeight
        for (int i = 0; i < tileRow.Length; i++)
        {
            for (int j = 0; j < tileRow[i].tileColumn.Length; j++) {
                WorldTile tile = tileRow[i].tileColumn[j];
                Handles.Label(new Vector3(tile.globalPosition.x, tile.globalPosition.y, 0), "("+tile.gridX +", " + tile.gridY + ")");
            }
        }
    }
    public List<WorldTile> FindPath(Vector2Int startPos, Vector2Int endPos)
    {
        WorldTile start = GetTileAt(startPos);
        WorldTile end = GetTileAt(endPos);

        List<WorldTile> openSet = new List<WorldTile>();
        List<WorldTile> path = new List<WorldTile>();
        HashSet<WorldTile> closedSet = new HashSet<WorldTile>();
        openSet.Add(start);

        while(openSet.Count > 0)
        {
            WorldTile current = openSet[0];
            for(int i = 0; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < current.fCost || openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost)
                {
                    current = openSet[i];
                }
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if(current == end)
            {
                path = RetracePath(start, end);
                return path;
            }

            //Get the neighbors based on their location
            List<WorldTile> neighbors = new List<WorldTile>();
            foreach(Vector2Int neighborLocation in current.neighborLocations)
            {
                Debug.Log("Neighbor location: " + neighborLocation);
                neighbors.Add(GetTileAt(neighborLocation));
            }

            foreach (WorldTile neighbour in neighbors)
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                int newMovementCostToNeighbour = current.gCost + GetDistance(current, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, end);
                    neighbour.parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return path;
    }
}
