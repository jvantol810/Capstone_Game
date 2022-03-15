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
            prefabTiles.Add(new List<WorldTile>());
        }
    }
}
