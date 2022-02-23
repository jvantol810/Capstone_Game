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
        //Debug.Log("Tile added at (x: " + newTile.gridX + ", y: " + newTile.gridY + ") -- Walkable: " + newTile.walkable);
    }

    public WorldTile GetTileAt(Vector2Int tilePosition)
    {
        return tileRow[tilePosition.x].tileColumn[tilePosition.y];
    }
    public List<WorldTile> GetWalkableTiles()
    {
        List<WorldTile> walkableTiles = new List<WorldTile>();
        //Iterate through each tile in the world map and collect the ones that are walkable into a list.
        for (int i = 0; i < tileRow.Length; i++)
        {
            for (int j = 0; j < tileRow[i].tileColumn.Length; j++)
            {
                if (tileRow[i].tileColumn[j].walkable)
                {
                    walkableTiles.Add(tileRow[i].tileColumn[j]);
                }
            }
        }
        //Return the list of walkable tiles
        return walkableTiles;
    }

    public List<Vector2Int> GetWalkableTileLocations()
    {
        List<Vector2Int> walkableTileLocations = new List<Vector2Int>();
        //Iterate through each tile in the world map and collect the ones that are walkable into a list.
        for (int i = 0; i < tileRow.Length; i++)
        {
            for (int j = 0; j < tileRow[i].tileColumn.Length; j++)
            {
                if (tileRow[i].tileColumn[j].walkable)
                {
                    walkableTileLocations.Add(tileRow[i].tileColumn[j].gridPosition);
                }
            }
        }
        //Return the list of walkable tiles
        return walkableTileLocations;
    }

    public Vector2Int[] GetRandomPath()
    {
        List<Vector2Int> walkableTileLocations = GetWalkableTileLocations();
        int randomIndex_1 = Random.Range(0, walkableTileLocations.Count);
        int randomIndex_2 = Random.Range(0, walkableTileLocations.Count);
        while(randomIndex_1 == randomIndex_2)
        {
            randomIndex_1 = Random.Range(0, walkableTileLocations.Count);
            randomIndex_2 = Random.Range(0, walkableTileLocations.Count);
        }
        Vector2Int[] randomPath =
        {
            walkableTileLocations[randomIndex_1],
            walkableTileLocations[randomIndex_2]
        };
        Debug.Log("Random path calculated: (" + randomPath[0] + ", " + randomPath[1] + ")");
        return randomPath;
    }

    public bool tileIsOutOfBounds(Vector2Int tilePosition)
    {
        return tilePosition.x + 1 >= tileRow.Length || tilePosition.x < 0 || tilePosition.y >= tileRow.Length || tilePosition.y < 0;
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

    WorldTile[] RetracePath(WorldTile start, WorldTile end)
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
        return path.ToArray();
    }

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
                Handles.Label(new Vector3(tile.worldPosition.x, tile.worldPosition.y, 0), "("+tile.gridX +", " + tile.gridY + "),\n" + 
                    "G: " + tile.gCost + ", H: " + tile.hCost + ", F: " + tile.fCost);
            }
        }
    }

    bool done = false;

    [Header("Markers for Debugging")]
    public GameObject pointMarker;
    public Color startColor;
    public Color endColor;
    public Color closedMarkerColor;
    public Color openMarkerColor;
    public void PlaceMarker(Vector2 position, Color color)
    {
        GameObject marker = Instantiate(pointMarker, (Vector3)position, Quaternion.identity);
        marker.GetComponent<SpriteRenderer>().color = color;
    }

    void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");
        foreach (GameObject m in markers)
        {
            Destroy(m);
        }
    }


    public void PrintOpenSet(List<WorldTile> set)
    {
        string tilesString = "";
        foreach (WorldTile tile in set)
        {
            tilesString += "( " + tile.gridPosition.x + ", " + tile.gridPosition.y + " ) \n";
        }
        //Debug.Log("Open Set Contains Tiles At: \n" + tilesString);
    }

    public WorldTile[] FindPath(Vector2Int startPosition, Vector2Int endPosition)
    {
        RemoveAllMarkers();
        WorldTile startNode = GetTileAt(startPosition);
        PlaceMarker(startPosition, startColor);
        WorldTile targetNode = GetTileAt(endPosition);
        PlaceMarker(endPosition, endColor);
        List<WorldTile> openSet = new List<WorldTile>();
        HashSet<WorldTile> closedSet = new HashSet<WorldTile>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            WorldTile currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                WorldTile[] finalPath = RetracePath(startNode, targetNode);
                return finalPath;
            }

            foreach (WorldTile neighbour in currentNode.neighborTiles)
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;

                float newMovementCostToNeighbour = currentNode.gCost + Vector2.Distance(currentNode.worldPosition, neighbour.worldPosition);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        //Add the neighbor to the open list because we want to explore it
                        openSet.Add(neighbour);
                        PlaceMarker(neighbour.worldPosition, Color.cyan);
                }
            }
        }

        return new WorldTile[] { };
    }
}
