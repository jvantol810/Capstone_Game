using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.Events;
using System.Collections.ObjectModel;
public class AStarGrid : MonoBehaviour
{
    //public Tilemap walkableMap;
    public Tilemap map;
    
    [SerializeField]
    public WorldTileRow[] tileRow;
    //Contains the locations for all tiles occupied by entities
    public List<CreatureController> enemiesOnGrid = new List<CreatureController>();
    public List<WorldTile> walkableTiles = new List<WorldTile>();
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
        //Subscribe to the OnEnemyMove event with the setTileToUnwalkable function (so each time the enemy moves into a new location, its tile is set as unwalkable)
        GameEvents.OnEnemyMove.AddListener(UpdateOccupiedTile);
    }

    public void FindEnemiesOnGrid()
    {
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObj in enemyObjs)
        {
            enemiesOnGrid.Add(enemyObj.GetComponent<CreatureController>());
        }
    }
    public void UpdateOccupiedTile(Vector2Int tilePosition)
    {
        if(enemiesOnGrid.Count == 0) { FindEnemiesOnGrid(); }
        //List<WorldTile> walkableTiles = GetWalkableTiles();
        List<Vector2Int> enemyGridPositions = new List<Vector2Int>();
        foreach (CreatureController enemy in enemiesOnGrid)
        {
            //Get tile at enemy position
            enemyGridPositions.Add(ConvertWorldPositionToTilePosition(enemy.transform.position));
        }
        foreach (WorldTile tile in walkableTiles)
        {
            //If the tile is occupied by an enemy, set it to occupied. Otherwise, set it to unoccupied.
            if (enemyGridPositions.Contains(tile.gridPosition))
            {
                tile.occupied = true;
            }
            else
            {
                tile.occupied = false;
            }
        }
        //WorldTile occupiedTile = GetTileAt(tilePosition);
        //occupiedTile.occupied = true;
        //if (!occupiedTiles.Contains(occupiedTile)) { occupiedTiles.Add(occupiedTile); }
        //foreach (WorldTile tile in occupiedTiles)
        //{
        //    PlaceMarker(tile.centerWorldPosition, Color.yellow);
        //}
        //foreach (WorldTile neighbor in occupiedTile.neighborTiles)
        //{
        //    if (neighbor.occupied)
        //    {
        //        //No longer occupied
        //        neighbor.occupied = false;
        //        occupiedTiles.Remove(neighbor);
        //    }
        //}
    }

    public void UpdateAllOccupiedTiles(Vector2Int dummyVariable)
    {
        //Each time an enemy moves, it broadcasts what tile it is currently standing on. Update the boolean for occupied for ONLY THAT TILE
        //Get the tile at that location and set its boolean of occupied to true
        //How do you know when the enemy exits the tile? Look at the neighboring tiles surrounding him and see if ANY are set to occupied. IF they are, test to see if THEY ARE ACTUALLY OCCUPIED BY COMPARING ENEMY LOCATIONS TO THEM.

    }
    //Add a new WorldTile object to the two dimensional tileArray. This does not check if a tile already exists here, so it will replace it.
    public void AddTile(WorldTile newTile)
    {
        tileRow[newTile.gridX].tileColumn[newTile.gridY] = newTile;
        if (newTile.walkable) { walkableTiles.Add(newTile); }
        //Debug.Log("Tile added at (x: " + newTile.gridX + ", y: " + newTile.gridY + ") -- Walkable: " + newTile.walkable);
    }

    public WorldTile GetNearestWalkableTile(Vector2 startingPosition)
    {
        WorldTile nearestTile = null;
        float minDistance = 0;
        foreach (WorldTile tile in walkableTiles)
        {
            float distance = Vector2.Distance(startingPosition, tile.centerWorldPosition);
            if (minDistance == 0 || distance < minDistance)
            {
                minDistance = distance;
                nearestTile = tile;
            }
        }
        return nearestTile;
    }
    
    //Overloaded with list of tiles to ignore
    public WorldTile GetNearestWalkableTile(Vector2 startingPosition, List<Vector2Int> ignoreTiles)
    {
        WorldTile nearestTile = null;
        float minDistance = 0;
        foreach (WorldTile tile in walkableTiles)
        {
            //We want to ignore all the tiles in the prefab 
            if (!ignoreTiles.Contains(tile.gridPosition))
            {
                float distance = Vector2.Distance(startingPosition, tile.centerWorldPosition);
                if (minDistance == 0 || distance < minDistance)
                {
                    minDistance = distance;
                    nearestTile = tile;
                } 
            }
            
        }
        return nearestTile;
    }

   

    public WorldTile GetTileAt(Vector2Int tilePosition)
    {
        return tileRow[tilePosition.x].tileColumn[tilePosition.y];
    }

    public Vector2Int ConvertWorldPositionToTilePosition(Vector2 worldPosition)
    {
        //Convert from the world position to the tile position
        //Vector3Int cellPosition = map.WorldToCell((Vector3)worldPosition);
        //Subtract 0.5 from each coordinate because of offset
        Vector2Int cellPosition = new Vector2Int(Mathf.RoundToInt(worldPosition.x - 0.5f), Mathf.RoundToInt(worldPosition.y - 0.5f));
        //Place a red marker on the position
        //PlaceMarker((Vector2Int)cellPosition, Color.red);

        //Return the tile at the cell position
        return (Vector2Int)cellPosition;
    }


    public List<WorldTile> SetWalkableTiles()
    {
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

    public void MarkNeighbors()
    {
        foreach (WorldTile tile in walkableTiles)
        {
            PlaceMarker(tile.centerWorldPosition, Color.yellow);
        }
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
    
    public List<Vector2Int> GetUnwalkableTileLocations()
    {
        List<Vector2Int> unwalkableTileLocations = new List<Vector2Int>();
        //Iterate through each tile in the world map and collect the ones that are walkable into a list.
        for (int i = 0; i < tileRow.Length; i++)
        {
            for (int j = 0; j < tileRow[i].tileColumn.Length; j++)
            {
                if (tileRow[i].tileColumn[j].walkable == false)
                {
                    unwalkableTileLocations.Add(tileRow[i].tileColumn[j].gridPosition);
                }
            }
        }
        //Return the list of Unwalkable tiles
        return unwalkableTileLocations;
    }
    
    public List<Vector2Int> GetUnwalkableTileLocations(Vector2 sizeOfRoom)
    {
        List<Vector2Int> unwalkableTileLocations = new List<Vector2Int>();
        //Iterate through each tile in the world map and collect the ones that are walkable into a list.
        for (int i = (int)sizeOfRoom.x + 1; i < tileRow.Length - sizeOfRoom.x; i++)
        {
            for (int j = (int)sizeOfRoom.y + 1; j < tileRow[i].tileColumn.Length - sizeOfRoom.y; j++)
            {
                if (tileRow[i].tileColumn[j].walkable == false)
                {
                    unwalkableTileLocations.Add(tileRow[i].tileColumn[j].gridPosition);
                }
            }
        }
        //Return the list of Unwalkable tiles
        return unwalkableTileLocations;
    }


    //Returns a WorldTile that is walkable within the range
    public WorldTile GetWalkableTileWithinRange(Vector2Int startTilePosition, float minTileDistance, float maxTileDistance)
    {
        
        //Create a list of all the tiles you could move, out to the max distance
        List<Vector2Int> nearbyWalkableTiles = new List<Vector2Int>();
        WorldTile start = GetTileAt(startTilePosition);
        //If the start position is unwalkable, log an error message and return
        if(start.walkable == false) {
            //Debug.log start
            Debug.LogError("Starting in unwalkable tile: " + startTilePosition);
            Debug.LogError("ERROR WITH GETTING WALKABLE TILE WITHIN RANGE. YOU CANNOT HAVE AN UNWALKABLE START POSITION."); return null; 
        }
        WorldTile currentNode = start;
        float distanceFromStart = 0;
        Vector2Int randPosition = Vector2Int.zero;
        while (distanceFromStart <= maxTileDistance)
        {
            //Iterate through start's neighbors, adding the walkable tiles from it to the list
            foreach (WorldTile neighbour in currentNode.neighborTiles)
            {
                //If the neighbor is NOT walkable, skip
                if (!neighbour.walkable) continue;
                //If the neighbor IS walkable, add it to the list
                nearbyWalkableTiles.Add(neighbour.gridPosition);
            }
            //At the end of going through each walkable neighbor, increment distance.
            distanceFromStart++;

            //There needs to be a chance that you return now -- if you have met the minimum distance requirement, return on a 50% chance
            if(distanceFromStart >= minTileDistance && Random.Range(0, 1) >= 0.5)
            {
                
                //Break out of the while loop, returning a random position
                break;
            }

            //Change currentNode to be a random neighbor
            currentNode = GetTileAt(nearbyWalkableTiles[Random.Range(0, nearbyWalkableTiles.Count - 1)]);
        }

        //We've hit the max distance. Choose a random tile location from the list of walkable tiles and return it.
        //In case the starting tile was added to the list, remove it.
        nearbyWalkableTiles.RemoveAll(tilePos => tilePos == startTilePosition);
        Debug.Log("Number of possible tiles to move to: " + nearbyWalkableTiles.Count);
        randPosition = nearbyWalkableTiles[Random.Range(0, nearbyWalkableTiles.Count - 1)];
        if (randPosition == startTilePosition)
        {
            Debug.Log("Rand equals start!");
        }
        //PlaceMarker(randPosition, Color.green);
        Debug.Log("Moved this many tiles: " + distanceFromStart);
        return GetTileAt(randPosition);
    }

    public WorldTile GetRandomWalkableTile()
    {
        int randomIndex = Random.Range(0, walkableTiles.Count);
        return walkableTiles[randomIndex];
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

    //Sets tiles containing moving, solid entities (like enemies) as unwalkable
    public void SetTileAsUnwalkable(Vector2Int tileLocation)
    {
        GetTileAt(tileLocation).walkable = false;
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
                Handles.Label(new Vector3(tile.centerWorldPosition.x, tile.centerWorldPosition.y, 0), "("+tile.gridX +", " + tile.gridY + "),\n" + 
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
    public int maxMarkerCount = 3;
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

    public WorldTile[] FindPath(Vector2Int startPosition, Vector2Int endPosition, bool ignoreWalkableTiles=true)
    {
        RemoveAllMarkers();
        WorldTile startNode = GetTileAt(startPosition);
        //PlaceMarker(startPosition, startColor);
        WorldTile targetNode = GetTileAt(endPosition);
        //PlaceMarker(endPosition, endColor);
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
                if (!neighbour.walkable && ignoreWalkableTiles || closedSet.Contains(neighbour)) continue;

                float newMovementCostToNeighbour = currentNode.gCost + Vector2.Distance(currentNode.centerWorldPosition, neighbour.centerWorldPosition);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        //Add the neighbor to the open list because we want to explore it
                        openSet.Add(neighbour);
                    //PlaceMarker(neighbour.worldPosition, Color.cyan);
                }
            }
        }
        return new WorldTile[] { };
    }

    public Vector2[] FindPath(Vector2 worldStartPosition, Vector2 worldEndPosition, bool ignoreWalkableTile=true)
    {
        RemoveAllMarkers();
        Vector2Int startPosition = ConvertWorldPositionToTilePosition(worldStartPosition);
        WorldTile startNode = GetTileAt(startPosition);
        Debug.Log("Starting at: " + startPosition);
        //PlaceMarker(startPosition, startColor);
        WorldTile targetNode = GetTileAt(ConvertWorldPositionToTilePosition(worldEndPosition));
        //PlaceMarker(worldEndPosition, endColor);
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
                WorldTile[] pathTiles = RetracePath(startNode, targetNode);
                List<Vector2> finalPath = new List<Vector2>();
                for (int i = 0; i < pathTiles.Length - 1; i++)
                {
                    finalPath.Add(pathTiles[i].centerWorldPosition);
                }
                return finalPath.ToArray();
            }

            foreach (WorldTile neighbour in currentNode.neighborTiles)
            {
                if (!neighbour.walkable && ignoreWalkableTile|| neighbour.occupied || closedSet.Contains(neighbour)) continue;
                //PlaceMarker(neighbour.centerWorldPosition, Color.cyan);
                float newMovementCostToNeighbour = currentNode.gCost + Vector2.Distance(currentNode.centerWorldPosition, neighbour.centerWorldPosition);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        //Add the neighbor to the open list because we want to explore it
                        openSet.Add(neighbour);
                    
                }
            }
        }
        return new Vector2[] { };
    }

    public Vector2 ConvertFromGridToWorldPosition(Vector2Int gridPosition)
    {
        return map.GetCellCenterWorld((Vector3Int)gridPosition);
    }
    
}
