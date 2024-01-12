using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTypeMethod : MonoBehaviour
{
    public static ColorScript.ColorType GetNextColor(ColorScript.ColorType currentColor)
    {
        switch (currentColor)
        {
            case ColorScript.ColorType.red:
                return ColorScript.ColorType.blue;
            case ColorScript.ColorType.blue:
                return ColorScript.ColorType.yellow;
            case ColorScript.ColorType.yellow:
                return ColorScript.ColorType.red;
            default:
                Debug.Log("Non existent color on bullet, return default");
                return ColorScript.ColorType.red;
        }
    }
}
