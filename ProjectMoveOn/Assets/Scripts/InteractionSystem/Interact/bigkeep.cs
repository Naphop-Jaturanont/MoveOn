using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigkeep : MonoBehaviour, IInteractable
{
    public Rigidbody rb;
    public Collider collider;
    public GameObject gameObject;

    [SerializeField] private Transform hip = null;
    public bool keeped = false;
    bool right = false;
    bool left = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
    public string InteractionPrompt => throw new System.NotImplementedException();

    public bool Interact(Interactor interactor)
    {
        hip = interactor.hips;
        interactor.handLeft = true;
        rb.useGravity = false;
        collider.enabled = false;
        this.gameObject.transform.position = interactor.PickUpPointL.position;
        this.transform.parent = GameObject.Find("Hips").transform;
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
    
}
