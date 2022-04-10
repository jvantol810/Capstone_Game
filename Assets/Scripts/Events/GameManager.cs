using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogError("Game Manager is NULL");
            return _instance;
        }

    }
    private void Awake()
    {
        _instance = this;
        GameEvents.OnEnterTeleporter.AddListener(ChangeLevel);
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
       //DontDestroyOnLoad();
    }
    
    public void ChangeLevel() { 
        SceneManager.LoadScene(1);
    }
}
