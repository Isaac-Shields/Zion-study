using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class fillNewListview : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset commentTemplate;
    private ListView lv;
    public GameObject masterObj;
    private DatabaseHelper dbHelper;
    private MasterScript master;
    public int setid = 0;
    public PracticeCanvasMaster pcm;
    private string title;
    public GameObject createCommentCanvas;
    private Button addCommentBtn;
    private Button goBackBtn;
    private Button openCardsetBtn;

    void Awake() 
    { 
        master = masterObj.GetComponent<MasterScript>();
        dbHelper = masterObj.GetComponent<DatabaseHelper>();
    }

    public void showData(int sid, string t)
    {
        gameObject.SetActive(true);
        setid = sid;
        title = t;
        var root = GetComponent<UIDocument>().rootVisualElement;
        Debug.Log(root);
        lv = root.Q<ListView>("commentLV");

        addCommentBtn = root.Q<Button>("addCommentBtn");
        addCommentBtn.clicked += openCreateCommentCanvas;

        goBackBtn = root.Q<Button>("goBackBtn");
        goBackBtn.clicked += onGoBackBtnPress;

        openCardsetBtn = root.Q<Button>("continueBtn");
        openCardsetBtn.clicked += loadPCM;

        List<commentObj> comments = dbHelper.getComments(sid);
        lv.itemsSource = comments;
        lv.makeItem = createLvItem;
        lv.bindItem = setValues;
        lv.fixedItemHeight = 50;
        lv.Rebuild();
        master.closeCardsCanvas();
        master.closeNavBarCanvas();
        gameObject.SetActive(true);

    }

    private VisualElement createLvItem()
    {
        return commentTemplate.CloneTree();
    }

    private void setValues(VisualElement element, int index)
    {
        commentObj curComment = (commentObj)lv.itemsSource[index];
        var title = element.Q<Label>("title");
        var body = element.Q<Label>("body");
        title.text = curComment.getTitle();
        body.text = curComment.getBody();
    }

    private void loadPCM()
    {
        pcm.loadAllProblems(title, setid);
    }

    private void openCreateCommentCanvas()
    {
        createCommentCanvas.SetActive(true);
        createCommentCanvas.GetComponent<createComment>().setId = setid;
        gameObject.SetActive(false);
    }

    private void onGoBackBtnPress()
    {
        master.closePublicCardsetCanvas();
        master.startCardsCanvas();
        master.startNavBarCanvas();
    }
}
