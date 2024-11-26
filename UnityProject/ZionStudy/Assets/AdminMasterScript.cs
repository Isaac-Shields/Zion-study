using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminMasterScript : MonoBehaviour
{
    public Button goBackBtn;
    public GameObject MasterObj;
    private MasterScript master;
    private DatabaseHelper dbHelper;

    void Start()
    {
        master = MasterObj.GetComponent<MasterScript>();
        dbHelper = MasterObj.GetComponent<DatabaseHelper>();
        goBackBtn.onClick.AddListener(goBack);
    }

    private void goBack()
    {
        master.closeAdminCanvas();
        master.startSettingsCanvas();
    }
}
