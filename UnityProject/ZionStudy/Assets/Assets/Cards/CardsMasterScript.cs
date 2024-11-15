using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsMasterScript : MonoBehaviour
{
    public Button createNewCardSet;
    public Button goBackFromCreateCardBtn;
    public MasterScript master;
    public GameObject cardsetNameCanvas;

    void Start()
    {
        createNewCardSet.onClick.AddListener(openCreateCardsCanvas);
        goBackFromCreateCardBtn.onClick.AddListener(goBack);
    }

    private void openCreateCardsCanvas()
    {
        master.closeNavBarCanvas();
        cardsetNameCanvas.SetActive(true);
    }

    private void goBack()
    {
        master.startCardsCanvas();
        master.startNavBarCanvas();
    }
}
