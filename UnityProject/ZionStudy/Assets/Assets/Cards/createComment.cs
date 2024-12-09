using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class createComment : MonoBehaviour
{
    public TMP_InputField title;
    public TMP_InputField body;
    public Button addBtn;
    public Button cancelBtn;
    private DatabaseHelper dbHelper;
    private MasterScript master;
    public GameObject masterObj;
    public int setId;
    public TextMeshProUGUI messageBox;
    public GameObject publicUICanvas;
    public string sentTitle;

    void Start()
    {
        master = masterObj.GetComponent<MasterScript>();
        dbHelper = masterObj.GetComponent<DatabaseHelper>();
        addBtn.onClick.AddListener(onAddCommentBtnPress);
        cancelBtn.onClick.AddListener(onCancelBtnPress);
    }

    private void onAddCommentBtnPress()
    {
        if(title.text.Length > 0 && body.text.Length > 0)
        {
            if(dbHelper.addComment(title.text, body.text, setId))
            {
                gameObject.SetActive(false);
                publicUICanvas.GetComponent<fillNewListview>().showData(setId, sentTitle);
                //publicUICanvas.GetComponent<fillNewListview>().showUI();
            }
            else
            {
                messageBox.text = "Error adding comment to database.";
            }
        }
        else
        {
            messageBox.text = "Please enter a title and body!";
        }
    }

    private void onCancelBtnPress()
    {
        gameObject.SetActive(false);
        publicUICanvas.GetComponent<fillNewListview>().showUI();
        title.text = "";
        body.text = "";
        messageBox.text = "";
    }
}
