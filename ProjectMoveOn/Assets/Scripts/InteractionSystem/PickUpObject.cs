using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PickUpObject : MonoBehaviour
{
    public GameObject myHands; //reference to your hands/the position where you want your object to go
    bool canpickup; //a bool to see if you can or cant pick up the item
    GameObject ObjectIwantToPickUp; // the game object on which you collided with
    bool hasItem; // a bool to see if you have an item in your hand
    // Start is called before the first frame update
    void Start()
    {
        canpickup = false;    //setting both to false
        hasItem = false;
    }
 
    // Update is called once per frame
    void Update()
    {
        if(canpickup == true) // if you enter thecollider of the objecct
        {
            if (Input.GetKeyDown("e"))  // can be e or any key
            {
                ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true;   //makes the rigidbody not be acted upon by forces
                ObjectIwantToPickUp.transform.position = myHands.transform.position; // sets the position of the object to your hand position
                ObjectIwantToPickUp.transform.parent = myHands.transform; //makes the object become a child of the parent so that it moves with the hands
            }
        }
        if (Input.GetButtonDown("q") && hasItem == true) // if you have an item and get the key to remove the object, again can be any key
        {
            ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false; // make the rigidbody work again
         
            ObjectIwantToPickUp.transform.parent = null; // make the object no be a child of the hands
        }
    }
    private void OnTriggerEnter(Collider other) // to see when the player enters the collider
    {
        if(other.gameObject.tag == "object") //on the object you want to pick up set the tag to be anything, in this case "object"
        {
            canpickup = true;  //set the pick up bool to true
            ObjectIwantToPickUp = other.gameObject; //set the gameobject you collided with to one you can reference
        }
    }
    private void OnTriggerExit(Collider other)
    {
        canpickup = false; //when you leave the collider set the canpickup bool to false
     
    }
}
 