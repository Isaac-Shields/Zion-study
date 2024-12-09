using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startCalcs : MonoBehaviour
{
    public Button basicCalc;
    public Button PowandSqrCalcs;

    public GameObject basicCalcCanvas;
    public GameObject PowandSqrCalcsCanvas;

    private void Start() 
    {
        basicCalc.onClick.AddListener(openBasicCalc);
        PowandSqrCalcs.onClick.AddListener(openPowAndSqrCanvas);
    }

    private void openBasicCalc()
    {
        basicCalcCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    private void openPowAndSqrCanvas()
    {
        PowandSqrCalcsCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
