using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    public InputField nameField;
    public InputField passwordField;
    public string username;
    public string password;
    public string message;
    public Text registerMenuText;
    public Button registerButton;

    public TitleScreenManager titleScreenManager;
    public GameObject SubmitScreen;

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        username = nameField.text;
        password = passwordField.text;
        //list<imultipartformsection> formdata = new list<imultipartformsection>();
        //formdata.add(new multipartformfilesection("username", username));
        //formdata.add(new multipartformfilesection("password", password));
        WWWForm formdata = new WWWForm();
        formdata.AddField("USERNAME", username);
        formdata.AddField("PASSWORD", password);

        using (UnityWebRequest registerPOST = UnityWebRequest.Post("http://localhost/capstone/register.php", formdata))
        {
            registerPOST.useHttpContinue = false;
            registerPOST.chunkedTransfer = false;////ADD THIS LINE
            yield return registerPOST.SendWebRequest();

            //if (registerPOST.result != UnityWebRequest.Result.Success)
            //{
            //    Debug.Log(registerPOST.error);
            //}
            //else
            //{
            //    //Handle Error
            //    Debug.Log("Form Upload Complete");
            //    Debug.Log(registerPOST.downloadHandler.text);
            //    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            //}

            if (registerPOST.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: Register Failed " + registerPOST.error);
            }
            else
            {
                message = registerPOST.downloadHandler.text;
                if (message.StartsWith("3")) //Username already exists
                {
                    Debug.Log(message);
                    registerMenuText.text = "Register an account" + "\n" + message.Substring(2);
                }
                else if (message.StartsWith("4"))
                {
                    Debug.Log(message);
                }
                else
                {
                    //DBManager.username = nameField.text;
                    registerMenuText.text = ("New Player Added: " + message);
                    //split message by tabs, and we get [0] = playerid, [1] = username, [2] = totalruns, [3] = currenttime, [4] = currency;
                    Debug.Log(message);
                    
                    string[] splitmessage = message.Split('\t');
                    DatabaseManager.userid = int.Parse(splitmessage[0]);
                    Debug.Log(DatabaseManager.userid);
                    DatabaseManager.username = splitmessage[1];
                    Debug.Log(DatabaseManager.username);
                    DatabaseManager.currency = int.Parse(splitmessage[4]);
                    Debug.Log(DatabaseManager.currency);

                    titleScreenManager.SignedIn();
                    if (titleScreenManager.FromSubmitScreen)
                    {
                        titleScreenManager.SwitchScreen(SubmitScreen);
                    }
                    else
                    {
                        titleScreenManager.SwitchScreen(titleScreenManager.gameObject);
                    }
                    //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                    //DBManager.time = int.Parse(loginPOST.downloadHandler.text.Split('\t')[0]);
                }
            }
        }

    }

    public void VerifyInputs()
    {
        registerButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8 && nameField.text.Length <= 20);
    }
}
