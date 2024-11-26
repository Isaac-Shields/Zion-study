using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class changePassword : MonoBehaviour
{
    public Button saveBtn;
    public Button cancelBtn;
    public TextMeshProUGUI messageBox;
    public TMP_InputField oldPw;
    public TMP_InputField newPw;
    public TMP_InputField confirmPw;
    public DatabaseHelper dbHelper;
    public MasterScript master;

    void Start()
    {
        saveBtn.onClick.AddListener(updatePw);
        cancelBtn.onClick.AddListener(cancelOperation);
    }

    private void updatePw()
    {
        if(oldPw.text == master.curSessionData.getUserPassword())
        {
            if(newPw.text != master.curSessionData.getUserPassword())
            {
                if(newPw.text == confirmPw.text)
                {
                    if(dbHelper.updatePassword(master.curSessionData.getUserId(), newPw.text))
                    {
                        messageBox.text = "Password changed";
                        master.curSessionData.setUserPassword(newPw.text);
                        master.startSettingsCanvas();
                        master.closeChangePwCanvas();
                    }
                    else
                    {
                        messageBox.text = "Error changing password";
                    }
                }
                else
                {
                    messageBox.text = "Passwords do not match";
                }
            }
            else
            {
                messageBox.text = "Passwords are the same";
            }
        }
        else
        {
            messageBox.text = "Incorrect password";
        }
    }

    private void cancelOperation()
    {
        master.startSettingsCanvas();
        master.closeChangePwCanvas();
    }

    public void clearCanvas()
    {
        oldPw.text = "";
        newPw.text = "";
        confirmPw.text = "";
        messageBox.text = "";
    }
}
