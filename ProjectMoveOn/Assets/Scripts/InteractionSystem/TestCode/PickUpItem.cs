#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private Transform PickUpPointR;
    [SerializeField] private Transform PickUpPointL;
    
    private Transform player;

    public float pickUpDistance;

    public bool itemIsPickedR = false;
    public bool itemIsPickedL = false;
    //public bool keepLamb = false; 

    public bool itemInteracted = false;

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
<<<<<<< HEAD
            if (Keyboard.current.eKey.wasPressedThisFrame )
            {
                if (itemIsPickedR ==false && PickUpPointR.childCount < 1 )
=======
<<<<<<< Updated upstream
            if (Keyboard.current.eKey.wasPressedThisFrame && itemIsPicked == false && PickUpPoint.childCount < 1)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                this.transform.position = PickUpPoint.position;
                this.transform.parent = GameObject.Find("PickupPoint").transform;
=======
            if (Keyboard.current.eKey.wasPressedThisFrame )
            {
                
                if (itemIsPickedR ==false && PickUpPointR.childCount < 1 && itemInteracted == false )
>>>>>>> PickUp&PushTestCode
                {
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<Collider>().isTrigger = true;
                    this.transform.position = PickUpPointR.position;
                    this.transform.parent = GameObject.Find("PickupPointR").transform;
                    itemIsPickedR = true;
<<<<<<< HEAD
=======
                    Invoke("ChangeBool", 0.75f);
>>>>>>> PickUp&PushTestCode
                    Debug.Log("R:"+itemIsPickedR.ToString());
                    if(this.gameObject.name == "Lamp (2)")
                    {
                        StarterAssets.ThirdPersonController.Instance.keepLamb = true;
                    }
                }
                else
                {
                    this.transform.parent = null;
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Collider>().isTrigger = false;
                    itemIsPickedR = false;
<<<<<<< HEAD
                    Debug.Log("R"+itemIsPickedR.ToString());
                }
                
=======
                    itemInteracted = false;
                    Debug.Log("R"+itemIsPickedR.ToString());
                }
                
            }

            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                if (itemIsPickedL == false && PickUpPointL.childCount < 1 && itemInteracted == false)
                {
                    GetComponent<Rigidbody>().useGravity = false;
                    GetComponent<Collider>().isTrigger = true;
                    this.transform.position = PickUpPointL.position;
                    this.transform.parent = GameObject.Find("PickupPointL").transform;
                    itemIsPickedL = true;
                    Invoke("ChangeBool", 0.75f);
                    Debug.Log("L"+itemIsPickedL.ToString());
                }
                else
                {
                    this.transform.parent = null;
                    GetComponent<Rigidbody>().useGravity = true;
                    GetComponent<Collider>().isTrigger = false;
                    itemIsPickedL = false;
                    itemInteracted = false;
                    Debug.Log("L" + itemIsPickedL.ToString());
                }
>>>>>>> Stashed changes

                itemIsPicked = true;
>>>>>>> PickUp&PushTestCode
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
<<<<<<< Updated upstream

    }
<<<<<<< HEAD

=======
=======
        
    }
    public void ChangeBool()
    {
        itemInteracted = true;
    }
    

>>>>>>> Stashed changes
>>>>>>> PickUp&PushTestCode
}
#endif