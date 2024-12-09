using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class powersAndSquares : MonoBehaviour
{
    public TMP_InputField power1;
    public TMP_InputField power2;
    public TMP_InputField square1;
    public TMP_InputField square2;

    public Button powerSubmit;
    public Button squareSubmit;
    public Button goBackBtn;

    public TextMeshProUGUI powerOutput;
    public TextMeshProUGUI squareOutput;

    public GameObject parentCanvas;

    private void Start() 
    {
        powerSubmit.onClick.AddListener(solvePower);
        squareSubmit.onClick.AddListener(solveSquare);
        goBackBtn.onClick.AddListener(closeCanvas);
    }

    private void solvePower()
    {
        if(!string.IsNullOrEmpty(power1.text) && !string.IsNullOrEmpty(power2.text))
        {
            float num1 = float.Parse(power1.text);
            float num2 =  float.Parse(power2.text);
            powerOutput.text = Mathf.Pow(num1, num2).ToString();

        }
    }
    private void solveSquare()
    {
        if(!string.IsNullOrEmpty(square1.text) && !string.IsNullOrEmpty(square2.text))
        {
            float num1 = float.Parse(square1.text);
            float num2 =  float.Parse(square2.text);
            squareOutput.text = Mathf.Pow(num2, 1/num1).ToString();

        }
    }

    private void closeCanvas()
    {
        gameObject.SetActive(false);
        parentCanvas.SetActive(true);
    }
}
