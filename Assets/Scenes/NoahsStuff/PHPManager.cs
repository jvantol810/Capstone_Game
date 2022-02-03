using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PHPManager : MonoBehaviour
{
    private void Awake()
    {
        CallGetGlobalLeaderboard();
    }

    public void CallGetGlobalLeaderboard()
    {
        StartCoroutine(GetGlobalLeaderboard());
    }

    IEnumerator GetGlobalLeaderboard()
    {
        UnityWebRequest leaderboardGET = UnityWebRequest.Get("http://localhost/capstone/globalleaderboard.php");
        yield return leaderboardGET.SendWebRequest();
        if (leaderboardGET.result != UnityWebRequest.Result.Success)
        {
            //Handle Error
            Debug.Log(leaderboardGET.result);
        }
        else
        {
            Debug.Log("Got it!");
            Debug.Log(leaderboardGET.downloadHandler.text);
        }
    }
}
