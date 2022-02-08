using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGrid : MonoBehaviour
{
    //public Tilemap walkableMap;
    public Tilemap map;
    [SerializeField]
    public MapData mapData;
    [SerializeField]
    public WorldTileRow[] tileRow;

    [System.Serializable]
    public struct WorldTileRow
    {
        public WorldTile[] tileColumn;
    }
    private void Start()
    {
        //CreateWorldTiles();
        tileRow = new WorldTileRow[mapData.width];
        //Go through tile array and create each column of height mapHeight
        for(int i = 0; i < tileRow.Length; i++)
        {
            tileRow[i].tileColumn = new WorldTile[mapData.height];
        }
    }

    public void AddTile(WorldTile newTile)
    {
        tileRow[newTile.gridX].tileColumn[newTile.gridY] = newTile;
        Debug.Log("Tile added at (x: " + newTile.gridX + ", y: " + newTile.gridY + ") -- Walkable: " + newTile.walkable);
    }

    public void ReplaceTileAt(Vector2Int originalTilePosition, WorldTile newTile)
    {
        tileRow[originalTilePosition.x].tileColumn[newTile.gridY] = newTile;
        Debug.Log("Tile at (x: " + originalTilePosition.x + ", y: " + originalTilePosition.y + ") replaced with new tile.");
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
                if (map.GetTile(new Vector3Int(j, i, 1)) != null) {
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
