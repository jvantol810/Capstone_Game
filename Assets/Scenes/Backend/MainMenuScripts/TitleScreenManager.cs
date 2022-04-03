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
    public GameObject GBPurchaseScreen;
    public Text GBDisplayText;
    public Button GBPurchaseButton;

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

    public void SetGBPurchaseScreenEnabled(bool enabledval)
    {
        GBPurchaseScreen.SetActive(enabledval && DatabaseManager.username != null);
    }

    public void UpdateGhostBucksDisplay(int amt)
    {
        GBDisplayText.text = amt.ToString("n0") + " GB";
    }
}
