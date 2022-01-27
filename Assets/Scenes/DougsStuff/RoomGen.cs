using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RoomGen : MonoBehaviour
{
    public Tilemap map;
    public Tile[] tiles;

    public int mapHeight;
    public int mapWidth;
    public int roomsMax;
    public int roomSeeds;
    
    public Vector2Int[] roomCenters;

    [Header("Walk Settings")] public int floorMax;

    
    
    // Start is called before the first frame update
    void Start()
    {
        InitMap();
        DrunkenWalkGen();
    }

    private void InitMap()
    {
        for (int i = -mapHeight; i < mapHeight; i++)
        {
            for (int j = -mapWidth; j < mapWidth; j++)
            {
                map.SetTile(new Vector3Int(j,i,0), tiles[0]);
            }
        }//end of outer loop
    }

    //Generation for square like rooms **WIP**
    private void CreateRooms()
    {
        for (int i = -mapHeight; i < mapHeight; i++)
        {
            for (int j = -mapWidth; j < mapWidth; j++)
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
        }//end of outer loop
       
       


    }

    //Randomly 'Walks' to create walkable area for player
    private void DrunkenWalkGen()
    {
        int walkX;
        int walkY;
        
        walkX = (int)map.cellBounds.center.x;
        walkY = (int)map.cellBounds.center.y;
        
        //Seed of walk
        map.SetTile(new Vector3Int(walkX, walkY,0), tiles[1]);
        
        //Make walk bounded within map height and width
        
        //Pick random direction to walk
        for (int i = 0; i < floorMax; i++)
        {
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
            
            //turn new tile into floor tile
            map.SetTile(new Vector3Int(walkX, walkY,0), tiles[1]);
        }
    }
}
