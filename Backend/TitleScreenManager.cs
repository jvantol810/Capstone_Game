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
    public Text signinText;
    public Text userInfo;
    public GameObject signedInScreen;
    public GameObject signedOutScreen;


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
        if (DatabaseManager.username != null)
        {
            //signinButton.interactable = false;
            signinText.text = "SIGN OUT";
        }
        else
        {
            signinText.text = "SIGN IN";
        }
        
    }

    //public void SignedIn()
    //{
    //    signinButton.interactable = (DatabaseManager.username == null);
    //}

    public void SetUserInfo()
    {
        if(DatabaseManager.username != null)
        {
            userInfo.text = DatabaseManager.username;
        }
        else
        {
            userInfo.text = "No User Signed In";
        }
    }

    public void SwitchToSignInScreen()
    {
        
        if (DatabaseManager.username != null)
        {
            //currentScreen.setactive(false);
            //signedOutScreen.setactive(true);
            //currentScreen = signedOutScreen;
            DatabaseManager.username = null;
            SetUserInfo();
            signinText.text = "SIGN IN";
        }
        else
        {
            //SetUserInfo();
            currentScreen.SetActive(false);
            signedInScreen.SetActive(true);
            currentScreen = signedInScreen;
        }

    }
}
