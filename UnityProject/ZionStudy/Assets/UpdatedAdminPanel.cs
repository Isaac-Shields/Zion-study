using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UpdatedAdminPanel : MonoBehaviour
{
    public GameObject masterObj;
    private DatabaseHelper dbHelper;
    private MasterScript master;
    private ListView userLv;
    private ListView cardsetLv;
    private TextField searchBox;
    private Button searchBtn;
    private Button goBackBtn;
    public GameObject updateUserCanvas;
    private  List<userObject> users = new List<userObject>();
    private void Awake() 
    {
        dbHelper = masterObj.GetComponent<DatabaseHelper>();
        master = masterObj.GetComponent<MasterScript>();
        var root = GetComponent<UIDocument>().rootVisualElement;
        userLv = root.Q<ListView>("userLv");
        cardsetLv = root.Q<ListView>("cardsetLv");
        searchBox = root.Q<TextField>("inputBox");

        goBackBtn = root.Q<Button>("goBackBtn");
        goBackBtn.clicked += goBackLogic;

        searchBtn = root.Q<Button>("searchBtn");
        searchBtn.clicked += searchUsers;

        users = dbHelper.getAllUsers();
    }

    public void loadAllUsers()
    {
        userLv.makeItem = () => {return new Button();};
        userLv.bindItem = (element, index) => 
        {
            if(element is Button button) 
            {
                userObject curUser = (userObject)userLv.itemsSource[index];
                button.text = curUser.getUserName();
                button.clicked += () => updateUserCanvas.GetComponent<updateUser>().fillInfo(curUser);
            }
        };

        userLv.itemsSource = users;
        userLv.fixedItemHeight = 40;
    }

    private void searchUsers()
    {
        users = dbHelper.getAllUsers();
        List<userObject> searchedUsers = new List<userObject>();
        if(searchBox.value.Length == 0)
        {
            loadAllUsers();
        }
        else
        {
            users = dbHelper.getAllUsers();
            string searchTerm = searchBox.value;
            searchedUsers = users.FindAll(user => user.getUserName() == searchTerm);
            users = searchedUsers;
            loadAllUsers();
        }
    }



    public void fillPendingCardsets()
    {
        List<cardsetObj> pendingCardsets = dbHelper.getCardsetsForApproval();
        cardsetLv.makeItem = () => {return new Button();};
        cardsetLv.bindItem = (element, index) => 
        {
            if(element is Button button) 
            {
                cardsetObj curCard = (cardsetObj)cardsetLv.itemsSource[index];
                button.text = curCard.getCardsetTitle();
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
}
