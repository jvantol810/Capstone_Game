using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGrid : MonoBehaviour
{
    //public Tilemap walkableMap;
    public Tilemap unwalkableMap;
    [SerializeField]
    public MapData mapData;
    private void Start()
    {
        CreateWorldTiles();
    }

    //Create a 2D array of tiles from the map
    public /*WorldTile[,]*/ void CreateWorldTiles()
    {
        WorldTile[,] tileArray = new WorldTile[mapData.height, mapData.height];

        for (int i = 0; i < mapData.height; i++)
        {
            for (int j = 0; j < mapData.width; j++)
            {
                
                //Get the tile found at the current point in the tilemap
                //TileBase tile = walkableMap.GetTile(new Vector3Int(j, i, 1));
                
                bool tileWalkable;
                //If there's a tile on the unwalkable tilemap at this point, set the tile to be unwalkable
                if (unwalkableMap.GetTile(new Vector3Int(j, i, 1)) != null) {
                    tileWalkable = false;
                }
                else
                {
                    tileWalkable = true;
                }
                //Construct a WorldTile class and add it to the TileArray
                WorldTile newTile = new WorldTile(tileWalkable, j, i);
                tileArray[j, i] = newTile;
            }
        }//end of outer loop

        ////Add the newTile to the tileArray
        ////tileArray[x, y] = newTile;
        //Debug.Log("Tile found at coordinates -- X: " + x + "  Y: " + y);

        //Print contents of tileArray
        foreach (WorldTile tile in tileArray)
        {
            Debug.Log("Tile found at (x: " + tile.gridX + ", y: " + tile.gridY + ") -- Walkable: " + tile.walkable);
        }
    }
}
