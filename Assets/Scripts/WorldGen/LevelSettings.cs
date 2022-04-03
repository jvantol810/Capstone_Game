using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public static class LevelSettings
{
    [System.Serializable]
    public static class MapData
    {
        public static int height = 50;
        public static int width = 50;
        public static AStarGrid activeAStarGrid;
        public static int mapSpawnPoints = 1; //Originally 3
        public static int MobPerSpawn = 3; // Originally 3
        public static void SetSize(int height, int width)
        {
            MapData.height = height;
            MapData.width = width;
        }

        public static void SetAStarGrid(AStarGrid aStarGrid)
        {
            activeAStarGrid = aStarGrid;
        }

        public static void InitializeGrid(Tilemap map)
        {
            //activeAStarGrid.InitGrid(map);
            activeAStarGrid.InitGrid();
        }
    }
}
