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
    public List<string> cardsetTitles;
    public DatabaseHelper dbHelper;
    public PracticeCanvasMaster pcm;

    void Start()
    {
        createNewCardSet.onClick.AddListener(openCreateCardsCanvas);
        fillCardsets();
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

        foreach(string title in cardsetTitles)
        {
            GameObject listItem = Instantiate(cardsetTemplate, privateContent);
            Button link = listItem.GetComponent<Button>();
            link.onClick.AddListener(() => pcm.loadCards(title));
            link.GetComponentInChildren<TextMeshProUGUI>().text = title;
        }
    }
}
