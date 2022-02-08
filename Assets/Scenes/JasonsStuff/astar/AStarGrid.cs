using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
}
