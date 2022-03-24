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
                Debug.Log("Error: Login Failed " + registerPOST.error);
            }
            else
            {
                message = registerPOST.downloadHandler.text;
                if (message.StartsWith("3")) //Username already exists
                {
                    Debug.Log(message);
                    registerMenuText.text = "Register an account" + "\n" + message.Substring(2);
                }
                else
                {
                    //DBManager.username = nameField.text;
                    registerMenuText.text = ("New Player Added: " + message);
                    DatabaseManager.username = nameField.text;
                    //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                    //DBManager.time = int.Parse(loginPOST.downloadHandler.text.Split('\t')[0]);
                }
            }
        }

    }

    public void VerifyInputs()
    {
        registerButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
