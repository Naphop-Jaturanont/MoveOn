using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;
    
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Get it!"); 
        return true;
    }
}
