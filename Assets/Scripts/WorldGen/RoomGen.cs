using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class RoomGen : MonoBehaviour
{
    public Tilemap map;
    public AStarGrid aStarGrid;
    public Tile[] tiles;
    //public GameObject enemyPrefab; new spawning function should render this obsolete
    public GameObject playerPrefab;
    //List for allRooms and list of their centers for connecting them to main map
    private List<RoomPrefab> allRooms = new List<RoomPrefab>();
    public List<Vector2Int> roomCenters = new List<Vector2Int>();
    
    public List<Vector2Int> prefabPoints = new List<Vector2Int>();
    
    private int mapHeight;
    private int mapWidth;
    // public int roomsMax;
    // public int roomSeeds;
    
    [Header("Debugging Settings")]
    [Tooltip("When enabled space bar regens level")]
    public bool testing = false;
    
    [Header("Proc Gen Settings")] 
    public int floorMax;
    public bool seeded = false;
    public int seed;
    public GameObject[] monsterPrefabs; // Move to another file along with SpawnEntity function

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
        
        FileParse.ParseWholeFolder();
        
        //Converts prefab arrays into lists
        ConvertToPrefab();
        
        //Generate the world map
        GenerateWorld();

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
            if (Input.GetKeyDown(KeyCode.L))
            {
                SaveGame();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                LoadGame();
                aStarGrid.InitGrid();

                //Set the aStarGrid in LevelSettings to be the aStarGrid assigned in inspector (this way any class can now reference the aStarGrid from LevelSettings)
                LevelSettings.MapData.SetAStarGrid(aStarGrid);

                FileParse.ParseWholeFolder();
                GenerateWorld();
            }
            if(Input.GetKeyDown(KeyCode.P))
            {
                ResetData();
            }
        }
    }

    void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/MySaveData.dat");
        SaveData data = new SaveData();
        data.savedSeed = seed;
        //data.savedFloat = floatToSave;
        data.savedBool = true;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
        Debug.Log("Saved Seed: " + seed);
    }
    void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath
                       + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            seed = data.savedSeed;
            //floatToSave = data.savedFloat;
            seeded = data.savedBool;
            Debug.Log("Game data loaded!");
            Debug.Log("Loaded seed: " + seed);
        }
        else
            Debug.LogError("There is no save data!");
    }
    void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
                  + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath
                              + "/MySaveData.dat");
            seed = 0;
            //floatToSave = 0.0f;
            seeded = false;
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");
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
        DrunkenWalkGen(true);
        MultiPrefabGeneration();
        SpawnEntity();
        
        //New spawning function should render this chunk obsolete
        /*for (int i = 0; i < 3; i++)
        {
            Vector2 spawnPoint = aStarGrid.GetRandomWalkableTile().centerWorldPosition;
            Instantiate(enemyPrefab, new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
        }*/
        Vector2 spawnPoint2 = aStarGrid.GetRandomWalkableTile().centerWorldPosition;
        Instantiate(playerPrefab, new Vector3(spawnPoint2.x, spawnPoint2.y, 0), Quaternion.identity);
    }
    
    //Randomly 'Walks' to create walkable area for player
    private void DrunkenWalkGen(bool jumpAtEdge)
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
            if (PickRandomDirection(jumpAtEdge))
            {
                //turn new tile into floor tile
                AddTileToMap(true, new Vector2Int(walkX, walkY), tiles[RandomIndex(tiles.Length,1)]);
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
    
    //Overloaded to accept WorldTiles as parameter 
    public void AddTileToMap(WorldTile worldTile, Tile tileObject)
    {
        //Set the tile onto the world's tilemap
        map.SetTile(new Vector3Int(worldTile.gridX, worldTile.gridY, 0), tileObject);
        //Add the tile data to the aStarGrid for pathfinding
        aStarGrid.AddTile(worldTile);
    }
    private bool PickRandomDirection(bool jumpAtEdge=false)
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
        if (jumpAtEdge)
        {
            walkX = tilesWalked[RandomIndex(tilesWalked.Count)].x;
            walkY = tilesWalked[RandomIndex(tilesWalked.Count)].y;
        }
        else
        {
            walkX = tilesWalked[lastIndex].x;
            walkY = tilesWalked[lastIndex].y;
        }

        return false;
    }
    
    //Converts String array into a Tilemap prefab
    private void ConvertToPrefab()//Works for multi
    {
        var stringArrays = FileParse.allTextPrefabs;

        for(int k = 0; k < stringArrays.Count; k++)
        {
            RoomPrefab roomTiles = new RoomPrefab();
            roomTiles.InitInnerLists(FileParse.listDepth[k]);
            //Loops through 2d string array and converts it into a 'RoomPrefab'
            for (int i = 0; i < stringArrays[k].GetLength(0); i++)
            {
                for (int j = 0; j < stringArrays[k].GetLength(1); j++)
                {
                    //Debug.Log(roomTiles.prefabTiles.Count);
                    //Debug.Log(stringArray[j, i] +"J:" + j + "I:" + i);
                    //This trims the 2d array down to just the parts that represent tiles
                    if (stringArrays[k][j, i] != null)
                    {
                        //A "1" is a wall, "0" for floors 
                        if (stringArrays[k][j, i] == "1")
                        {
                            roomTiles.prefabTiles[j].Insert(i , new WorldTile(false, 0, 0, aStarGrid));
                        }
                        else
                        {
                            roomTiles.prefabTiles[j].Insert(i , new WorldTile(true, 0, 0, aStarGrid));
                        }
                    }
                }
            }
            
            //Debug.Log("Room added");
            //Add to prefab list
            allRooms.Add(roomTiles);
        }
    }

    //I don't like doing it this way
    private void ConnectPrefab(List<Vector2Int> prefabPlacePoints)
    {
        Vector2 prevTile;
        Vector2 newTile;

        foreach(var center in roomCenters)
        {
            WorldTile closestTile = aStarGrid.GetNearestWalkableTile(center, prefabPlacePoints);
            int targetX = closestTile.gridX;
            int targetY = closestTile.gridY;
            
            if (center.x < targetX)
            {
                for (int i = center.x; i < targetX; i++)
                {
                    prevTile = center;
                    if (i != 0)
                    {
                        prevTile = new Vector2(i-1, center.y);
                    }
                
                    newTile = prevTile + Vector2.right;

                    AddTileToMap(true, Vector2Int.RoundToInt(newTile), tiles[1]);
                }
            }
            else
            {
                for (int i = center.x; i > targetX; i--)
                {
                    prevTile = center;
                    if (i != 0)
                    {
                        prevTile = new Vector2(i-1, center.y);
                    }
                
                    newTile = prevTile + Vector2.left;

                    AddTileToMap(true, Vector2Int.RoundToInt(newTile), tiles[1]);
                }
            }

            if (center.y < targetY)
            {
                for (int i = center.y; i < targetY; i++)
                {
                    prevTile = new Vector2(targetX,center.y);
                    if (i != 0)
                    {
                        prevTile = new Vector2(targetX, i-1);
                    }
                
                    newTile = prevTile + Vector2.up;

                    AddTileToMap(true, Vector2Int.RoundToInt(newTile), tiles[1]);
                }
            }
            else
            {
                for (int i = center.y; i > targetY; i--)
                {
                    prevTile = new Vector2(targetX,center.y);
                    if (i != 0)
                    {
                        prevTile = new Vector2(targetX, i - 1);
                    }
                
                    newTile = prevTile + Vector2.up;

                    AddTileToMap(true, Vector2Int.RoundToInt(newTile), tiles[1]);
                }
            }
        }
        
    }//Works multi

    //Functions for multiprefab generation below
    private void MultiPrefabGeneration()
    {
        List<Vector2Int> prefabPlacePoints = new List<Vector2Int>();
        
        foreach (var room in allRooms)
        {
            prefabPlacePoints = MultiFindPrefabSpace(room);
            MultiPlacePrefab(room, prefabPlacePoints);
            ConnectPrefab(prefabPlacePoints);
        }
    }

    private List<Vector2Int> MultiFindPrefabSpace(RoomPrefab room)
    {
        List<Vector2Int> multiPrefabPoints = new List<Vector2Int>();
        
        int pWidth = room.prefabTiles[0].Count;
        int pHeight = room.prefabTiles.Count;

        List<Vector2Int> unwalkableLocations = aStarGrid.GetUnwalkableTileLocations(new Vector2((float)pWidth, (float)pHeight));
        
        //Get random point
        int randIndex = Random.Range(0, unwalkableLocations.Count - 1);
        Vector2Int randomPoint = unwalkableLocations[randIndex];
        while (randomPoint.x + pWidth > LevelSettings.MapData.width - 2 && randomPoint.y + pHeight > LevelSettings.MapData.height - 2)
        {
            //Debug.Log("point out of bounds");
            randIndex = Random.Range(0, unwalkableLocations.Count - 1);
            randomPoint = unwalkableLocations[randIndex];
        }
        
        int upperX = randomPoint.x + pWidth;
        int upperY = randomPoint.y + pHeight;
        
        while (!GetMultiPrefabPoints(multiPrefabPoints, randomPoint, upperY, upperX))
        {
            randIndex = Random.Range(0, unwalkableLocations.Count - 1);
            randomPoint = unwalkableLocations[randIndex];
            while (randomPoint.x + pWidth > LevelSettings.MapData.width - 2 && randomPoint.y + pHeight > LevelSettings.MapData.height - 2)
            {
                //Debug.Log("point out of bounds");
                randIndex = Random.Range(0, unwalkableLocations.Count - 1);
                randomPoint = unwalkableLocations[randIndex];
            }
        
            upperX = randomPoint.x + pWidth;
            upperY = randomPoint.y + pHeight;
            
        }
        
        //Grab room center
        //Calculate these in a different way
        int xCenter = (randomPoint.x + upperX)/2;
        int yCenter = (randomPoint.y + upperY) / 2;
        roomCenters.Add(new Vector2Int(xCenter,yCenter));

        return multiPrefabPoints;

    }
    
    private bool GetMultiPrefabPoints(List<Vector2Int> multiPrefabPoints, Vector2Int randomPoint, int upperY, int upperX)
    {
        //Loop through all the points to make sure their valid for placing 
        for (int i = randomPoint.y; i < upperY; i++)
        {
            for (int j = randomPoint.x; j < upperX; j++)
            {
                //There is already an out of bounds catch in PlacePrefab but for some reason it doesn't catch everything?
                if (i >= LevelSettings.MapData.height - 2 || j >= LevelSettings.MapData.width - 2)
                {
                    //Debug.Log("OOB GPP");
                    return false;
                }
                
                //If the tile is valid add it to the list of points
                if (aStarGrid.GetTileAt(new Vector2Int(j, i)).walkable == false)
                {
                    //Debug.Log("J:" + j + "I"+ i);
                    //aStarGrid.PlaceMarker(new Vector2Int(j, i), Color.red);
                    multiPrefabPoints.Add(new Vector2Int(j, i));
                    //Debug.Log("HIT2");
                }
                else
                {
                    multiPrefabPoints.Clear();
                    return false;
                }
                
            }
        }
        
        return true;
    }
    
    private void MultiPlacePrefab(RoomPrefab room, List<Vector2Int> prefabPlacePoints)
    {
        int totalTiles = 0;
        
        //Loops through the allRooms tiles setting their positions to a valid area 
        for (int i = 0; i < room.prefabTiles.Count; i++)
        {
            for (int j = 0; j < room.prefabTiles[i].Count; j++)
            {
                room.prefabTiles[j][i].gridX = prefabPlacePoints[totalTiles].x;
                room.prefabTiles[j][i].gridY = prefabPlacePoints[totalTiles].y;
                totalTiles++;
                
                
                //If walkable set the map to floor if not walls and add to map and astar grid
                if (room.prefabTiles[j][i].walkable)
                {
                    AddTileToMap(room.prefabTiles[j][i], tiles[1]);
                }
                else
                {
                    AddTileToMap(room.prefabTiles[j][i], tiles[0]);
                }
            }
        }
    }

    //This should be in it's own file, but I'm not sure how to port over the grids
    private void SpawnEntity()
    {
        List<Vector2Int> walkableLocations = aStarGrid.GetWalkableTileLocations();
        List<Vector2Int> spawnLocations = new List<Vector2Int>();

        float xMin = 3;
        float xMax = 13;
        float yMin = 3;
        float yMax = 13;

        //Get random points for spawning
        for (int i = 0; i < LevelSettings.MapData.mapSpawnPoints; i++)
        {
            int spawnIndex = Random.Range(0, walkableLocations.Count - 1);
            spawnLocations.Add(walkableLocations[spawnIndex]);
            //aStarGrid.PlaceMarker(walkableLocations[spawnIndex], Color.yellow);
        }

        foreach (var spawnCoord in spawnLocations)
        {
            for (int i = 0; i < LevelSettings.MapData.MobPerSpawn; i++)
            {
                float xOffset = Random.Range(xMin, xMax);
                float yOffset = Random.Range(yMin, yMax);

                //Vector2 spawn = new Vector2(spawnCoord.x + xOffset, spawnCoord.y+yOffset);
                Vector2 spawn = new Vector2(spawnCoord.x, spawnCoord.y);
                Debug.Log("SPAWNLOCATION:" + spawn);

                if (spawn.x > LevelSettings.MapData.width)
                    spawn.x = LevelSettings.MapData.width - 2;
                if (spawn.y > LevelSettings.MapData.height)
                    spawn.y = LevelSettings.MapData.height - 2;
                if (spawn.x < 0)
                    spawn.x = 3;
                if (spawn.y < 0)
                    spawn.y = 3;
                
                //Debug.Log("X:" +spawn.x + "Y:" + spawn.y);
                if (aStarGrid.GetTileAt(Vector2Int.RoundToInt(spawn)).walkable)
                {
                    //aStarGrid.PlaceMarker(spawn, Color.green);
                    Instantiate(monsterPrefabs[RandomIndex(monsterPrefabs.Length)], new Vector3(spawn.x, spawn.y, 0), Quaternion.identity);
                }
                else
                {
                    //aStarGrid.PlaceMarker(aStarGrid.GetNearestWalkableTile(spawn).centerWorldPosition, Color.green);
                    spawn = aStarGrid.GetNearestWalkableTile(spawn).worldPosition;
                    var tileSpawn = aStarGrid.GetNearestWalkableTile(spawn);
                    Instantiate(monsterPrefabs[RandomIndex(monsterPrefabs.Length)], new Vector3(tileSpawn.centerWorldPosition.x, tileSpawn.centerWorldPosition.y, 0), Quaternion.identity);
                }
            }
            
        }
    }

    private int RandomIndex(int maxIndex, int minIndex=0)
    {
        return Random.Range(minIndex, maxIndex);
    }
    
    //You are now entering the depreciated zone
    //Tread with caution
    
    //**Depreciated functions**
    
    [Obsolete(" GeneratePrefabs is deprecated, please use MultiPrefabGeneration instead.", true )]
    private void GeneratePrefabs()
    {
        //int count = 0;
        ConvertToPrefab();
        
        //Single prefab functions
       
        //roomCenters.Add(FindPrefabSpace(room, count));
        //PlacePrefab();
        //ConnectPrefab();
    }
    
    //Adds prefab to map and astar grid
    [Obsolete(" PlacePrefab is deprecated, please use MultiPlacePrefab instead.")]
    private void PlacePrefab()
    {
        List<Vector2Int> usablePoints = prefabPoints;
        int totalTiles = 0;
        
        //Do this loop for every prefab in the list
        //Loops through the allRooms tiles setting their positions to an empty area 
        for (int i = 0; i < allRooms[0].prefabTiles.Count; i++)
        {
            for (int j = 0; j < allRooms[0].prefabTiles[i].Count; j++)
            {
                allRooms[0].prefabTiles[j][i].gridX = usablePoints[totalTiles].x;
                allRooms[0].prefabTiles[j][i].gridY = usablePoints[totalTiles].y;
                totalTiles++;
                
                
                //If walkable set the map to floor if not walls and add to map and astar grid
                if (allRooms[0].prefabTiles[j][i].walkable)
                {
                    AddTileToMap(allRooms[0].prefabTiles[j][i], tiles[1]);
                }
                else
                {
                    AddTileToMap(allRooms[0].prefabTiles[j][i], tiles[0]);
                }
            }
        }
    }

    //Finds space for prefab to fit
    [Obsolete(" FindPrefabSpace is deprecated, please use MultiFindPrefabSpace instead.")]
    private Vector2Int FindPrefabSpace(RoomPrefab room)
    {
        // Debug.Log("Called FindPrefabSpace");
        int pWidth = room.prefabTiles[0].Count;
        int pHeight = room.prefabTiles.Count;
        //Debug.Log("wid:" + pWidth + "Hei:"+ pHeight);

        List<Vector2Int> unwalkableLocations = aStarGrid.GetUnwalkableTileLocations();
        
        //Get random point 
        int randIndex = Random.Range(0, unwalkableLocations.Count - 1);
        Vector2Int randomPoint = unwalkableLocations[randIndex];

        while (randomPoint.x + pWidth > LevelSettings.MapData.width - 2 && randomPoint.y + pHeight > LevelSettings.MapData.height - 2)
        {
            //Debug.Log("point out of bounds");
            randIndex = Random.Range(0, unwalkableLocations.Count - 1);
            randomPoint = unwalkableLocations[randIndex];
        }

        int upperX = randomPoint.x + pWidth;
        int upperY = randomPoint.y + pHeight;
        
        int xCenter = randomPoint.x + (pWidth / 2);
        int yCenter = randomPoint.y + (pHeight / 2);

       // Debug.Log("Random X:" + randomPoint.x + "Random Y:" + randomPoint.y);
       Debug.Log("upperX:" + upperX + "UpperY:" + upperY);

        if (!GetPrefabPoints(randomPoint, upperY, upperX))
        {
            //Debug.Log("Points not valid.");
            return FindPrefabSpace(room);
        }
        
        //Returns center ish of the room
        return new Vector2Int(xCenter, yCenter);
    }
    
    //Checks the space to make sure the points are valid
    [Obsolete(" GetPrefabPoints is deprecated, please use GetMultiPrefabPoints instead.")]
    private bool GetPrefabPoints(Vector2Int randomPoint, int upperY, int upperX)
    {
        //Loop through all the points to make sure their valid for placing 
        for (int i = randomPoint.y; i < upperY; i++)
        {
            for (int j = randomPoint.x; j < upperX; j++)
            {
                //There is already an out of bounds catch in PlacePrefab but for some reason it doesn't catch everything?
                if (i >= LevelSettings.MapData.height - 2 || j >= LevelSettings.MapData.width - 2)
                {
                    //Debug.Log("OOB GPP");
                    return false;
                }
                
                //If the tile is valid add it to the list of points
                if (aStarGrid.GetTileAt(new Vector2Int(j, i)).walkable == false)
                {
                    //Debug.Log("J:" + j + "I"+ i);
                    //aStarGrid.PlaceMarker(new Vector2Int(j, i), Color.red);
                    prefabPoints.Add(new Vector2Int(j, i));
                    //Debug.Log("HIT2");
                }
                else
                {
                    return false;
                }
                
            }
        }
        
        return true;
    }

}
[Serializable]
class SaveData
{
    public int savedSeed;
    //public float savedFloat;
    public bool savedBool;
}

