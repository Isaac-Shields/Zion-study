using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsPageMaster : MonoBehaviour
{
    public Button logoutBtn;
    public GameObject Master;

    private void Start() 
    {
        logoutBtn.onClick.AddListener(LogoutUser);
    }

    public void settingsCanvasCleanup()
    {

    }

    public void LogoutUser()
    {
        Master.GetComponent<MasterScript>().closeSettingsCanvas();
        Master.GetComponent<MasterScript>().startLoginCanvas();
    }
}
