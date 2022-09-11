using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckMethod
{
    Distance,
    Trigger
}
public class Showinvisible : MonoBehaviour
{
    public Transform player;
    public CheckMethod checkMethod;
    public float loadRange;

    private bool isLoaded;
    private bool shouldLoad;

    //add
    public MeshRenderer renderers;
    public float startalpha = 0f;
    public LighterSystem lighter;
    public Collider collider;

    public float t = 5.0f;
    public float speed = .5f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerArmature").transform;        
        renderers = GetComponent<MeshRenderer>();
        lighter = GameObject.FindGameObjectWithTag("Lamp").GetComponent<LighterSystem>();
        collider = GetComponent<Collider>();
       // renderers.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Material[] mats = renderers.materials;

        if (lighter.openlamb == true)
        {
            Debug.Log(Mathf.Sin(t * speed));
            collider.enabled = true;
            if (checkMethod == CheckMethod.Distance)
            {
                DistanceCheck();
            }
            else if (checkMethod == CheckMethod.Trigger)
            {
                TriggerCheck();
            }
        }
        else
        {
            collider.enabled = false;
        }
        
    }

    

    void DistanceCheck()
    {
        //Checking if the player is within the range
        if (Vector3.Distance(player.position, transform.position) < loadRange)
        {
            if (!isLoaded)
            {
                LoadScene();
            }
        }
        else
        {
            UnLoadScene();
        }
    }
    void TriggerCheck()
    {
        //shouldLoad is set from the Trigger methods
        if (shouldLoad)
        {
            LoadScene();
        }
        else
        {
            UnLoadScene();
        }
    }
    void LoadScene()
    {
        Material[] mats = renderers.materials;
        renderers.gameObject.SetActive(true);        
        mats[0].SetFloat("_Cutoff", Mathf.Sin(t * speed));
        t += Time.deltaTime;
        if(Mathf.Sin(t*speed) <= 0) { mats[0].SetFloat("_Cutoff", 0); }        
        renderers.material = mats[0];        
        isLoaded = true;        
    }

    void UnLoadScene()
    {
        if (isLoaded)
        {
            Material[] mats = renderers.materials;
            renderers.gameObject.SetActive(true);
            mats[0].SetFloat("_Cutoff", Mathf.Sin(t * speed));
            t += Time.deltaTime;
            if (Mathf.Sin(t * speed) >= 0.9f) { mats[0].SetFloat("_Cutoff", 1); }
            renderers.material = mats[0];
            isLoaded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldLoad = false;
        }
    }
}
