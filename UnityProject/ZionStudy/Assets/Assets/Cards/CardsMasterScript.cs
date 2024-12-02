using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsMasterScript : MonoBehaviour
{
    public Button createNewCardSet;
    public MasterScript master;
    public GameObject cardsetNameCanvas;
    public Transform privateContent;
    public Transform publicContent;
    public GameObject cardsetTemplate;
    public List<cardsetObj> cardsetTitles;
    public DatabaseHelper dbHelper;
    public PracticeCanvasMaster pcm;
    public publicCardsetPreview pcp;
    public fillNewListview fnlv;

    void Start()
    {
        createNewCardSet.onClick.AddListener(openCreateCardsCanvas);
        fillCardsets();
        fillPublicCardset();
    }

    private void openCreateCardsCanvas()
    {
        master.closeNavBarCanvas();
        master.closeCardsCanvas();
        master.curCard.clearData();
        cardsetNameCanvas.SetActive(true);
    }

    //Create cards
    public void fillCardsets()
    {
        cardsetTitles = dbHelper.getPrivateCardsets(master.curSessionData.getUserId());
        foreach(Transform child in privateContent)
        {
            Destroy(child.gameObject);
        }

        foreach(cardsetObj curCard in cardsetTitles)
        {
            GameObject listItem = Instantiate(cardsetTemplate, privateContent);
            Button link = listItem.GetComponent<Button>();
            link.onClick.AddListener(() => pcm.loadAllProblems(curCard.getCardsetTitle(), curCard.getSetId()));
            link.GetComponentInChildren<TextMeshProUGUI>().text = curCard.getCardsetTitle();
        }
    }

    public void fillPublicCardset()
    {
        List<cardsetObj> publicCards = dbHelper.getPublicCardsets();
        foreach(Transform child in publicContent)
        {
            Destroy(child.gameObject);
        }

        foreach(cardsetObj curCard in publicCards)
        {
            GameObject listItem = Instantiate(cardsetTemplate, publicContent);
            Button link = listItem.GetComponent<Button>();
            link.onClick.AddListener(() => fnlv.showData(curCard.getSetId(), curCard.getCardsetTitle()));
            link.GetComponentInChildren<TextMeshProUGUI>().text = curCard.getCardsetTitle();
        }
    }
}
