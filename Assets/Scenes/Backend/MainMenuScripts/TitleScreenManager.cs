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
    public Button CashShopButton;
    public GameObject submitScoresScreen;

    //public CashEquipsData CEData;

    // Start is called before the first frame update
    void Start()
    {
        if(GameplaySession.isPlaying == true)
        {
            SwitchScreen(submitScoresScreen);
        }
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
    public void SwitchToSubmit()
    {
        currentScreen.SetActive(false);
        submitScoresScreen.SetActive(true);
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
        CashShopButton.interactable = (DatabaseManager.userid > 0);
        UpdateGhostBucksDisplay(DatabaseManager.currency);
        FindObjectOfType<PHPManager>().CallGetPlayerReceipts();
        //CEData.InitializeOwnedHats();
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
