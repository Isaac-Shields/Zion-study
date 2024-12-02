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

    private void Awake() 
    {
        master = masterObj.GetComponent<MasterScript>();
        dbHelper = masterObj.GetComponent<DatabaseHelper>();
    }

    public void fillInfo(userObject curUser)
    {
        gameObject.SetActive(true);
        master.closeAdminCanvas();
        var root = GetComponent<UIDocument>().rootVisualElement;
        unameBox = root.Q<TextField>("uNameBox");
        passwordBox = root.Q<TextField>("pWordBox");

        updateBtn = root.Q<Button>("updateBtn");
        updateBtn.clicked += () => updateUserInfo(curUser);

        deleteBtn = root.Q<Button>("deleteBtn");
        deleteBtn.clicked += () => deleteUser(curUser);

        cancelBtn = root.Q<Button>("cancelBtn");
        cancelBtn.clicked += cancelOperation;

        unameBox.value = curUser.getUserName();
        passwordBox.value = curUser.getUserPassword();

    }

    private void updateUserInfo(userObject user)
    {
        if(unameBox.value.Length != 0 && passwordBox.value.Length != 0 && user.getUid() != 0)
        {
            userObject updatedInfo = new userObject();
            updatedInfo.setUserName(unameBox.value);
            updatedInfo.setPassword(passwordBox.value);
            updatedInfo.setUid(user.getUid());
            dbHelper.updateUser(updatedInfo);
        }
        else
        {
            Debug.Log("Fill out the boxes!");
        }
    }

    private void deleteUser(userObject user)
    {
        dbHelper.deleteUser(user.getUid());
    }

    private void cancelOperation()
    {
        gameObject.SetActive(false);
        master.startAdminSettings();
    }
}
