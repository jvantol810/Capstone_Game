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
            {
                Debug.LogError("Game Manager is NULL");
                _instance = GameObject.FindObjectOfType<GameManager>();
            }
            return _instance;
        }

    }
    private void Awake()
    {
        //if (_instance != null && _instance != this)
          //  Destroy(this);
        //_instance = this;
        DontDestroyOnLoad(gameObject);
        GameEvents.OnEnterTeleporter.AddListener(ChangeLevel);
        GameEvents.OnPlayerDie.AddListener(ChangetoSubmit);
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void ChangeLevel() { 
        SceneManager.LoadScene(1);
    }

    public  void ChangetoSubmit()
    {
        SceneManager.LoadScene(0);
        StartCoroutine(StopYouveViolatedTheLaw());
    }
    private IEnumerator StopYouveViolatedTheLaw()
    {
        while (SceneManager.GetActiveScene().buildIndex != 0)
        {
            yield return null;
        }

        TitleScreenManager title = FindObjectOfType<TitleScreenManager>();

        Debug.Log(title);
        title.SwitchToSubmit();
    }
}
