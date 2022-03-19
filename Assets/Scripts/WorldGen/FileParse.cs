using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public static class FileParse 
{
    private static String textPath = Application.persistentDataPath + "/Prefabs";

    private static String jsonString;
    
    
    public static List<String[,]> allTextPrefabs = new List<string[,]>();

    //Reads in text file splits into a String 2d array
    private static String[,] ParseTextFile(String file)
    {
        //IMPORTANT
        //The formatting of a room prefab has to be in a square shape see TestPrefab.txt for reference
        
        //Reads each prefab file
        String input = File.ReadAllText(file);
        Debug.Log(input);
        
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
        Debug.Log(inputLines[1,1]);
        
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
    

}
