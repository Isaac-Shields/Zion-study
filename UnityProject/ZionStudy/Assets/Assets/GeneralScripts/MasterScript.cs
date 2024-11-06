using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasterScript : MonoBehaviour
{
    public GameObject loginCanvas;
    public GameObject calculatorCanvas;
    public GameObject navBarCanvas;
    public GameObject notesCanvas;
    public GameObject cardsCanvas;
    public GameObject settingsCanvas;
    public GameObject createCardsCanvas;
    public GameObject createNotesCanvas;
    public GameObject signupCanvas;


    private void Start() 
    {
        cleanUpBeforeSwitch();
        startLoginCanvas();
    }


    public void closeSignupCanvas()
    {
        cleanUpBeforeSwitch();
        signupCanvas.SetActive(false);
        signupCanvas.GetComponent<SignupScriptMaster>().signupCanvasCleanup();
    }


    public void startSettingsCanvas()
    {
        cleanUpBeforeSwitch();
        settingsCanvas.SetActive(true);
    }

    public void startCalculatorCanvas()
    {
        cleanUpBeforeSwitch();
        calculatorCanvas.SetActive(true);
    }

    public void startNavBarCanvas()
    {
        navBarCanvas.SetActive(true);
    }

    public void startNotesCanvas()
    {
        cleanUpBeforeSwitch();
        notesCanvas.SetActive(true);
    }

    public void startCardsCanvas()
    {
        cleanUpBeforeSwitch();
        cardsCanvas.SetActive(true);
    }

    public void startLoginCanvas()
    {
        cleanUpBeforeSwitch();
        loginCanvas.GetComponent<LoginMasterScript>().loginCanvasCleanup();
        navBarCanvas.SetActive(false);
        loginCanvas.SetActive(true);
    }

    public void startAddNotesCanvas()
    {   
        cleanUpBeforeSwitch();
        createNotesCanvas.SetActive(true);
    }

    public void startAddCardCanvas()
    {
        cleanUpBeforeSwitch();
        createCardsCanvas.SetActive(true);
    }

    public void closeCalculatorCanvas()
    {

    }

    public void closeNavBarCanvas()
    {
        navBarCanvas.SetActive(false);
    }

    public void closeCardsCanvas()
    {

    }

    public void closeLoginCanvas()
    {

    }

    public void closeNotesCanvas()
    {

    }

    public void closeAddCardsCanvas()
    {

    }

    public void closeSettingsCanvas()
    {
        settingsCanvas.SetActive(false);
    }

    public void cleanUpBeforeSwitch()
    {
        loginCanvas.SetActive(false);
        calculatorCanvas.SetActive(false);
        notesCanvas.SetActive(false);
        cardsCanvas.SetActive(false);
        notesCanvas.SetActive(false);
        createCardsCanvas.SetActive(false);
        createNotesCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

    }

}
