using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class navbarBtnHandler : MonoBehaviour
{
    public Button calculators;
    public Button notes;
    public Button cards;
    public Button settings;
    public MasterScript master;

    private void Start() 
    {
        calculators.onClick.AddListener(master.startCalculatorCanvas);
        notes.onClick.AddListener(master.startNotesCanvas);
        cards.onClick.AddListener(master.startCardsCanvas);
        settings.onClick.AddListener(master.startSettingsCanvas);
    }
}
