using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginMasterScript : MonoBehaviour
{
    public TMP_InputField loginPassword;
    public TMP_InputField loginUsername;
    public TextMeshProUGUI errorText;
    public Button loginBtn;
    public Button signupBtn;
    public GameObject masterObj;
    private MasterScript master;


    private void Start() 
    {
        master = masterObj.GetComponent<MasterScript>();
        loginBtn.onClick.AddListener(login);
        signupBtn.onClick.AddListener(signup);
    }

    public void loginCanvasCleanup()
    {
        loginUsername.text = "";
        loginPassword.text = "";
        errorText.text = "";
    }

    private void login()
    {
        if(loginPassword.text.Length > 0 && loginPassword.text.Length > 0)
        {
            if(!masterObj.GetComponent<DatabaseHelper>().newUserName(loginUsername.text))
            {
                int uid = masterObj.GetComponent<DatabaseHelper>().getSessionData(loginUsername.text, loginPassword.text);
                if(uid != -1)
                {
                    master.curSessionData.setUsername(loginUsername.text);
                    master.curSessionData.setUserPassword(loginPassword.text);
                    master.curSessionData.setUserId(uid);
                    master.curSessionData.setAdminLevel(masterObj.GetComponent<DatabaseHelper>().getUserLevel(master.curSessionData.getUserId()));
                    master.cleanUpBeforeSwitch();
                    master.startCalculatorCanvas();
                    master.startNavBarCanvas();
                }
                else
                {
                    errorText.text = "Invalid Password.";
                }
            }
            else
            {
                errorText.text = "Invalid Username.";
            }
        }
        else
        {
            errorText.text = "Please enter a username and password";
        }
    }


    private void signup()
    {
        master.loginCanvas.SetActive(false);
        master.signupCanvas.SetActive(true);
    }
}
