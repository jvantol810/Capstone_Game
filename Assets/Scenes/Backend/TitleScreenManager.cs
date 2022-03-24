using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject Leaderboard;
    public GameObject currentScreen;
    public Button signinButton;
    public Text userInfo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToLeaderboard()
    {
        Leaderboard.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SwitchToTitle()
    {
        Leaderboard.SetActive(false);
        gameObject.SetActive(true);
        //SetUserInfo();
    }

    public void SwitchScreen(GameObject newScreen)
    {
        SetUserInfo();
        currentScreen.SetActive(false);
        newScreen.SetActive(true);
        currentScreen = newScreen;
        
    }

    public void SignedIn()
    {
        signinButton.interactable = (DatabaseManager.username == null);
    }

    public void SetUserInfo()
    {
        if(DatabaseManager.username != null)
        {
            userInfo.text = DatabaseManager.username;
        }
    }
}
