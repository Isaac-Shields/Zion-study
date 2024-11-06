using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignupScriptMaster : MonoBehaviour
{
    public GameObject MasterObj;
    public Button signUpBtn;
    public Button goBackBtn;
    public TMP_InputField username;
    public TMP_InputField password;
    public TextMeshProUGUI errorMsg;

    private void Start() 
    {
        signUpBtn.onClick.AddListener(signupAttempt);
        goBackBtn.onClick.AddListener(backToMainMenu);
    }

    public void signupCanvasCleanup()
    {
        username.text = "";
        password.text = "";
        errorMsg.text = "";
    }

    private void signupAttempt ()
    {
        if(username.text.Length > 0 && password.text.Length > 0)
        {
            if(MasterObj.GetComponent<DatabaseHelper>().newUserName(username.text))
            {
                if(MasterObj.GetComponent <DatabaseHelper>().addNewUser(username.text, password.text))
                {
                    errorMsg.text = "User successfully added.";
                    errorMsg.color = Color.green;
                    username.text = "";
                    password.text = "";
                }
                else
                {
                    errorMsg.text = "Error adding user.";
                    errorMsg.color = Color.red;
                }
            }
            else
            {
                errorMsg.text = "Username already exists.";
                errorMsg.color = Color.red;
            }
        }
        else
        {
            errorMsg.text = "Please make sure all boxes are filled.";
            errorMsg.color = Color.red;
        }
    }

    private void backToMainMenu()
    {
        MasterObj.GetComponent<MasterScript>().closeSignupCanvas();
        MasterObj.GetComponent<MasterScript>().startLoginCanvas();
    }

}
