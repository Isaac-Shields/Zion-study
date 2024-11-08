using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotesListview : MonoBehaviour
{
    public List<notesObj> notes;
    public GameObject notesTemplate;
    public Transform content;
    public NotesRUD noterudScript;
    public DatabaseHelper dbHelper;
    public MasterScript master;
    public Button addNewNoteBtn;


    private void Start() 
    {
        addNewNoteBtn.onClick.AddListener(startAddNewNote);
    }

    public void fillListView()
    {
        notes = dbHelper.getAllNotesFromDatabase(master.curSessionData.getUserId());
        foreach(Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach(notesObj noteobj in notes)
        {
            GameObject listItem = Instantiate(notesTemplate, content);
            Button link = listItem.GetComponent<Button>();
            link.onClick.AddListener(() => noterudScript.fillNote(noteobj.getTitle(), noteobj.getBody()));
            link.GetComponentInChildren<TextMeshProUGUI>().text = noteobj.getTitle();
        }
    }

    private void startAddNewNote()
    {
        master.startAddNotesCanvas();
        master.closeNavBarCanvas();
    }
}
