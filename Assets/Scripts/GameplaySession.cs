using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplaySession
{
    public static bool isPlaying = false;
    public static float playerAliveTime = 0;
    public static int levelsCompleted = 0;
    public static string playerEquippedHat = "";
    public static int seed;

    public static void ClearGameSession()
    {
        playerAliveTime = 0;
        levelsCompleted = 0;
    }
}
