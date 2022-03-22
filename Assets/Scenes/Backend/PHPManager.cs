using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PHPManager : MonoBehaviour
{
    private string lastRetrievedLeaderboardData;
    private bool connectionSuccess;

    private void Awake()
    {
        //CallGetGlobalLeaderboard();
    }

    public string CallGetGlobalLeaderboard()
    {
        StartCoroutine(GetGlobalLeaderboard());
        return connectionSuccess.ToString() + "\t" + lastRetrievedLeaderboardData;
        //CoroutineWithData cwd = new CoroutineWithData(this, LoadGlobal());
        //yield return cwd.coroutine;
    }
    
    public IEnumerator GetGlobalLeaderboard()
    {
        UnityWebRequest leaderboardGET = UnityWebRequest.Get("http://localhost/capstone/globalleaderboard.php");
        yield return leaderboardGET.SendWebRequest();
        if (leaderboardGET.result != UnityWebRequest.Result.Success)
        {
            //Handle Error
            Debug.Log(leaderboardGET.result);
            //connectionSuccess = false;
            yield return "False";
        }
        else
        {
            Debug.Log("Got it!");
            //connectionSuccess = true;
            //lastRetrievedLeaderboardData = leaderboardGET.downloadHandler.text;
            //Debug.Log(lastRetrievedLeaderboardData);
            yield return "True\t" + leaderboardGET.downloadHandler.text;
        }
    }

    public IEnumerator GetShopItems(string itemType)
    {
        string preparedURL = "";
        if (itemType == "Hat" || itemType == "Bandana")
        {
            preparedURL = "http://localhost/capstone/shopItems/" + itemType + ".php";
        }
        else
        {
            yield return "Invalid ItemType";
        }
        UnityWebRequest shopGET = UnityWebRequest.Get(preparedURL);
        yield return shopGET.SendWebRequest();

        if(shopGET.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Shop Request Error: " + shopGET.result);
            yield return "False";
        }
        else
        {
            Debug.Log("Fetched Shop Items");
            yield return "True\t" + shopGET.downloadHandler.text;
        }

    }
    
    public IEnumerator LoadGlobal()
    {
        CoroutineWithData cwd = new CoroutineWithData(this, LoadGlobal());
        yield return cwd.coroutine;
    }
}

//Thanks Ted-Bigham!
//https://answers.unity.com/questions/24640/how-do-i-return-a-value-from-a-coroutine.html
public class CoroutineWithData
{
    public Coroutine coroutine { get; private set; }
    public object result;
    private IEnumerator target;
    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }
}
