using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class RoomGen : MonoBehaviour
{
    public Tilemap map;
    public AStarGrid aStarGrid;
    public Tile[] tiles;
    public GameObject enemyPrefab;
    private List<RoomPrefab> prefabs =new List<RoomPrefab>();

    private int mapHeight;
    private int mapWidth;
    // public int roomsMax;
    // public int roomSeeds;

    public Vector2Int[] roomCenters;

    [Header("Walk Settings")] public int floorMax;
    public bool testing = false;
    public bool seeded = false;
    public int seed;
    
    private int walkX;
    private int walkY;
    private List<Vector3Int> tilesWalked = new List<Vector3Int>();

    //Variables for pathfinding debugging, currently these are set to random values each frame to test the efficiency of the algorithm
    Vector2Int pathStart;
    Vector2Int pathEnd;
    Vector2Int[] path;
    // Start is called before the first frame update
    void Start()
    {
        pathStart = new Vector2Int(Random.Range(0, LevelSettings.MapData.width - 1), Random.Range(0, LevelSettings.MapData.height - 1));
        pathEnd = new Vector2Int( Random.Range(0, LevelSettings.MapData.width - 1), Random.Range(0, LevelSettings.MapData.height - 1));
        //Set the map size according to the values put in the inspector -- NOT DOING THIS CURRENTLY, BUT YOU CAN IF YOU WANT TO SET MAP SIZE IN INSPECTOR FOR CONVENINECE
        //LevelSettings.MapData.SetSize(mapHeight, mapWidth);

        //Get the map size from the LevelSettings script
        mapHeight = LevelSettings.MapData.height;
        mapWidth = LevelSettings.MapData.width;

        //Initialize the aStar grid
        aStarGrid.InitGrid();

        //Set the aStarGrid in LevelSettings to be the aStarGrid assigned in inspector (this way any class can now reference the aStarGrid from LevelSettings)
        LevelSettings.MapData.SetAStarGrid(aStarGrid);

        //Generate the world map
        GenerateWorld();
        
        DoPrefabStuff();

        //Generate a random path on the aStarGrid
        path = aStarGrid.GetRandomPath();
    }
    bool pathfinding = false;

    private void Update()
    {
        if (testing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateWorld();
            }
            if (pathfinding)
            {

                //WorldTile[] route = aStarGrid.FindPath(path[0], path[1]);
                //path = aStarGrid.GetRandomPath();
                aStarGrid.GetWalkableTileWithinRange(path[0], 1, 5);
                
            }
        }
        



    }

    private void InitMap()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                //Add an unwalkable tile to the tilemap and aStar grid
                AddTileToMap(false, new Vector2Int(j, i), tiles[0]);

            }
        }//end of outer loop
    }
    
    private void GenerateWorld()
    {
        if (seeded)
        {
            Random.InitState(seed);
        }
        else
        {
            int randSeed = Random.Range(0, 99999);
            seed = randSeed;
            Random.InitState(randSeed);
        }
        InitMap();
        DrunkenWalkGen();
        //Add enemies
        for (int i = 0; i < 3; i++)
        {
            Vector2 spawnPoint = aStarGrid.GetRandomWalkableTile().centerWorldPosition;
            Instantiate(enemyPrefab, new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
        }
        LevelSettings.MapData.activeAStarGrid.MarkNeighbors();
    }
    
    //Randomly 'Walks' to create walkable area for player
    private void DrunkenWalkGen()
    {
        tilesWalked.Clear();
        walkX = Mathf.RoundToInt(mapWidth / 2);
        walkY = Mathf.RoundToInt(mapHeight / 2);

        //Seed of walk
        AddTileToMap(true, new Vector2Int(walkX, walkY), tiles[1]);

        //Rand walk
        for (int i = 0; i < floorMax; i++)
        {
            //Pick random direction
            if (PickRandomDirection())
            {
                //turn new tile into floor tile
                AddTileToMap(true, new Vector2Int(walkX, walkY), tiles[1]);
                tilesWalked.Add(new Vector3Int(walkX, walkY, 0));
            }
            
        }
    }

    public void AddTileToMap(bool walkable, Vector2Int position, Tile tileObject)
    {
        //Set the tile onto the world's tilemap
        map.SetTile(new Vector3Int(position.x, position.y, 0), tileObject);
        //Add the tile data to the aStarGrid for pathfinding
        aStarGrid.AddTile(new WorldTile(walkable, position.x, position.y, aStarGrid));
    }
    
    //Overloaded to accept WorldTiles  as parameter 
    public void AddTileToMap(WorldTile worldTile, Tile tileObject)
    {
        //Set the tile onto the world's tilemap
        map.SetTile(new Vector3Int(worldTile.gridX, worldTile.gridY, 0), tileObject);
        //Add the tile data to the aStarGrid for pathfinding
        aStarGrid.AddTile(worldTile);
    }
    private bool PickRandomDirection()
    {
        int lastIndex = tilesWalked.Count - 1;
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 1:
                walkX += 1;
                break;
            case 2:
                walkX -= 1;
                break;
            case 3:
                walkY += 1;
                break;
            case 4:
                walkY -= 1;
                break;
        }
        if (walkX < mapWidth - 1 && walkX > 0 && walkY < mapHeight - 1 && walkY > 0)
        {
            return true;
        }
        
        //if you're going to walk out of bounds roll back to previous tile and pick new direction
        walkX = tilesWalked[lastIndex].x;
        walkY = tilesWalked[lastIndex].y;
        return false;
        
        
    }


    private void DoPrefabStuff()
    {
        ConvertToPrefab();
        PlacePrefab();
    }
    
    //Converts String array into a Tilemap prefab
    private void ConvertToPrefab()
    {
        String[,] stringArray = FileParse.ParseTextFile();
        RoomPrefab roomTiles = new RoomPrefab();
        
        //Loops through 2d string array and converts it into a 'RoomPrefab'
        for (int i = 0; i < stringArray.GetLength(0); i++)
        {
            for (int j = 0; j < stringArray.GetLength(1); j++)
            {
                //This trims the 2d array down to just the parts that represent tiles
                if (stringArray[j, i] != null)
                {
                    //A "1" is a wall, "0" for floors 
                    if (stringArray[j, i] == "1")
                    {
                        //roomTiles.prefabTiles[j][i] = new WorldTile(false, 0, 0, aStarGrid);
                        roomTiles.prefabTiles[j].Insert(i , new WorldTile(false, 0, 0, aStarGrid));
                    }
                    else
                    {
                        
                        //roomTiles.prefabTiles[j][i] = new WorldTile(true, 0, 0, aStarGrid);
                        roomTiles.prefabTiles[j].Insert(i , new WorldTile(false, 0, 0, aStarGrid));
                    }

                }
            }
        }
        //Add to prefab list
        prefabs.Add(roomTiles);
    }

    private void PlacePrefab()
    {
        List<Vector2Int> usablePoints = FindPrefabSpace();
        int totalTiles = 0;
        //Loops through the prefabs tiles setting their positions to an empty area 
        for (int i = 0; i < prefabs[0].prefabTiles.Count; i++)
        {
            for (int j = 0; j < prefabs[0].prefabTiles[i].Count; j++)
            {
                prefabs[0].prefabTiles[j][i].gridX = usablePoints[totalTiles].x;
                prefabs[0].prefabTiles[j][i].gridY = usablePoints[totalTiles].y;
                totalTiles++;
                
                Debug.Log(prefabs[0].prefabTiles[j][i]);
                //If walkable set the map to floor if not walls and add to map and astar grid
                if (prefabs[0].prefabTiles[j][i].walkable)
                {
                    AddTileToMap(prefabs[0].prefabTiles[j][i], tiles[2]);
                }
                else
                {
                    AddTileToMap(prefabs[0].prefabTiles[j][i], tiles[2]);
                }
            }
        }
        Debug.Log("Total:" + totalTiles);
    }

    private List<Vector2Int> FindPrefabSpace()
    {
        List<Vector2Int> prefabPoints = new List<Vector2Int>();
        int pWidth = prefabs[0].prefabTiles[0].Count;
        int pHeight = prefabs[0].prefabTiles.Count;
        List<Vector2Int> unwalkableLocations = aStarGrid.GetUnwalkableTileLocations();
        
        //Get random point 
        int randIndex = Random.Range(0, unwalkableLocations.Count);
        Vector2Int randomPoint = unwalkableLocations[randIndex];

        for (int i = randomPoint.y; i < randomPoint.y + pHeight; i++)
        {
            for (int j = randomPoint.x; j < randomPoint.x + pWidth; j++)
            {
                if (randomPoint.y + pHeight >= mapHeight - 1 || randomPoint.x + pWidth >= mapWidth)
                {
                    FindPrefabSpace();
                }
                if (aStarGrid.GetTileAt(new Vector2Int(j,i)).walkable == false)//Getting weird Out of bound here sometimes
                {
                    prefabPoints.Add(new Vector2Int(j, i));
                }
                else
                {
                    FindPrefabSpace();
                }
            }
        }

        return prefabPoints;
    }
}
