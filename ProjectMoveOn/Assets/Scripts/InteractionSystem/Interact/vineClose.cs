using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vineClose : MonoBehaviour, IInteractable
{
    public string InteractionPrompt => throw new System.NotImplementedException();

    public bool Interact(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(ThirdPersonController thirdPersonController)
    {
        //playanumation        
        return true;
    }

    public void afterburn()
    {
        this.gameObject.SetActive(false);
    }
}
