using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    [SerializeField]
    private ColorScript.ColorType bulletColor;
    public ColorScript.ColorType BulletColor
    {
        get { return bulletColor; }
        set { bulletColor = value;
            Color color;
            if (ColorUtility.TryParseHtmlString(bulletColor.ToString(), out color))
            {
                //Debug.Log("Changing bullet color");
                gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", color);
            }

        }
    }
}
