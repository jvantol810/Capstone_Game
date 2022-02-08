using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelSettings
{
    [System.Serializable]
    public static class MapData
    {
        public static int height = 25;
        public static int width = 25;

        public static void SetSize(int height, int width)
        {
            MapData.height = height;
            MapData.width = width;
        }
    }
}
