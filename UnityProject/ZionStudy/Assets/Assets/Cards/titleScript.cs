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

    private void goBack()
    {
        this.gameObject.SetActive(false);
        master.startNavBarCanvas();
        master.startCardsCanvas();
        title.text = "";
    }

    private void startAddCard()
    {
        this.gameObject.SetActive(false);
        master.startNavBarCanvas();
        master.startAddCardCanvas();
        master.curCard.setCardsetTitle(title.text);
        title.text = "";
    }
}
