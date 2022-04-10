using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PHPManager : MonoBehaviour
{
    [SerializeField]
    private string lastRetrievedLeaderboardData;
    [SerializeField]
    private bool connectionSuccess;
    [SerializeField]
    private string lastRetrievedOwnedHats;

    private void Awake()
    {
        //CallGetGlobalLeaderboard();
        //DatabaseManager.userid = 12345;
        //CallGetPlayerReceipts();
        //CallPostPlayerReceipt("H333", 11);
    }

    public string CallGetGlobalLeaderboard()
    {
        StartCoroutine(GetGlobalLeaderboard());
        return connectionSuccess.ToString() + "\t" + lastRetrievedLeaderboardData;
        //CoroutineWithData cwd = new CoroutineWithData(this, LoadGlobal());
        //yield return cwd.coroutine;
    }

    public string CallGetPlayerReceipts()
    {
        StartCoroutine(GetPlayerReceipts());
        Debug.Log(lastRetrievedOwnedHats);
        return lastRetrievedOwnedHats;
    }

    public void CallUpdatePlayerCurrency()
    {
        StartCoroutine(UpdatePlayerCurrency());
    }

    public string CallPostPlayerReceipt(string itemID, int itemPrice)
    {
        return StartCoroutine(PostPlayerReceipt(itemID, itemPrice)).ToString();
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

    public IEnumerator GetPlayerReceipts()
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("PLAYERID", DatabaseManager.userid);
        using (UnityWebRequest playerReceiptsGET = UnityWebRequest.Post("http://localhost/capstone/getcashshopreceipts.php", formdata))
        {
            playerReceiptsGET.useHttpContinue = false;
            playerReceiptsGET.chunkedTransfer = false;////ADD THIS LINE
            yield return playerReceiptsGET.SendWebRequest();
            if (playerReceiptsGET.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(playerReceiptsGET.result);
                Debug.Log(playerReceiptsGET.downloadHandler.text);
                yield return "False";
            }
            else
            {
                //Debug.Log(playerReceiptsGET.downloadHandler.text);
                //lastRetrievedOwnedHats = playerReceiptsGET.downloadHandler.text;
                FindObjectOfType<CashEquipsData>().InitializeOwnedHats(playerReceiptsGET.downloadHandler.text);
                yield return "True\t" + playerReceiptsGET.downloadHandler.text;
            }
        }   
    }

    public IEnumerator PostPlayerReceipt(string id, int price)
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("PLAYERID", DatabaseManager.userid);
        formdata.AddField("ITEMID", id);
        formdata.AddField("PRICE", price);
        //formdata.AddField("DATE", System.DateTime.Now.ToString("MM/dd/yyyy"));
        using (UnityWebRequest playerReceiptsPOST = UnityWebRequest.Post("http://localhost/capstone/postcashshopreceipt.php", formdata))
        {
            playerReceiptsPOST.useHttpContinue = false;
            playerReceiptsPOST.chunkedTransfer = false;////ADD THIS LINE
            yield return playerReceiptsPOST.SendWebRequest();
            if (playerReceiptsPOST.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(playerReceiptsPOST.result);
                Debug.Log(playerReceiptsPOST.downloadHandler.text);
                yield return "False";
            }
            else
            {
                Debug.Log(playerReceiptsPOST.downloadHandler.text);
                yield return "True\t" + playerReceiptsPOST.downloadHandler.text;
            }
        }
    }

    public IEnumerator UpdatePlayerCurrency()
    {
        WWWForm formdata = new WWWForm();
        formdata.AddField("PLAYERID", DatabaseManager.userid);
        formdata.AddField("CURRENCY", DatabaseManager.currency);
        using (UnityWebRequest playerCurrencyPOST = UnityWebRequest.Post("http://localhost/capstone/updatecurrency.php", formdata))
        {
            playerCurrencyPOST.useHttpContinue = false;
            playerCurrencyPOST.chunkedTransfer = false;////ADD THIS LINE
            yield return playerCurrencyPOST.SendWebRequest();
            Debug.Log(playerCurrencyPOST.downloadHandler.text);
            if (playerCurrencyPOST.result != UnityWebRequest.Result.Success)
            {
                yield return "Failed to update currency.";
            }
            else
            {
                
                yield return "Successfully updated currency.";
            }
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


