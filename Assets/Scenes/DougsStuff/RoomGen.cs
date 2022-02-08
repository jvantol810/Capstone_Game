using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


public class RoomGen : MonoBehaviour
{
    public Tilemap map;
    public AStarGrid aStarGrid;
    public Tile[] tiles;


    public int mapHeight;
    public int mapWidth;
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

    
    
    // Start is called before the first frame update
    void Start()
    {
        //Set the map size according to the values put in the inspector -- NOT DOING THIS CURRENTLY, BUT YOU CAN IF YOU WANT TO SET MAP SIZE IN INSPECTOR FOR CONVENINECE
        //LevelSettings.MapData.SetSize(mapHeight, mapWidth);

        //Get the map size from the LevelSettings script
        mapHeight = LevelSettings.MapData.height;
        mapWidth = LevelSettings.MapData.width;

        //Initialize the aStar grid
        aStarGrid.InitGrid();
        //Generate the world map
        GenerateWorld();
    }

    private void Update()
    {
        if (testing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateWorld();
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
    }

    //Generation for square like rooms **WIP**
    private void CreateRooms()
    {
       /* for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                int rand = Random.Range(0, 100);
                if (rand <= 1)
                {
                    if (roomSeeds < roomsMax)
                    {
                        map.SetTile(new Vector3Int(j,i,0), tiles[1]);
                        roomSeeds++;
                    }
                }
            }
        }//end of outer loop*/
       
       


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
        aStarGrid.AddTile(new WorldTile(walkable, position.x, position.y));
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
}
