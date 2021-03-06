using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;
    public string username;
    public string password;
    public string message;
    public Text loginMenuText;
    public Button submitButton;

    public TitleScreenManager titleScreenManager;
    public GameObject SubmitScreen;

    public void CallLogin()
    {
        StartCoroutine(LoginClass());
    }

    IEnumerator LoginClass()
    {
        username = nameField.text;
        password = passwordField.text;
        WWWForm formdata = new WWWForm();
        formdata.AddField("USERNAME", username);
        formdata.AddField("PASSWORD", password);

        using (UnityWebRequest loginPOST = UnityWebRequest.Post("http://localhost/capstone/login.php", formdata))
        {
            loginPOST.useHttpContinue = false;
            loginPOST.chunkedTransfer = false;////ADD THIS LINE
            yield return loginPOST.SendWebRequest();

            if (loginPOST.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: Login Failed " + loginPOST.error);
            }
            else
            {
                message = loginPOST.downloadHandler.text;

                if (message.StartsWith("3:")) //Username already exists
                {
                    Debug.Log(message);
                    loginMenuText.text = "Log into your account" + "\n" + message.Substring(2);
                }
                else if (message.StartsWith("4:"))
                {
                    Debug.Log(message);
                    loginMenuText.text = "Log into your account" + "\n" + message.Substring(2);
                }
                else if (message.StartsWith("2:"))
                {
                    Debug.Log(message);
                    //change text to incorrect password
                }
                else
                {
                    //Debug.Log(message);
                    //split message by tabs, and we get [0] = playerid, [1] = username, [2] = totalruns, [3] = currenttime, [4] = currency;
                    string[] splitmessage = message.Split('\t');
                    DatabaseManager.userid = int.Parse(splitmessage[0]);
                    Debug.Log(DatabaseManager.userid);
                    DatabaseManager.username = splitmessage[1];
                    Debug.Log(DatabaseManager.username);
                    DatabaseManager.currency = int.Parse(splitmessage[4]);
                    Debug.Log(DatabaseManager.currency);

                    titleScreenManager.SignedIn();
                    if(titleScreenManager.FromSubmitScreen)
                    {
                        titleScreenManager.SwitchScreen(SubmitScreen);
                    }
                    else
                    {
                        titleScreenManager.SwitchScreen(titleScreenManager.gameObject);
                    }
                    
                    //loginMenuText.text = ("Previous High Score: " + message);
                    //TitleScreenManager.SetUserInfo(nameField.text);
                    //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
            }
        }

    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8 && nameField.text.Length <= 20);
    }


}
