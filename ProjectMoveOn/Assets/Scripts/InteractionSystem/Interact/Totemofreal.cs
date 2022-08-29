using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totemofreal : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => throw new System.NotImplementedException();

    public bool Interact(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(ThirdPersonController thirdPersonController)
    {
        //playanimation
        //setactive true bridge
        return true;
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
