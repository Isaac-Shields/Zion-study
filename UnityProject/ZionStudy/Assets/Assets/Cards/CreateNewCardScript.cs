using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewCardScript : MonoBehaviour
{
    public TextMeshProUGUI title;
    public Button goBackBtn;
    public Button addCardBtn;
    public TMP_InputField question;
    public TMP_InputField answer;
    public MasterScript master;
    public DatabaseHelper dbHelper;
    private int operation;
    public PracticeCanvasMaster pcm;

    private void Start() 
    {
        goBackBtn.onClick.AddListener(closeThisCanvas);
        addCardBtn.onClick.AddListener(checkCardset);    
    }

    //Close this canvas
    private void closeThisCanvas()
    {
        if(operation == 0)
        {
            question.text = "";
            answer.text = "";
            master.closeAddCardsCanvas();
            master.startCardsCanvas();
            operation = 0;
        }
        else if(operation == 1)
        {
            question.text = "";
            answer.text = "";
            master.closeAddCardsCanvas();
            pcm.loadAllProblems(master.curCard.getCardsetTitle(), master.curSessionData.getUserId());
            operation = 0;
        }
    }

    //If there's no cardset, create one, else, add a card to the existing cardset
    private void checkCardset()
    {
        if(master.curCard.getSetId() == -1)
        {
            createCardset();
            master.curCard.setCardsetTitle(title.text);
        }
        else
        {
            addCard();
        }
    }

    //Create cardset
    private void createCardset()
    {
        if(dbHelper.createCardset(title.text, master.curSessionData.getUserId()))
        {
            master.curCard.setId(dbHelper.getCardsetId(title.text, master.curSessionData.getUserId()));
            if(question.text.Length > 0 && answer.text.Length > 0)
            {
                addCard();
            }

            Debug.Log("Cardset created");
        }
        else
        {
            Debug.Log("Error creating cardset");
        }
    }

    //Add a card to a cardset
    private void addCard()
    {
        if(dbHelper.addCardToCardset(question.text, answer.text, master.curCard.getSetId()))
        {
            Debug.Log("Added problem");
            question.text = "";
            answer.text = "";
        }
        else
        {
            Debug.Log("Error adding card");
        }
    }

    //setter
    public void setAltOperation(int opN)
    {
        operation = opN;
    }


}
