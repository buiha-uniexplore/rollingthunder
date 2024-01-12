using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    [SerializeField]
    private ColorScript.ColorType thisColor;

    public ColorScript.ColorType GetColor()
    {
        return thisColor;
    }
}
