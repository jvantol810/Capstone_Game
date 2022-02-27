using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileParse : MonoBehaviour
{
    private String path;
    private String jsonString;
    
    //This is a bigger area than rooms should need
    private string[,] lines = new String[20, 20];
    
    // Start is called before the first frame update
    void Start()
    {
       ParseTextFile();
    }

    //Reads in text file splits into a String 2d array
    private String[,] ParseTextFile()
    {
        path = Application.persistentDataPath + "/TextPrefab.txt";
        String input = File.ReadAllText(path);

        
        int i = 0, j = 0;
        
        foreach (var row in input.Split('\n'))
        {
            foreach (var col in row.Trim().Split(','))
            {
                lines[j, i] = col.Trim();
                j++;
            }

            i++;
        }

        return lines;
    }
    
    //Reads in from JSON -- Doesn't Work -.-
    private void ParseJSON()
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
