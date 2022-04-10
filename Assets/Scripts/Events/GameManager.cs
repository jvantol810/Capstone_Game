using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }
    public void NewScene()
    {
        SceneManager.LoadScene(1);
    }
}
