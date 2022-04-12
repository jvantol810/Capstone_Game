using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SubmitScoreManager : MonoBehaviour
{
    public string score = "10";
    public Text ScoreText;
    public Text UsernameText;
    public string username;
    public string message;
    public Button YesButton;
    public Text submitMenuText;

    //public TitleScreenManager titleScreenManager;

    // Start is called before the first frame update
    void Start()
    {
        //DatabaseManager.username = "JohnGoodlaski";
    }

    // Update is called once per frame
    void Update()
    {
        UsernameText.text = DatabaseManager.username;
        ScoreText.text = score;

        if (DatabaseManager.username == null)
        {
            YesButton.interactable = false;
            submitMenuText.text = "User not signed in - Unable to submit score";
        }
        else
        {
            submitMenuText.text = "Would you like to submit your score to the leaderboards?";
            YesButton.interactable = true;
        }
    }

    public void CallYes()
    {
        StartCoroutine(SubmitClass());
    }

    IEnumerator SubmitClass()
    {
        username = DatabaseManager.username;
        WWWForm formdata = new WWWForm();
        formdata.AddField("USERNAME", username);
        formdata.AddField("SCORE", score);
        formdata.AddField("PLAYERID", DatabaseManager.userid);

        using (UnityWebRequest submitPOST = UnityWebRequest.Post("http://localhost/capstone/submit.php", formdata))
        {
            submitPOST.useHttpContinue = false;
            submitPOST.chunkedTransfer = false;////ADD THIS LINE
            yield return submitPOST.SendWebRequest();

            if (submitPOST.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: Login Failed " + submitPOST.error);
            }
            else
            {
                message = submitPOST.downloadHandler.text;
                Debug.Log(message);
            }
        }
    }
}
