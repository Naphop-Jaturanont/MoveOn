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
        
        if(lighter.openlamb == true)
        {
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
        renderers.gameObject.SetActive(true);
        startalpha = 1.0f;
        //Color mycolor = new Color(1.0f, 1.0f, 1.0f, startalpha);
        renderers.material.SetFloat("Cutoff", 0f);
        isLoaded = true;        
    }

    void UnLoadScene()
    {
        if (isLoaded)
        {
            startalpha = 0f;
            renderers.material.SetFloat("Cutoff", 1f);
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
