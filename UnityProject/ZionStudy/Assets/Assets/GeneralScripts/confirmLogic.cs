using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class confirmLogic : MonoBehaviour
{
    public Button confirmBtn;
    public Button denyBtn;
    public TextMeshProUGUI message;
    private bool allGood;
    public PracticeCanvasMaster pcm;
    public int operation;
    public TextMeshProUGUI messageText;
    public MasterScript master;
    public DatabaseHelper dbHelper;
    private List<problemObj> allProblems = new List<problemObj>();
    public int probPos;

    private void Start() 
    {
        allGood = false;
        confirmBtn.onClick.AddListener(confirmed);
        denyBtn.onClick.AddListener(denied);
    }

    private void confirmed()
    {
        if(operation == 1)
        {
            //delete problem
            allProblems = dbHelper.getAllProblems(master.curCard.getCardsetTitle());
            if(allProblems.Count == 1)
            {
                if(dbHelper.deleteCardSet(master.curCard.getSetId()))
                {
                    allGood = true;
                    master.curCard.clearData();
                    master.closePracticeCardsCanvas();
                    master.startCardsCanvas();
                }
            }
            else
            {
                if(dbHelper.deleteProblem(allProblems[probPos].getId()))
                {
                    allGood = true;
                    pcm.loadCards(master.curCard.getCardsetTitle());
                }
            }
            if(!allGood)
            {
                messageText.text = "Error Deleting";
                messageText.color = Color.red;
            }

            gameObject.SetActive(false);

        }
        else if(operation == 2)
        {
            //delete problem set
            if(dbHelper.deleteCardSet(master.curCard.getSetId()))
            {
                allGood = true;
                master.curCard.clearData();
                master.closePracticeCardsCanvas();
                master.startCardsCanvas();
            }

            if(!allGood)
            {
                messageText.text = "Error Deleting";
                messageText.color = Color.red;
            }

            gameObject.SetActive(false);
        }
    }

    public void denied()
    {
        gameObject.SetActive(false);
    }

}
