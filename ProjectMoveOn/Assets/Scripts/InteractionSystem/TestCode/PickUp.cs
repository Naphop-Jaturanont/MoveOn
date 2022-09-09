using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{

    [SerializeField] private Transform PickUpPointR;
    [SerializeField] private Transform PickUpPointL;

    private Transform player;

    public bool itemIsPickedR = false;
    public bool itemIsPickedL = false;
    
    private Rigidbody rb;
    
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("PlayerArmature").transform;
        PickUpPointR = GameObject.Find("PickupPointR").transform;
        PickUpPointL = GameObject.Find("PickupPointL").transform;
    }
    void Update()
    {
        DropR();
        DropL();
    }
    void OnTriggerStay(Collider player)
    {
        if(player.tag == "Player")
        {
            if(itemIsPickedR == false)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame && PickUpPointR.childCount < 1)
                {
                    itemIsPickedR = true;
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<Collider>().enabled = false;
                    GetComponent<SphereCollider>().enabled = false;
                    this.transform.position = PickUpPointR.position;
                    this.transform.parent = GameObject.Find("PickupPointR").transform;
                }
            }

            if (itemIsPickedL == false)
            {
                if(Keyboard.current.qKey.wasPressedThisFrame && PickUpPointL.childCount < 1)
                {
                    itemIsPickedL = true;
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<Collider>().enabled = false;
                    GetComponent<SphereCollider>().enabled = false;
                    this.transform.position = PickUpPointL.position;
                    this.transform.parent = GameObject.Find("PickupPointL").transform;                   
                }
            }
        }
    }
    private void DropR()
    {
        if (itemIsPickedR == true)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Collider>().enabled = true;
                GetComponent<SphereCollider>().enabled = true;
                Debug.Log("Drop R");
                Invoke("ChangeBoolPick", 0.1f);
            }
        }
    }
    private void DropL()
    {
        if (itemIsPickedL == true)
        {
            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Collider>().enabled = true;
                GetComponent<SphereCollider>().enabled = true;
                Invoke("ChangeBoolPick", 0.1f);
            }
        }
    }
    private void ChangeBoolPick()
    {
        if(itemIsPickedR == true)
        {
            itemIsPickedR = false;
            return;
        }
        if(itemIsPickedL == true)
        {
            itemIsPickedL = false;
            return;
        }
    }
}
