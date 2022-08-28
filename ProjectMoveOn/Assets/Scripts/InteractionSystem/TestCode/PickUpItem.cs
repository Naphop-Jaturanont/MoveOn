using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpItem : MonoBehaviour
{
    private Transform PickUpPoint;
    private Transform player;

    public float pickUpDistance;

    public bool itemIsPicked;

    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("PlayerArmature").transform;
        PickUpPoint = GameObject.Find("PickupPoint").transform;
    }

    public void PickUpLamp()
    {
        if (!itemIsPicked)
        {
            itemIsPicked = true;
           
        }
    }
    void Update()
    {
        pickUpDistance = Vector3.Distance(player.position, transform.position);

        if(pickUpDistance <= 2)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame && itemIsPicked == false && PickUpPoint.childCount < 1)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().isTrigger = true;
                this.transform.position = PickUpPoint.position;
                this.transform.parent = GameObject.Find("PickupPoint").transform;

                itemIsPicked = true;
            }
        }

        if(Keyboard.current.qKey.wasPressedThisFrame&& itemIsPicked == true)
        {
            this.transform.parent = null;
            itemIsPicked = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }
}
