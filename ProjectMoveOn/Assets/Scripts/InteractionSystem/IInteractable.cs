using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string InteractionPrompt { get; }
    public bool Interact(Interactor interactor);
    public bool InteractL(Interactor interactor);
    public bool InteractR(Interactor interactor);
    public bool Interact(ThirdPersonController thirdPersonController);
}
