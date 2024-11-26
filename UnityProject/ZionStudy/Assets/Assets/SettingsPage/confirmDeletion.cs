using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class confirmDeletion : MonoBehaviour
{
    public Button confirmBtn;
    public Button denyBtn;
    public MonoBehaviour senderScript;
    public TextMeshProUGUI message;
    private string curMessage;
    private void Start() 
    {
        confirmBtn.onClick.AddListener(confirmed);
        denyBtn.onClick.AddListener(cancelDeletion);
        curMessage = message.text;
    }

    public void updateMessage(string m)
    {
        message.text = m;
    }

    private void confirmed()
    {
        var method = senderScript.GetType().GetMethod("waitForConfirmation");
        if(method != null)
        {
            method.Invoke(senderScript, null);
            gameObject.SetActive(false);
            message.text = curMessage;
        }
        else
        {
            Debug.Log("Error" + method.Name);
        }
    }

    private void cancelDeletion()
    {
        gameObject.SetActive(false);
        message.text = curMessage;
    }


}
