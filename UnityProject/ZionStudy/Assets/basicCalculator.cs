using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class basicCalculator : MonoBehaviour
{
    public TMP_InputField problemInput;
    public TextMeshProUGUI problemOutput;
    public Button submitBtn;
    public GameObject parentCanvas;
    public Button goBackBtn;

    private void Awake() 
    {
        submitBtn.onClick.AddListener(solve);
        goBackBtn.onClick.AddListener(closeCanvas);
    }


    private void solve()
    {
        if(!string.IsNullOrEmpty(problemInput.text))
        {
            string input = problemInput.text;
            input = input.Trim();
            if(input.Any(x => char.IsLetter(x)))
            {
                //break
                problemOutput.text = "No letters allowed!";
            }
            else
            {
                try
                {
                    DataTable table  = new DataTable();
                    object results = table.Compute(input, string.Empty);
                    problemOutput.text = results.ToString();
                }
                catch(Exception ex)
                {
                    problemOutput.text = "Invalid problem";
                    Debug.Log(ex.Message);
                }
            }
        }
    }

    private void closeCanvas()
    {
        gameObject.SetActive(false);
        parentCanvas.SetActive(true);
    }
}
