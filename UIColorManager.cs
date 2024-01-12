using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorManager : MonoBehaviour
{
    [SerializeField]
    private GameObject leftColorUI;
    [SerializeField]
    private GameObject rightColorUI;
    [SerializeField]
    private GameObject leftNextColorUI;
    [SerializeField]
    private GameObject rightNextColorUI;
    [SerializeField]
    private GameObject leftBigColorUI;
    [SerializeField]
    private GameObject rightBigColorUI;

    private PlayerData playerData;

    private Image leftColorImage;
    private Image rightColorImage;
    private Image leftBigColorImage;
    private Image rightBigColorImage;
    private Image leftNextColorImage;
    private Image rightNextColorImage;

    private ColorScript.ColorType leftBulletColor;
    private ColorScript.ColorType rightBulletColor;

    // Start is called before the first frame update
    void Start()
    {
        leftColorUI = GameObject.Find("Left Color");
        rightColorUI = GameObject.Find("Right Color");
        leftBigColorUI = GameObject.Find("Left Color Big");
        rightBigColorUI = GameObject.Find("Right Color Big");
        leftNextColorUI = GameObject.Find("Left Next Color");
        rightNextColorUI = GameObject.Find("Right Next Color");

        leftColorImage = leftColorUI.GetComponent<Image>();
        rightColorImage = rightColorUI.GetComponent<Image>();
        leftBigColorImage = leftBigColorUI.GetComponent<Image>();
        rightBigColorImage = rightBigColorUI.GetComponent<Image>();
        leftNextColorImage = leftNextColorUI.GetComponent<Image>();
        rightNextColorImage = rightNextColorUI.GetComponent<Image>();
        playerData = GameObject.Find("Player").GetComponent<PlayerData>();
    }

    private void Update()
    {
        ChangeColor();
    }

    public void ChangeColor()
    {
        leftBulletColor = playerData.GetLeftColor();
        rightBulletColor = playerData.GetRightColor();

        leftColorImage.color = ParseColor(leftBulletColor);
        rightColorImage.color = ParseColor(rightBulletColor);

        leftBigColorImage.color = ParseColor(leftBulletColor);
        rightBigColorImage.color = ParseColor(rightBulletColor);

        leftNextColorImage.color = ParseColor(ColorTypeMethod.GetNextColor(leftBulletColor));
        rightNextColorImage.color = ParseColor(ColorTypeMethod.GetNextColor(rightBulletColor));
    }

    public Color ParseColor(ColorScript.ColorType color)
    {
        Color parsedColor;
        if (ColorUtility.TryParseHtmlString(color.ToString(), out parsedColor))
        {
            return parsedColor;
        }
        else
        {
            switch (color.ToString())
            {
                case "orange":
                    ColorUtility.TryParseHtmlString("#FFA500", out parsedColor);
                    break;
                default:
                    Debug.Log("Nonexistent color");
                    break;
            }

            return parsedColor;
        }
    }

}
