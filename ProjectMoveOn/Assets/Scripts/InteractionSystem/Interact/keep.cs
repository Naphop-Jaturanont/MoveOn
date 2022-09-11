using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class keep : MonoBehaviour, IInteractable
{
    public Rigidbody rb;
    public Collider collider;
    public GameObject gameObject;

    [SerializeField]private Transform PickUpPointR = null;
    [SerializeField]private Transform PickUpPointL = null;
    public Interactor Interactor;
    public bool keeped = false;
    public bool right = false;
    public bool left = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        Interactor = GameObject.Find("PlayerArmature").GetComponent<Interactor>();
    }

    private void Update()
    {
        
        if(keeped == true)
        {
            if(right == true)
            {
                
                gameObject.transform.position = PickUpPointR.position;
                
                
                return;
            }
            if (left == true)
            {
                gameObject.transform.position = PickUpPointL.position;
                
                return;
            }
            DropR();
            DropL();
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
    private void DropR()
    {
        if (Input.GetKeyDown(KeyCode.E) && right == true)
        {
            Debug.Log("DropR");
            Interactor.handRight = false;
            rb.useGravity = true;
            collider.enabled = true;
            keeped = true;
            Invoke("ChangeBoolPick", 0.1f);
        }
    }
    private void DropL()
    {
        if (Input.GetKeyDown(KeyCode.Q) && left ==true)
        {
            Interactor.handLeft = false;
            rb.useGravity = true;
            collider.enabled = true;
            keeped = false;      
            Invoke("ChangeBoolPick", 0.1f);
        }
    }
    private void ChangeBoolPick()
    {
        if (right == true)
        {
            right = false;
            return;
        }
        if (left == true)
        {
            left = false;
            return;
        }
    }
}
