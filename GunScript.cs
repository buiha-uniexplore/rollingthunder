using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunScript : MonoBehaviour
{
    [SerializeField]
    private float damage;
    private int scoreGet = 10;
    private float range = 100f;
    [SerializeField]
    private float shootAngle;
    [SerializeField]
    private float knockBackTime = 1f;
    private bool canShoot = true;
    

    private Vector3 shootDirection;

    public Camera cam;
    public GameObject player;
    private PlayerData playerData;
    public Canvas canvas;
    public GameObject hitEffect;
    public ParticleSystem muzzleFlash;
    public TMP_Text scoreText;
    [SerializeField]
    private GameObject dialogueManager;
    private bool shootPillarOnce = false;


    private void Start()
    {
        playerData = player.GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && canShoot)
        {
            Shoot();
        }
    }

    private void Shoot()
    {

        muzzleFlash = GameObject.Find("Muzzle Flash").GetComponent<ParticleSystem>();
        var muzzleFlashmain = muzzleFlash.main;

        Color muzzleFlashColor;
        string playerBulletColor = playerData.GetMixedColor().ToString();
        if(playerBulletColor == "")
        {
            return;
        }
        else
        {          


            DeductBullet(playerBulletColor);

            if (ColorUtility.TryParseHtmlString(playerBulletColor, out muzzleFlashColor))
            {
                muzzleFlashmain.startColor = new ParticleSystem.MinMaxGradient(muzzleFlashColor);
            }
            else
            {
                switch (playerBulletColor)
                {
                    case "orange":
                        ColorUtility.TryParseHtmlString("#FFA500", out muzzleFlashColor);
                        break;
                    default:
                        Debug.Log("Nonexistent color");
                        break;
                }

                muzzleFlashmain.startColor = new ParticleSystem.MinMaxGradient(muzzleFlashColor);
            }




            muzzleFlash.Play();
            int layerMask = 1 << 7;
            layerMask = ~layerMask;

            /*shootDirection = Vector3.up*shootAngle + player.transform.forward;
            Ray ray = new Ray(player.transform.position, shootDirection);
            Debug.DrawRay(ray.origin, ray.direction * range);*/
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, layerMask))
            {
                Debug.Log(hit.transform.name);
                GameObject hitGameObject = hit.transform.gameObject;

                if (hitGameObject.CompareTag("Target"))
                {
                    TargetScript targetScript = hitGameObject.GetComponent<TargetScript>();
                    EnemyMoveScript enemyMoveScript = hitGameObject.GetComponent<EnemyMoveScript>();

                    if (playerData.GetMixedColor() == targetScript.GetData().GetColor())
                    {
                        enemyMoveScript.ChangeCanBeHitStatus(true);
                        if (!targetScript.TakeDamage(10))
                        {
                            int playerScore = playerData.GetScore();
                            playerData.SetScore(playerScore + scoreGet, out playerScore);

                        }
                        KnockBack(hitGameObject);
                    }

                }else if (hitGameObject.CompareTag("Pillar"))
                {
                    Debug.Log("Hit pillar");
                    PillarScript pillarScript = hitGameObject.GetComponent<PillarScript>();
                    pillarScript.SpawnBullets();

                    if (!shootPillarOnce)
                    {
                        dialogueManager.GetComponent<ExplainationScript>().ShootPillar = true;
                        shootPillarOnce = true;
                    }
                    
                }

                LineRenderer shootingLine = gameObject.GetComponent<LineRenderer>();
                shootingLine.enabled = true;
                Vector3 startPos = gameObject.transform.position + Vector3.up*0.1f;
                Vector3 endPos = hit.point;
                shootingLine.SetPosition(0, startPos);
                shootingLine.SetPosition(1, endPos);
                shootingLine.material.color = muzzleFlashColor;
                StartCoroutine(WaitThenDisable(shootingLine));



                var hitEffectMain = hitEffect.GetComponent<ParticleSystem>().main;
                hitEffectMain.startColor = new ParticleSystem.MinMaxGradient(muzzleFlashColor);
                hitEffect.transform.localScale = new Vector3(3f, 3f, 3f);

                GameObject go = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Debug.Log("Spawned hiteffect");

                Destroy(go, 0.2f);
            }
        }

    }

    private void KnockBack(GameObject hitGameObject)
    {
        EnemyMoveScript enemyMoveScript = hitGameObject.GetComponent<EnemyMoveScript>();
        enemyMoveScript.knockBackTime = knockBackTime;
    }

    private void DeductBullet(string playerBulletColor)
    {
        switch (playerBulletColor)
        {
            case "red":
                playerData.BulletCountRed -= 2;
                break;
            case "yellow":
                playerData.BulletCountYellow -= 2;
                break;
            case "blue":
                playerData.BulletCountBlue -= 2;
                break;
            case "magenta":
                playerData.BulletCountBlue -= 1;
                playerData.BulletCountRed -= 1;
                break;
            case "green":
                playerData.BulletCountBlue -= 1;
                playerData.BulletCountYellow -= 1;
                break;
            case "orange":
                playerData.BulletCountRed -= 1;
                playerData.BulletCountYellow -= 1;
                break;
            default:
                break;

        }
    }

    public void DisableShooting()
    {
        canShoot = false;
    }

    private IEnumerator WaitThenDisable(LineRenderer shootingLine)
    {
        yield return new WaitForSeconds(0.05f);
        shootingLine.enabled = false;
    }

    
}
