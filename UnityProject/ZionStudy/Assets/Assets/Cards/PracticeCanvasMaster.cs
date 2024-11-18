using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PracticeCanvasMaster : MonoBehaviour
{
    public TextMeshProUGUI problem;
    public GameObject answer;
    public Button showAnswerBtn;
    private bool initBtn = true;
    public MasterScript master;
    public DatabaseHelper dbHelper;
    public Button prevBtn;
    public Button nextBtn;
    private int id;
    private List<problemObj> allProblems;

    private void Start() 
    {
        id = 0;
        showAnswerBtn.onClick.AddListener(reveal);
        prevBtn.onClick.AddListener(prevProblem);
        nextBtn.onClick.AddListener(nextCard);
        answer.SetActive(false);
    }

    public void loadCards(string t)
    {
        allProblems = dbHelper.getAllProblems(t);
        master.closeCardsCanvas();
        master.startPracticeCardsCanvas();
        problem.text = allProblems[id].getProblem();
        answer.GetComponent<TextMeshProUGUI>().text = allProblems[id].getAnswer();
        checkNums();
    }

    private void reveal()
    {
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

    private void nextCard()
    {
        if(id + 1 < allProblems.Count)
        {
            id++;
            checkNums();
        }
        initBtn = false;
        reveal();
        problem.text = allProblems[id].getProblem();
        answer.GetComponent<TextMeshProUGUI>().text = allProblems[id].getAnswer();

    }

    private void prevProblem()
    {
        if(id - 1 >= 0)
        {
            id--;
            checkNums();
        }
        initBtn = false;
        reveal();
        problem.text = allProblems[id].getProblem();
        answer.GetComponent<TextMeshProUGUI>().text = allProblems[id].getAnswer();
    }

    private void checkNums()
    {
        if(id + 1 >= allProblems.Count)
        {
            nextBtn.interactable = false;
        }
        else
        {
            nextBtn.interactable = true;
        }

        if(id - 1 < 0)
        {
            prevBtn.interactable = false;
        }
        else
        {
            prevBtn.interactable = true;
        }
    }

}
