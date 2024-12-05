using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class updateUser : MonoBehaviour
{
    public GameObject masterObj;
    private DatabaseHelper dbHelper;
    private MasterScript master;
    private TextField unameBox;
    private TextField passwordBox;
    private Button updateBtn;
    private Button deleteBtn;
    private Button cancelBtn;
    private UIDocument uiDoc;
    private Toggle adminToggle;
    private userObject sentUser;
    public UpdatedAdminPanel uap;

    private void Awake() 
    {
        master = masterObj.GetComponent<MasterScript>();
        dbHelper = masterObj.GetComponent<DatabaseHelper>();
        uiDoc = GetComponent<UIDocument>();

        var root = GetComponent<UIDocument>().rootVisualElement;

        unameBox = root.Q<TextField>("uNameBox");
        passwordBox = root.Q<TextField>("pWordBox");

        updateBtn = root.Q<Button>("updateBtn");
        updateBtn.clicked += updateUserInfo;

        deleteBtn = root.Q<Button>("deleteBtn");
        deleteBtn.clicked += deleteUser;

        cancelBtn = root.Q<Button>("cancelBtn");
        cancelBtn.clicked += cancelOperation;

        adminToggle = root.Q<Toggle>("adminBox");
    }

    public void fillInfo()
    {
        userObject user = master.userToUpdate;
        showUI();
        unameBox.value = user.getUserName();
        passwordBox.value = user.getUserPassword();

        if(user.getUserLevel() == 0)
        {
            adminToggle.value = false;
        }
        else if(user.getUserLevel() == 1)
        {
            adminToggle.value = true;
        }

        Debug.Log("User ID: " + user.getUserLevel());

    }

    public void buttonClick(string uname)
    {
        Debug.Log("Button Clicked!" + " Username: " + uname);
    }



    private void updateUserInfo()
    {
        if(unameBox.value.Length != 0 && passwordBox.value.Length != 0 && sentUser.getUid() != -1)
        {
            userObject updatedInfo = new userObject();
            updatedInfo.setUserName(unameBox.value);
            updatedInfo.setPassword(passwordBox.value);
            updatedInfo.setUid(sentUser.getUid());
            if(adminToggle.value)
            {
                updatedInfo.setUserLevel(1);
            }
            else
            {
                updatedInfo.setUserLevel(0);
            }
            dbHelper.updateUser(updatedInfo);
        }
        else
        {
            Debug.Log("Fill out the boxes!");
        }
    }

    private void deleteUser()
    {
        dbHelper.deleteUser(sentUser.getUid());
    }

    private void cancelOperation()
    {
        master.startAdminSettings();
        hideUI();
    }

    public void showUI()
    {
        var root = uiDoc.rootVisualElement;
        root.style.display = DisplayStyle.Flex;
    }

    public void hideUI()
    {
        unameBox.value = "";
        passwordBox.value = "";
        adminToggle.value = false;
        sentUser = null;
        var root = uiDoc.rootVisualElement;
        root.style.display = DisplayStyle.None;
    }
}
