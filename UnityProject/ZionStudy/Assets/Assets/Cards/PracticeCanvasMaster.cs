using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PracticeCanvasMaster : MonoBehaviour
{
    public TMP_InputField problem;
    public GameObject answer;
    public Button showAnswerBtn;
    private bool initBtn = true;
    public MasterScript master;
    public DatabaseHelper dbHelper;
    public Button prevBtn;
    public Button nextBtn;
    private int id;
    public Button goBackBtn;
    public Button editBtn;
    public Button saveBtn;
    public TMP_InputField title;
    public Button cancelBtn;
    public TextMeshProUGUI messageSender;
    private string originalProb;
    private string originalAnswer;
    private string originalTitle;
    public Button deleteProblemBtn;
    public Button deleteSetBtn;
    public GameObject confirmCanvas;
    public Button addProblemBtn;
    public CreateNewCardScript ccs;
    private List<problemObj> allProblems = new List<problemObj>();
    public Button makePublicBtn;

    private void Start() 
    {
        id = 0;
        saveBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        addProblemBtn.onClick.AddListener(addProblem);
        addProblemBtn.gameObject.SetActive(false);
        answer.GetComponent<TMP_InputField>().readOnly = true;
        problem.readOnly = true;
        title.readOnly = true;
        editBtn.onClick.AddListener(startEditing);
        saveBtn.onClick.AddListener(saveBtnLogic);
        cancelBtn.onClick.AddListener(cancelBtnLogic);
        showAnswerBtn.onClick.AddListener(reveal);
        prevBtn.onClick.AddListener(prevProblem);
        nextBtn.onClick.AddListener(nextCard);
        goBackBtn.onClick.AddListener(goBack);
        deleteProblemBtn.onClick.AddListener(startProblemDeletion);
        deleteSetBtn.onClick.AddListener(startSetDeletion);
        answer.SetActive(false);
        deleteProblemBtn.gameObject.SetActive(false);
        deleteSetBtn.gameObject.SetActive(false);
        makePublicBtn.onClick.AddListener(makePublic);
    }

    //Load all the problems in a cardset
    public void loadAllProblems(string t, int sid)
    {
        allProblems.Clear();
        id = 0;
        master.curCard.setCardsetTitle(t);
        master.curCard.setId(sid);
        allProblems = dbHelper.getAllProblems(sid);
        master.closeCardsCanvas();
        master.startPracticeCardsCanvas();
        master.closeNavBarCanvas();
        Debug.Log(master.curCard.getSetId());
        Debug.Log(allProblems.Count);
        problem.text = allProblems[id].getProblem();
        answer.GetComponent<TMP_InputField>().text = allProblems[id].getAnswer();
        title.text = t;
        checkNums();
        if(dbHelper.getCardsetPublicState(master.curCard.getSetId()))
        {
            editBtn.gameObject.SetActive(false);
        }
        else
        {
            editBtn.gameObject.SetActive(true);
        }
    }

    //Reveal the answer, or hide it
    private void reveal()
    {
        messageSender.text = "";
        if(initBtn)
        {
            answer.SetActive(true);
            showAnswerBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Hide answer";
            initBtn = false;
        }
        else
        {
            answer.SetActive(false);
            showAnswerBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Show answer";
            initBtn = true;
        }
    }

    //Switch to the next problem
    private void nextCard()
    {
        messageSender.text = "";
        if(id + 1 < allProblems.Count)
        {
            id++;
            checkNums();
        }
        initBtn = false;
        reveal();
        problem.text = allProblems[id].getProblem();
        answer.GetComponent<TMP_InputField>().text = allProblems[id].getAnswer();

    }

    //Switch to the previous problem
    private void prevProblem()
    {
        messageSender.text = "";
        if(id - 1 >= 0)
        {
            id--;
            checkNums();
        }
        initBtn = false;
        reveal();
        problem.text = allProblems[id].getProblem();
        answer.GetComponent<TMP_InputField>().text = allProblems[id].getAnswer();
    }

    //Stop the user from going out of bounds
    private void checkNums()
    {
        if(id + 1 < allProblems.Count)
        {
            nextBtn.interactable = true;
        }
        else
        {
            nextBtn.interactable = false;
        }

        if(id - 1 >= 0)
        {
            prevBtn.interactable = true;
        }
        else
        {
            prevBtn.interactable = false;
        }
    }

    //Go back
    private void goBack()
    {
        id = 0;
        allProblems.Clear();
        initBtn = true;
        answer.SetActive(false);
        title.readOnly = true;
        saveBtn.gameObject.SetActive(false);
        addProblemBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        answer.GetComponent<TMP_InputField>().readOnly = true;
        problem.readOnly = true;
        showAnswerBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Show answer";
        master.closePracticeCardsCanvas();
        master.startCardsCanvas();
        master.startNavBarCanvas();
        messageSender.text = "";
    }


    //Show buttons for editing
    private void startEditing()
    {
        title.readOnly = false;
        saveBtn.gameObject.SetActive(true);
        cancelBtn.gameObject.SetActive(true);
        deleteProblemBtn.gameObject.SetActive(true);
        deleteSetBtn.gameObject.SetActive(true);
        addProblemBtn.gameObject.SetActive(true);
        originalAnswer = answer.GetComponent<TMP_InputField>().text;
        originalProb = problem.text;
        originalTitle = title.text;

        answer.GetComponent<TMP_InputField>().readOnly = false;
        answer.SetActive(true);
        problem.readOnly = false;

    }

    //Update the problem
    private void saveBtnLogic()
    {
        if(originalAnswer != answer.GetComponent<TMP_InputField>().text || originalProb != problem.text || originalTitle != title.text)
        {
            //update Card
            if(dbHelper.updateProblemSet(allProblems[id].getId(), problem.text, answer.GetComponent<TMP_InputField>().text, title.text, master.curCard.getSetId()))
            {
                messageSender.text = "Updated";
                messageSender.color = Color.green;
                title.readOnly = true;
                saveBtn.gameObject.SetActive(false);
                addProblemBtn.gameObject.SetActive(false);
                cancelBtn.gameObject.SetActive(false);
                answer.GetComponent<TMP_InputField>().readOnly = true;
                problem.readOnly = true;
                deleteProblemBtn.gameObject.SetActive(false);
                deleteSetBtn.gameObject.SetActive(false);
                master.curCard.setCardsetTitle(title.text);
                initBtn = false;
                reveal();

            }
            messageSender.text = "";
        }
        else
        {
            //show error
            messageSender.text = "Nothing to save";
            messageSender.color = Color.red;
        }
    }

    //Cancel update
    private void cancelBtnLogic()
    {
        answer.GetComponent<TMP_InputField>().text = originalAnswer;
        problem.text = originalProb;
        title.text = originalTitle;
        messageSender.text = "";
        addProblemBtn.gameObject.SetActive(false);
        title.readOnly = true;
        saveBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        deleteProblemBtn.gameObject.SetActive(false);
        deleteSetBtn.gameObject.SetActive(false);
        answer.GetComponent<TMP_InputField>().readOnly = true;
        problem.readOnly = true;

        initBtn = false;
        reveal();
    }

    //reset the canvas
    public void resetCanvas()
    {
        id = 0;
        title.readOnly = true;
        saveBtn.gameObject.SetActive(false);
        cancelBtn.gameObject.SetActive(false);
        addProblemBtn.gameObject.SetActive(false);
        answer.GetComponent<TMP_InputField>().readOnly = true;
        problem.readOnly = true;
        deleteProblemBtn.gameObject.SetActive(false);
        deleteSetBtn.gameObject.SetActive(false);
        master.curCard.setCardsetTitle(title.text);
        initBtn = false;
        reveal();
    }

    //start the process of deleting a problem
    private void startProblemDeletion()
    {
        confirmCanvas.SetActive(true);
        confirmCanvas.GetComponent<confirmLogic>().operation = 1;
        confirmCanvas.GetComponent<confirmLogic>().probPos = id;
    }

    //start the process of deleting a cardset
    private void startSetDeletion()
    {
        confirmCanvas.SetActive(true);
        confirmCanvas.GetComponent<confirmLogic>().operation = 2;
    }

    //Start the process of adding a problem to the cardset
    private void addProblem()
    {
        master.closePracticeCardsCanvas();
        master.startAddCardCanvas();
        ccs.setAltOperation(1);

    }

    //Submit cardset for aproval to be made public
    private void makePublic()
    {
        if(dbHelper.updateToPending(master.curCard.getSetId()))
        {
            messageSender.text = "Cardset submitted, awaiting approval";
        }
        else
        {
            messageSender.text = "Unable to submit cardset";
        }
    }
}
