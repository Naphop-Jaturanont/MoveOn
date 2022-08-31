#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]private Transform PickUpPointR;
    [SerializeField] private Transform PickUpPointL;
    private Transform player;

    public float pickUpDistance;

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
        pickUpDistance = Vector3.Distance(player.position, transform.position);

        if(pickUpDistance <= 2)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame )
            {
                if (itemIsPickedR ==false && PickUpPointR.childCount < 1 )
                {
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<Collider>().isTrigger = true;
                    this.transform.position = PickUpPointR.position;
                    this.transform.parent = GameObject.Find("PickupPointR").transform;
                    itemIsPickedR = true;
                    Debug.Log("R:"+itemIsPickedR.ToString());
                    
                }
                else
                {
                    this.transform.parent = null;
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Collider>().isTrigger = false;
                    itemIsPickedR = false;
                    Debug.Log("R"+itemIsPickedR.ToString());

                }
                
                
            }

            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                if (itemIsPickedL == false && PickUpPointL.childCount < 1)
                {
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<Collider>().isTrigger = true;
                    this.transform.position = PickUpPointL.position;
                    this.transform.parent = GameObject.Find("PickupPointL").transform;
                    itemIsPickedL = true;
                    Debug.Log("L"+itemIsPickedL.ToString());

                }
                else
                {
                    this.transform.parent = null;
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Collider>().isTrigger = false;
                    itemIsPickedL = false;
                    Debug.Log("L" + itemIsPickedL.ToString());
                    

                }

                
            }
        }


    }
}
#endif