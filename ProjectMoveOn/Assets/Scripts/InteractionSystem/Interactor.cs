using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
     [SerializeField] private Transform interactionPoint;
     [SerializeField] private float interactionPointRadius = 0.5f;
     [SerializeField] private LayerMask interactableMask;

     private readonly Collider[] _colliders = new Collider[3];
     [SerializeField] private int numFound;

    [SerializeField]private StarterAssets.StarterAssetsInputs inputs;

    private void Start()
    {
        inputs = GetComponent<StarterAssets.StarterAssetsInputs>();
    }

    private void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, _colliders,
               interactableMask);
        if (numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();
            if (interactable != null && inputs.Interact)
            {
                interactable.Interact(this);
            }
        }
    }
 
     private void OnDrawGizmos()
     {
          Gizmos.color = Color.red;
          Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
     }
}
