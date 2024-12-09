using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class trianlgeScript : MonoBehaviour
{
    public TMP_InputField angle1;
    public TMP_InputField angle2;
    public TMP_InputField angle3;
    public TMP_InputField side1;
    public TMP_InputField side2;
    public TMP_InputField side3;


    private void solveAngle()
    {
        double a1 = double.Parse(angle1.text);
        double a2 = double.Parse(angle2.text);
        double a3 = double.Parse(angle3.text);

        double missingAngle;

        if(a1 == 0)
        {
            missingAngle = 180 - a2 - a3;
        }
        else if(a2 == 0)
        {
            missingAngle = 180 - a1 - a3;
        }
        if(a3 == 0)
        {
            missingAngle = 180 - a1 - a2;
        }
    }

    private void solveHyp()
    {
        double s1 = double.Parse(side1.text);
        double s2 = double.Parse(side2.text);
        double s3 = double.Parse(side3.text);
        double hyp = math.sqrt(math.pow(s1, 2) + math.pow(s2, 2));
    }
}
