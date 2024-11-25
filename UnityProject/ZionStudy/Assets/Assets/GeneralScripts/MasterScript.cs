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
    public UserSessionData curSessionData;
    public GameObject noteRud;
    public cardsetObj curCard;
    public GameObject practiceCardsCanvas;
    private void Start() 
    {
        curSessionData = new UserSessionData();
        curCard = new cardsetObj();
        cleanUpBeforeSwitch();
        startLoginCanvas();
    }


    public void startPracticeCardsCanvas()
    {
        cleanUpBeforeSwitch();
        practiceCardsCanvas.SetActive(true);
    }

    public void closePracticeCardsCanvas()
    {
        cleanUpBeforeSwitch();
        practiceCardsCanvas.GetComponent<PracticeCanvasMaster>().resetCanvas();
        practiceCardsCanvas.SetActive(false);
    }

    public void closeSignupCanvas()
    {
        cleanUpBeforeSwitch();
        signupCanvas.SetActive(false);
        signupCanvas.GetComponent<SignupScriptMaster>().signupCanvasCleanup();
    }


    public void startNoteRUDCanvas()
    {
        noteRud.SetActive(true);
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
        notesCanvas.GetComponent<NotesListview>().fillListView();
    }

    public void startCardsCanvas()
    {
        cleanUpBeforeSwitch();
        cardsCanvas.SetActive(true);
        cardsCanvas.GetComponent<CardsMasterScript>().fillCardsets();
    }

    public void startLoginCanvas()
    {
        cleanUpBeforeSwitch();
        loginCanvas.GetComponent<LoginMasterScript>().loginCanvasCleanup();
        navBarCanvas.SetActive(false);
        loginCanvas.SetActive(true);
        curSessionData.clearData();
        curCard.clearData();
    }

    public void startAddNotesCanvas()
    {   
        cleanUpBeforeSwitch();
        closeNavBarCanvas();
        createNotesCanvas.SetActive(true);
    }

    public void startAddCardCanvas()
    {
        cleanUpBeforeSwitch();
        createCardsCanvas.SetActive(true);
        createCardsCanvas.GetComponent<CreateNewCardScript>().title.text = curCard.getCardsetTitle();
    }

    public void closeCalculatorCanvas()
    {

    }

    public void closeNavBarCanvas()
    {
        navBarCanvas.SetActive(false);
    }


    public void closeNoteRudCanvas()
    {
        noteRud.SetActive(false);
    }

    public void closeCardsCanvas()
    {
        cardsCanvas.SetActive(false);
    }

    public void closeLoginCanvas()
    {

    }

    public void closeNotesCanvas()
    {
        notesCanvas.SetActive(false);
    }

    public void closeAddNoteCanvas()
    {
        createNotesCanvas.GetComponent<NotesAddNote>().clearCanvas();
        createNotesCanvas.SetActive(false);
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
        noteRud.SetActive(false);
        practiceCardsCanvas.SetActive(false);
        

    }

}
