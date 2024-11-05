using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginScriptMaster : MonoBehaviour
{
    public TMP_InputField loginPassword;
    public TMP_InputField loginUsername;
    public TextMeshProUGUI errorText;
    public Button loginBtn;
    public Button signupBtn;

    public GameObject databaseHelperOBJ;


    private void Start() 
    {
        loginBtn.onClick.AddListener(login);
        signupBtn.onClick.AddListener(signup);
    }


    private void login()
    {
        if(loginPassword.text.Length > 0 && loginPassword.text.Length > 0)
        {
            Debug.Log("Username: " + loginUsername.text);
            Debug.Log("User password: " + loginPassword.text);
            if(databaseHelperOBJ.GetComponent<DatabaseHelper>().checkUsername(loginUsername.text))
            {
                Debug.Log("Valid username!");
            }
            else
            {
                Debug.Log("Invalid");
            }
            loginUsername.text = null;
            loginPassword.text = null;

        }
        else
        {
            errorText.text = "Please enter a username and password";
        }
    }

    private void signup()
    {
        Debug.Log("Time to sign up");
    }
}
