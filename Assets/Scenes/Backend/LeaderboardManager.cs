using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject viewportContent;

    public PHPManager phpManager;

    [Header("Prefabs")]
    public GameObject scoreEntryPrefab;
    public GameObject lastEntryPrefab;
    public GameObject failedConnectionPrefab;
    public GameObject loadingEntryPrefab;
    [Header("Top Buttons")]
    public Button globalButton;
    public Button localButton;
    [Header("Bottom Buttons")]
    public Button dc10Button;
    public Button dc25Button;
    public Button dc50Button;
    public Button dc100Button;

    //Private Variables

    string currentDisplayType = "Global";
    int displayCount = 10;


    // Start is called before the first frame update
    void Start()
    {
        //globalButton.interactable = false;
        //globalButton.GetComponentInChildren<Text>().color = Color.white;
        SetLeaderboardType(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //SetLeaderboardType: Switches from Global to Local scores based on parameter
    //if displayType = 1, toggle global leaderboard, if displayType = 2, toggle local leaderboard
    public void SetLeaderboardType(int displayType)
    {
        StopAllCoroutines();
        if(displayType == 1)
        {
            currentDisplayType = "Global";
            WipeDataEntries();
            StartCoroutine(RefreshGlobalLeaderboard());

        }
        else if(displayType == 2)
        {
            currentDisplayType = "Local";
            WipeDataEntries();
            RefreshLocalLeaderboard();
        }
        else
        {
            //Invalid input
            Debug.Log("Invalid displayType entered.");
        }
        UpdateTopButtons();
    }

    private void UpdateTopButtons()
    {
        if(currentDisplayType == "Global")
        {
            globalButton.interactable = false;
            globalButton.GetComponentInChildren<Text>().color = Color.white;
            localButton.interactable = true;
            localButton.GetComponentInChildren<Text>().color = Color.black;
        }
        else if(currentDisplayType == "Local")
        {
            localButton.interactable = false;
            localButton.GetComponentInChildren<Text>().color = Color.white;
            globalButton.interactable = true;
            globalButton.GetComponentInChildren<Text>().color = Color.black;
        }
    }

    private void WipeDataEntries()
    {
        Transform[] currentEntries;
        currentEntries = viewportContent.GetComponentsInChildren<Transform>();
        Debug.Log("Currently have: " + currentEntries.Length + " entries.");
        for(int i = 1; i < currentEntries.Length; i++)
        {
            Destroy(currentEntries[i].gameObject);
        }
    }

    public void CreateDataEntries(string scores)
    {

    }

    private IEnumerator RefreshGlobalLeaderboard()
    {
        GameObject LoadingEntry = Instantiate(loadingEntryPrefab, viewportContent.transform);
        CoroutineWithData cwd = new CoroutineWithData(this, phpManager.GetGlobalLeaderboard());
        yield return cwd.coroutine;
        Destroy(LoadingEntry);
        string leaderboardscores = cwd.result.ToString();
        //Debug.Log(leaderboardscores);
        string[] sortedEntries = leaderboardscores.Split('\t');
        if(sortedEntries[0] == "False")
        {
            Instantiate(failedConnectionPrefab, viewportContent.transform);
        }
    }

    //CURRENTLY A TEST!
    private void RefreshLocalLeaderboard()
    {
        for(int i = 1; i < 10; i++)
        {
            Instantiate(scoreEntryPrefab, viewportContent.transform);
        }
    }

}
