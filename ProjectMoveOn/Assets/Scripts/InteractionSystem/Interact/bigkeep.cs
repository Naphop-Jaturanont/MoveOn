using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class bigkeep : MonoBehaviour, IInteractable
{
    public Rigidbody rb;
    public Collider collider;
    public GameObject gameObject;
    public Interactor interactor;

    [SerializeField] private Transform hip = null;
    public bool keeped = false;
    public bool right = false;
    public bool left = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        interactor = GameObject.Find("PlayerArmature").GetComponent<Interactor>();
    }

    private void Update()
    {
        Drop();
    }
    public string InteractionPrompt => throw new System.NotImplementedException();

    public bool Interact(Interactor interactor)
    {
        //hip = interactor.hips;
        interactor.handLeft = true;
        interactor.handRight = true;
        rb.useGravity = false;
        collider.enabled = false;
        this.gameObject.transform.position = gameObject.transform.position;
        this.transform.parent = GameObject.Find("point block").transform;
        keeped = true;
        left = true;
        right = true;
        return true;        
    }

    public bool Interact(ThirdPersonController thirdPersonController)
    {
        throw new System.NotImplementedException();
    }

    public bool InteractL(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public bool InteractR(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    

    private void Drop()
    {
        if(right == true && left == true)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame || Keyboard.current.qKey.wasPressedThisFrame)
            {
                
                Invoke("ChangeBoolPick", 0.1f);
            }
        }
        


    }
    private void ChangeBoolPick()
    {
        this.transform.parent = null;
        Debug.Log("DropR");
        interactor.handRight = false;
        interactor.handLeft = false;
        rb.useGravity = true;
        collider.enabled = true;
        keeped = false;
        right = false;
        left = false;
        return;
    }

}
