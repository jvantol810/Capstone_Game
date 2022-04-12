using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public static class GameManagerStatic
{
    static GameManagerStatic()
    {
        GameEvents.OnPlayerDie.AddListener(ChangetoSubmit);
    }
    public static void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public static void ChangeLevel() { 
        SceneManager.LoadScene(1);
    }

    public static void ChangetoSubmit()
    {
        SceneManager.LoadScene(0);
        //StartCoroutine(StopYouveViolatedTheLaw());
    }
    //private IEnumerator StopYouveViolatedTheLaw()
    //{
    //    while (SceneManager.GetActiveScene().buildIndex != 0)
    //    {
    //        yield return null;
    //    }

    //    TitleScreenManager title = FindObjectOfType<TitleScreenManager>();

    //    Debug.Log(title);
    //    title.SwitchToSubmit();
    //}
}
