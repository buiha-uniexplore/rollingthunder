using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pillarUI;
    [SerializeField]
    public Material[] materials;
    [SerializeField]
    private GameObject[] pillars;
    [SerializeField]
    private GameObject pillar;
    private float range = 10f;
    [SerializeField]
    private Camera cam;
    private GameObject preplacingPillar;
    private bool pillarSpawned = false;

    private SpriteRenderer pillarUISpriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        pillarUISpriteRenderer = pillarUI.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("x"))
        {
            if(pillarSpawned == false)
            {
                SpawnPillar();
            }
            PreplacePillar();
        }
        if (Input.GetKeyUp("x"))
        {
            if(preplacingPillar.GetComponent<PillarScript>().CollisionCount == 0)
            {
                Destroy(preplacingPillar);
            }
            else
            {
                Color pillarColor = preplacingPillar.GetComponent<MeshRenderer>().material.color;
                pillarColor.a = 1f;
                preplacingPillar.GetComponent<MeshRenderer>().material.color = pillarColor;

                Instantiate(preplacingPillar, preplacingPillar.transform.position, Quaternion.identity);
                Destroy(preplacingPillar);
            }
            pillarSpawned = false;
        }
        ChangePillar();
        
    }

    void SpawnPillar()
    {
        Vector3 camPosition = cam.transform.position;

        preplacingPillar = Instantiate(pillar, camPosition + Vector3.forward * range, Quaternion.identity);
        Color pillarColor = preplacingPillar.GetComponent<MeshRenderer>().material.color;
        pillarColor.a = 0.5f;
        preplacingPillar.GetComponent<MeshRenderer>().material.color = pillarColor;

        pillarSpawned = true;     

    }

    void PreplacePillar()
    {
        int layerMask = 1 << 3;
        RaycastHit hit;
        Vector3 premovePillarPosition = preplacingPillar.transform.position;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, layerMask))
        {
            preplacingPillar.transform.position = hit.point + Vector3.up * 0.4f ;
        }
        else
        {
            preplacingPillar.transform.position = cam.transform.position + cam.transform.forward * 10f + Vector3.up * 0.4f;
        }
        Vector3 aftermovePillarPosition = preplacingPillar.transform.position;
        preplacingPillar.GetComponent<Rigidbody>().velocity = aftermovePillarPosition - premovePillarPosition;
    }

    void ChangePillar()
    {
        if (Input.GetKeyUp("c"))
        {
            ColorScript.ColorType currentColor = preplacingPillar.GetComponent<PillarScript>().PillarColor;

            if (currentColor == ColorScript.ColorType.red)
            {
                preplacingPillar.GetComponent<MeshRenderer>().material = materials[1];
                preplacingPillar.GetComponent<PillarScript>().PillarColor = ColorScript.ColorType.blue;
                pillarUISpriteRenderer.color = Color.blue;
            }
            else if (preplacingPillar.GetComponent<PillarScript>().PillarColor == ColorScript.ColorType.blue)
            {
                preplacingPillar.GetComponent<MeshRenderer>().material = materials[2];
                preplacingPillar.GetComponent<PillarScript>().PillarColor = ColorScript.ColorType.yellow;
                pillarUISpriteRenderer.color = Color.yellow;

            }
            else if (preplacingPillar.GetComponent<PillarScript>().PillarColor == ColorScript.ColorType.yellow)
            {
                preplacingPillar.GetComponent<MeshRenderer>().material = materials[0];
                preplacingPillar.GetComponent<PillarScript>().PillarColor = ColorScript.ColorType.red;
                pillarUISpriteRenderer.color = Color.red;
            }
            Debug.Log("Changed pillar color to" + preplacingPillar.GetComponent<PillarScript>().PillarColor);

            Color pillarColor = preplacingPillar.GetComponent<MeshRenderer>().material.color;
            pillarColor.a = 0.5f;
            preplacingPillar.GetComponent<MeshRenderer>().material.color = pillarColor;
        }
    }

}
