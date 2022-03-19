using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomPrefab
{
    public List<List<WorldTile>> prefabTiles = new List<List<WorldTile>>();
    
    
    public void InitInnerLists(int roomHeight)
    {
        for (int i = 0; i < roomHeight ; i++)
        {
            List<WorldTile> newList = new List<WorldTile>();
            prefabTiles.Add(newList);
        }
    }
}
