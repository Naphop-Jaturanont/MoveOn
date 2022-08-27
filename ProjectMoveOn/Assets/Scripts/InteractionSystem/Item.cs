using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;
    
    public bool Interact(Interactor interactor)
    {
        var pickup = interactor.GetComponent<PickupAndDrop>();

        if (pickup == null) return false;

        if (pickup.Haskey)
        {
            Debug.Log("Get it");
            return true;
        }

        Debug.Log("log");
        return false;
    }
}
