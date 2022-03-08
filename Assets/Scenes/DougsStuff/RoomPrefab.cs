using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomPrefab
{
    public List<List<WorldTile>> prefabTiles = new List<List<WorldTile>>();
    public int maxListSize = 25;
    
    public void InitInnerLists(int roomHeight)
    {
        for (int i = 0; i < roomHeight ; i++)
        {
            prefabTiles.Capacity = maxListSize;
            prefabTiles.Add(new List<WorldTile>(maxListSize));
        }
    }
}
