using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerData : Data
{
    [SerializeField]
    private int health;

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health <= 0)
            {
                gameManager.EndGame();
            }
        }
    }
    [SerializeField]
    private int score;
    [SerializeField, Header("Bullet color")]
    private ColorScript.ColorType bulletColorRight;
    [SerializeField]
    private ColorScript.ColorType bulletColorLeft;
    [SerializeField]
    private ColorScript.ColorType? mixedColor;
    [SerializeField, Header("Bullet count")]
    private int bulletCountRed;
    public int BulletCountRed
    {
        get { return bulletCountRed; }
        set { bulletCountRed = value; redBulletText.text = bulletCountRed.ToString();  }
    }
    [SerializeField]
    private int bulletCountBlue;
    public int BulletCountBlue
    {
        get { return bulletCountBlue; }
        set { bulletCountBlue = value; blueBulletText.text = bulletCountBlue.ToString();  }
    }
    [SerializeField]
    private int bulletCountYellow;
    public int BulletCountYellow
    {
        get { return bulletCountYellow; }
        set { bulletCountYellow = value; yellowBulletText.text = bulletCountYellow.ToString(); }
    }

    private HashSet<ColorScript.ColorType> colorList = new HashSet<ColorScript.ColorType>();

    public TMP_Text scoreText;
    public TMP_Text redBulletText;
    public TMP_Text blueBulletText;
    public TMP_Text yellowBulletText;
    public GameObject gameManagerGO;
    private GameManager gameManager;




    void Start()
    {
        gameManager = gameManagerGO.GetComponent<GameManager>();

        redBulletText.text = bulletCountRed.ToString();
        blueBulletText.text = bulletCountBlue.ToString();
        yellowBulletText.text = bulletCountYellow.ToString();

        colorList.Add(bulletColorLeft);
        colorList.Add(bulletColorRight);

        MixColor();
    }

    private void Update()
    {
        //MixColor();
    }

    private void MixColor()
    {
        colorList.Clear();
        colorList.Add(bulletColorLeft);
        colorList.Add(bulletColorRight);

        if (bulletColorLeft == bulletColorRight)
        {
            mixedColor = bulletColorLeft;
            switch (mixedColor)
            {
                case ColorScript.ColorType.red:
                    if (bulletCountRed < 2)
                    {
                        mixedColor = null;
                    }
                    break;
                case ColorScript.ColorType.blue:
                    if (bulletCountBlue < 2)
                    {
                        mixedColor = null;
                    }
                    break;
                case ColorScript.ColorType.yellow:
                    if(bulletCountYellow < 2)
                    {
                        mixedColor = null;
                    }
                    break;
                default:
                    mixedColor = null;
                    break;
            }
            return;
        }


        if (colorList.Contains(ColorScript.ColorType.red) && colorList.Contains(ColorScript.ColorType.blue))
        {
            if (bulletCountRed > 0 && bulletCountBlue > 0)
            {
                mixedColor = ColorScript.ColorType.magenta;
            }
            else mixedColor = null;
            
        }else if (colorList.Contains(ColorScript.ColorType.red) && colorList.Contains(ColorScript.ColorType.yellow))
        {
            if (bulletCountRed > 0 && bulletCountYellow > 0)
            {
                mixedColor = ColorScript.ColorType.orange;
            }
            else mixedColor = null;

        }
        else if(colorList.Contains(ColorScript.ColorType.blue) && colorList.Contains(ColorScript.ColorType.yellow))
        {
            if (bulletCountBlue > 0 && bulletCountYellow > 0)
            {
                mixedColor = ColorScript.ColorType.green;
            }
            else mixedColor = null;
        }

        //Debug.Log("Color mixed");
        return;

    }

    public ColorScript.ColorType GetLeftColor()
    {
        return bulletColorLeft;
    }

    public ColorScript.ColorType GetRightColor()
    {
        return bulletColorRight;
    }

    public ColorScript.ColorType? GetMixedColor()
    {
        MixColor();
        return mixedColor;
    }

    public void SetColor(string side, ColorScript.ColorType color)
    {
        switch (side)
        {
            case "left":
                bulletColorLeft = color;
                break;
            case "right":
                bulletColorRight = color;
                break;
            default:
                Debug.Log("Non existent side");
                break;
        }

        MixColor();
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int setScore, out int scoreSet)
    {
        score = setScore;
        scoreSet = score;

        scoreText.text = "SCORE: " + score;
    }

}
