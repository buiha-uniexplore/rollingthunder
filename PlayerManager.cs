using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private PlayerData playerData;
    private ColorScript.ColorType mixedColor;
    private GameObject colorBallLeft;
    private GameObject colorBallRight;

    // Start is called before the first frame update
    void Start()
    {
        playerData = gameObject.GetComponent<PlayerData>();
        colorBallLeft = GameObject.Find("Color Ball Left");
        colorBallRight = GameObject.Find("Color Ball Right");
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColorLeft();
        ChangeColorRight();
        ReflectColors(); // Should make so that it run only when there are changes
    }

    public void ReflectColors()
    {
        string leftColorString = playerData.GetLeftColor().ToString();
        string rightColorString = playerData.GetRightColor().ToString();

        Material m_left;
        Material m_right;  

        m_left = Resources.Load("Material/" + leftColorString + " Material", typeof(Material)) as Material;
        m_right = Resources.Load("Material/" + rightColorString + " Material", typeof(Material)) as Material;

        colorBallLeft.GetComponent<MeshRenderer>().material = m_left;
        colorBallRight.GetComponent<MeshRenderer>().material = m_right;


    }

    public void ChangeColorLeft()
    {
        if (Input.GetKeyDown("q"))
        {
            var leftColor = playerData.GetLeftColor();
            var nextColor = GetNextColor(leftColor);
            playerData.SetColor("left", nextColor);
        }
    }

    public void ChangeColorRight()
    {
        if (Input.GetKeyDown("e"))
        {
            var rightColor = playerData.GetRightColor();
            var nextColor = GetNextColor(rightColor);
            playerData.SetColor("right", nextColor);
        }
    }

    public ColorScript.ColorType GetNextColor(ColorScript.ColorType currentColor)
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ColorScript.ColorType bulletColor = collision.gameObject.GetComponent<BulletData>().BulletColor;
            switch (bulletColor)
            {
                case ColorScript.ColorType.red:
                    playerData.BulletCountRed += 1;
                    Destroy(collision.gameObject);
                    break;
                case ColorScript.ColorType.blue:
                    playerData.BulletCountBlue += 1;
                    Destroy(collision.gameObject);
                    break;
                case ColorScript.ColorType.yellow:
                    playerData.BulletCountYellow += 1;
                    Destroy(collision.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
