using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomPrefab
{
    public List<List<WorldTile>> prefabTiles = new List<List<WorldTile>>();

    public RoomPrefab()
    {
        for (int i = 0; i < 5; i++)
        {
            prefabTiles.Capacity = 25;
            prefabTiles.Add(new List<WorldTile>(25));
        }
    }
    
}
