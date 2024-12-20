using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class titleScript : MonoBehaviour
{
    public Button cancelBtn;
    public Button continueBtn;
    public TMP_InputField title;
    public MasterScript master;
    private void Start() 
    {
        cancelBtn.onClick.AddListener(goBack);
        continueBtn.onClick.AddListener(startAddCard);
    }

    //Go back
    private void goBack()
    {
        this.gameObject.SetActive(false);
        master.startNavBarCanvas();
        master.startCardsCanvas();
        title.text = "";
    }

    //Start the next canvas
    private void startAddCard()
    {
        master.curCard.setCardsetTitle(title.text);
        this.gameObject.SetActive(false);
        master.startNavBarCanvas();
        master.startAddCardCanvas();
        title.text = "";
    }
}
