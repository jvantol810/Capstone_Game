using System;
using System.IO;
using UnityEngine;


public static class FileParse 
{
    private static String path;
    private static String jsonString;
    
    //This is a bigger area than rooms should need
    private static string[,] lines = new String[20, 20];

    //Reads in text file splits into a String 2d array
    public static String[,] ParseTextFile()
    {
        String textPath = Application.persistentDataPath + "/TextPrefab.txt";
        String input = File.ReadAllText(textPath);

        
        int i = 0, j = 0;
        
        foreach (string row in input.Split('\n'))
        {
            j = 0;
            foreach (string col in row.Trim().Split(','))
            {
                lines[j, i] = col.Trim();
                j++;
            }

            i++;
        }

        return lines;
    }
    
    //Reads in from JSON -- Doesn't Work -.-
    private static void ParseJSON()
    {
        path = Application.persistentDataPath + "/TestPrefab.json";
        
        if (File.Exists(path))
        {
            Debug.Log("Should've worked");
            jsonString = File.ReadAllText(path);
            PrefabArray prefab = JsonUtility.FromJson<PrefabArray>(jsonString);
            Debug.Log(prefab.jsonTiles[0,1]);
        }
        else
        {
            Debug.Log("Didn't work");
        }
    }

    private class PrefabArray
    {
        public String[,] jsonTiles = {{"one","two"}};
}
}
