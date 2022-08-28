using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hintofleech : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => throw new System.NotImplementedException();

    public bool Interact(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(ThirdPersonController thirdPersonController)
    {
        //playanimation
        //showtext
        return true;
    }
}
