using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class publicCardsetPreview : MonoBehaviour
{
    public GameObject masterObj;
    private DatabaseHelper dbHelper;
    private MasterScript master;
    public Transform commentPanel;
    public Button addComment;
    public Button openCardset;
    public GameObject createCommentCanvas;
    public GameObject commentsTemplate;
    public TextMeshProUGUI cardsetTitle;
    public PracticeCanvasMaster pcm;
    private int sid;
    public Button goBackBtn;

    void Start()
    {
        master = masterObj.GetComponent<MasterScript>();
        dbHelper = masterObj.GetComponent<DatabaseHelper>();
        addComment.onClick.AddListener(openCreateCommentCanvas);
        openCardset.onClick.AddListener(loadPCM);
        goBackBtn.onClick.AddListener(onGoBackBtnPress);
    }

    public void startScript(int s, string t)
    {
        sid = s;
        cardsetTitle.text = t;
        master = masterObj.GetComponent<MasterScript>();
        master.startPublicCardsetCanvas();
        loadComments();
    }

    public void loadComments()
    {
        dbHelper = masterObj.GetComponent<DatabaseHelper>();
        List<commentObj> comments = dbHelper.getComments(sid);
        foreach(Transform child in commentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach(commentObj curComment in comments)
        {
            GameObject listItem = Instantiate(commentsTemplate, commentPanel);
            listItem.transform.Find("title").GetComponent<TextMeshProUGUI>().text = curComment.getTitle();
            listItem.transform.Find("body").GetComponent<TextMeshProUGUI>().text = curComment.getBody();
        }
    }

    private void loadPCM()
    {
        pcm.loadAllProblems(cardsetTitle.text, sid);
    }

    private void openCreateCommentCanvas()
    {
        createCommentCanvas.SetActive(true);
        createCommentCanvas.GetComponent<createComment>().setId = sid;
    }

    private void onGoBackBtnPress()
    {
        master.closePublicCardsetCanvas();
        master.startCardsCanvas();
    }
}
