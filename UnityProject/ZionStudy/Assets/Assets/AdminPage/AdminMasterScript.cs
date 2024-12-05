using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminMasterScript : MonoBehaviour
{
    public Button goBackBtn;
    public GameObject MasterObj;
    private MasterScript master;
    private DatabaseHelper dbHelper;
    private int taskNumber;
    public GameObject confirmCanvas;
    public Transform listOfPending;
    public GameObject cardsetTemplate;
    private int cardsetNumber;

    void Start()
    {
        taskNumber = -1;
        cardsetNumber = -1;
        master = MasterObj.GetComponent<MasterScript>();
        goBackBtn.onClick.AddListener(goBack);
    }

    private void goBack()
    {
        master.closeAdminCanvas();
        master.startSettingsCanvas();
    }

    public void loadPendingCardsets()
    {
        List<cardsetObj> objects = new List<cardsetObj>();
        dbHelper = MasterObj.GetComponent<DatabaseHelper>();
        objects = dbHelper.getCardsetsForApproval();
        foreach(Transform child in listOfPending)
        {
            Destroy(child.gameObject);
        }

        foreach(cardsetObj cObj in objects)
        {
            GameObject listItem = Instantiate(cardsetTemplate, listOfPending);
            Button link = listItem.GetComponent<Button>();
            link.onClick.AddListener(() => setCardsetId(cObj.getSetId()) );
            link.GetComponentInChildren<TextMeshProUGUI>().text = cObj.getCardsetTitle();
        }
    }

    public void waitForConfirmation()
    {
        if(taskNumber == 1)
        {
            //open confirm canvas to allow or deny cardsets to made public
            if(cardsetNumber != -1)
            {
                dbHelper.changeCardsetToPublic(cardsetNumber);
                loadPendingCardsets();
            }
        }
        else if(taskNumber == 2)
        {

        }
    }

    private void setCardsetId(int cid)
    {
        taskNumber = 1;
        cardsetNumber = cid;
        confirmCanvas.GetComponent<confirmDeletion>().senderScript = gameObject.GetComponent<AdminMasterScript>();
        confirmCanvas.GetComponent<confirmDeletion>().updateMessage("Do you want make this public?");
        confirmCanvas.SetActive(true);
    }
}
