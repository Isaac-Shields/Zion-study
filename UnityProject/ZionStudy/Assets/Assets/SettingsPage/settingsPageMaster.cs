using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class settingsPageMaster : MonoBehaviour
{
    public Button logoutBtn;
    public GameObject MasterObj;
    public Button changePasswordBtn;
    public TMP_InputField adminCode;
    public Button codeSubmit;
    public Button openAdminSettingsBtn;
    public Button deleteAccount;
    public GameObject confirmCanvas;
    private int taskNumber = -1;
    private DatabaseHelper dbHelper;
    private MasterScript master;

    private void Start() 
    {
        dbHelper = MasterObj.GetComponent<DatabaseHelper>();
        master = MasterObj.GetComponent<MasterScript>();
        logoutBtn.onClick.AddListener(LogoutUser);
        changePasswordBtn.onClick.AddListener(switchCanvasPW);
        openAdminSettingsBtn.onClick.AddListener(switchToAdminPanel);
        codeSubmit.onClick.AddListener(setAdmin);
        deleteAccount.onClick.AddListener(startDeletingUser);
    }

    public void settingsCanvasCleanup()
    {

    }

    private void startDeletingUser()
    {
        confirmCanvas.GetComponent<confirmDeletion>().senderScript = gameObject.GetComponent<settingsPageMaster>();
        confirmCanvas.GetComponent<confirmDeletion>().updateMessage("Warning! This will delete your account. If successful, you will be logged out.");
        confirmCanvas.SetActive(true);
        taskNumber = 1;
    }

    public void showAdminSettingsBtn(int level)
    {
        if(level == 1)
        {
            openAdminSettingsBtn.gameObject.SetActive(true);

        }
        else
        {
            openAdminSettingsBtn.gameObject.SetActive(false);
        }
    }

    private void switchToAdminPanel()
    {
        master.closeSettingsCanvas();
       master.startAdminSettings();
    }

    public void LogoutUser()
    {
        master.closeSettingsCanvas();
        master.startLoginCanvas();
    }

    private void switchCanvasPW()
    {
        master.closeSettingsCanvas();
        master.startChangePwCanvas();
    }

    private void setAdmin()
    {
        if(adminCode.text ==master.curSessionData.getUserPassword())
        {
            dbHelper.updateAdminLevel(master.curSessionData.getUserId(), 1);
        }
    }

    public void waitForConfirmation()
    {
        Debug.Log(taskNumber);
        if(taskNumber == 1)
        {
            if(dbHelper.deleteUser(master.curSessionData.getUserId()))
            {
                master.startLoginCanvas();
            }
        }
    }

}
