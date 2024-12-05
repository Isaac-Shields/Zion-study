using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UpdatedAdminPanel : MonoBehaviour
{
    public GameObject masterObj;
    private DatabaseHelper dbHelper;
    private MasterScript master;
    private ListView cardsetLv;
    private Button goBackBtn;
    public GameObject updateUserCanvas;
    //private  List<userObject> users = new List<userObject>();
    private UIDocument uiDoc;
    public GameObject confirmCanvas;
    private int taskNumber = -1;
    private cardsetObj selectedCardset;
    private void Awake() 
    {
        uiDoc = GetComponent<UIDocument>();
        dbHelper = masterObj.GetComponent<DatabaseHelper>();
        master = masterObj.GetComponent<MasterScript>();
        var root = GetComponent<UIDocument>().rootVisualElement;
        cardsetLv = root.Q<ListView>("cardsetLv");

        goBackBtn = root.Q<Button>("goBackBtn");
        goBackBtn.clicked -= goBackLogic;
        goBackBtn.clicked += goBackLogic;
        //users = dbHelper.getAllUsers();
    }

    private void clickLogic(userObject curUser)
    {
        master.userToUpdate = curUser;
        master.closeAdminCanvas();
        master.startUpdateCanvas();
    }

    private void openUpdateWindow(userObject curUser)
    {
        updateUserCanvas.GetComponent<updateUser>().fillInfo();
        hideUI();
    }

    public void fillPendingCardsets()
    {
        List<cardsetObj> pendingCardsets = dbHelper.getCardsetsForApproval();
        cardsetLv.Clear();
        cardsetLv.makeItem = () => {return new Button();};
        cardsetLv.bindItem = (element, index) => 
        {
            if(element is Button button) 
            {
                cardsetObj curCard = (cardsetObj)cardsetLv.itemsSource[index];
                button.text = curCard.getCardsetTitle();
                button.clicked -= ( () => startConfirmCanvas(curCard));
                button.clicked += ( () => startConfirmCanvas(curCard));
            }
        };

        cardsetLv.itemsSource = pendingCardsets;
        cardsetLv.fixedItemHeight = 40;

    }

    private void goBackLogic()
    {
        master.closeAdminCanvas();
        master.startSettingsCanvas();
    }

    public void showUI()
    {
        var root = uiDoc.rootVisualElement;
        root.style.display = DisplayStyle.Flex;
    }

    public void hideUI()
    {
        var root = uiDoc.rootVisualElement;
        root.style.display = DisplayStyle.None;
    }

    private void startConfirmCanvas(cardsetObj curCard)
    {
        selectedCardset = curCard;
        taskNumber = 1;
        confirmCanvas.SetActive(true);
        confirmCanvas.GetComponent<confirmDeletion>().senderScript = gameObject.GetComponent<UpdatedAdminPanel>();
        confirmCanvas.GetComponent<confirmDeletion>().updateMessage("Do you want to make this public?");
    }

    public void waitForConfirmation()
    {
        if(taskNumber == 1)
        {
            taskNumber = -1;
            Debug.Log("Canceling operation on cardset: " + selectedCardset.getCardsetTitle());
            dbHelper.changeCardsetToPublic(selectedCardset.getSetId());
            selectedCardset = null;
            fillPendingCardsets();

        }
    }

    public void cancelOperationOnWait()
    {
        if(taskNumber == 1)
        {
            Debug.Log("Canceling operation on cardset: " + selectedCardset.getCardsetTitle());
            taskNumber = -1;
            dbHelper.revertCardset(selectedCardset.getSetId());
            selectedCardset = null;
            fillPendingCardsets();
        }
    }
}
