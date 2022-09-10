using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openClock : MonoBehaviour, IInteractable
{
    public GameObject Clock;
    public GameObject Rock;
    public GameObject lamb;
    public string InteractionPrompt => throw new System.NotImplementedException();

    public bool Interact(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(ThirdPersonController thirdPersonController)
    {
        if(ThirdPersonController.Instance.keepLamb == true)
        {
            Rock.SetActive(false);
            Clock.SetActive(true);
            Destroy(lamb.gameObject);
        }
        return true;
    }

    public bool InteractL(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public bool InteractR(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
