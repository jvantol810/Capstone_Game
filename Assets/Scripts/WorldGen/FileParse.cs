using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditorInternal.VersionControl;
#endif
using UnityEngine;


public static class FileParse 
{
    private static String textPath = Application.persistentDataPath + "/Prefabs";
    
    private static String path;
    private static String jsonString;

    public static List<int> listDepth = new List<int>();
    
    
    public static List<String[,]> allTextPrefabs = new List<string[,]>();

    //Reads in text file splits into a String 2d array
    private static String[,] ParseTextFile(String file)
    {
        //IMPORTANT
        //The formatting of a room prefab has to be in a square shape see TestPrefab.txt for reference

        
        //Reads each prefab file
        String input = File.ReadAllText(file);
        //Debug.Log(input);
        
        //This is a bigger area than rooms should need
        String[,] inputLines = new String[15, 15];
          
        int i = 0, j = 0;
        
        //Reads each line of file and splits accordingly
        foreach (string row in input.Split('\n'))
        {
            j = 0;
            foreach (string col in row.Trim().Split(','))
            {
                inputLines[j, i] = col.Trim();
                j++;
                
            }

            i++;
        }
        //Debug.Log(j);
        listDepth.Add(j);
        return inputLines;
    }

    public static void ParseWholeFolder()
    {
        //Reads each file in the folder */Prefabs
        foreach (var file in Directory.GetFiles(textPath))
        {
            allTextPrefabs.Add(ParseTextFile(file));
        }
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
