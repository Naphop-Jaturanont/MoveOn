using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keep : MonoBehaviour, IInteractable
{
    public Rigidbody rb;
    public Collider collider;
    public GameObject gameObject;

    [SerializeField]private Transform PickUpPointR = null;
    [SerializeField]private Transform PickUpPointL = null;
    public bool keeped = false;
    bool right = false;
    bool left = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        
        if(keeped == true)
        {
            Debug.Log("keep");
            if(right == true)
            {
                gameObject.transform.position = PickUpPointR.position;
                Debug.Log("keep1");
                return;
            }
            if (left == true)
            {
                gameObject.transform.position = PickUpPointL.position;
                Debug.Log("keep2");
                return;
            }
            
        }
    }
    public string InteractionPrompt => throw new System.NotImplementedException();

            
    
    public bool Interact(Interactor interactor)
    {
        //itemIsPickedR = true;
        throw new System.NotImplementedException();
    }   

    public bool Interact(ThirdPersonController thirdPersonController)
    {
        throw new System.NotImplementedException();
    }

    public bool InteractL(Interactor interactor)
    {
        PickUpPointL = interactor.PickUpPointL;
        interactor.handLeft = true;
        rb.useGravity = false;
        collider.enabled = false;
        this.gameObject.transform.position = interactor.PickUpPointL.position;
        this.transform.parent = GameObject.Find("Left_Hand").transform;
        keeped = true;
        left = true;
        return true;
    }

    public bool InteractR(Interactor interactor)
    {
        PickUpPointR = interactor.PickUpPointR;
        interactor.handRight = true;
        rb.useGravity = false;
        collider.enabled = false;
        this.gameObject.transform.position = interactor.PickUpPointR.position;
        this.transform.parent = GameObject.Find("Right_Hand").transform;
        keeped = true;
        right = true;
        return true;
    }
}
