using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lamp : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt;

    private Transform PickUpPoint;
    private Transform player;
    //[SerializeField]private GameObject lamb;

    public float pickUpDistance;

    public bool itemIsPicked;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("PlayerArmature").transform;
        PickUpPoint = GameObject.FindGameObjectWithTag("Lamp").transform;
        //lamb = GameObject.FindGameObjectWithTag("Lamp");
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && itemIsPicked == true)
        {
            this.transform.parent = null;
            itemIsPicked = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }
    public void PickUpLamp()
    {
        if (itemIsPicked == false)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<BoxCollider>().isTrigger = true;
            Debug.Log(PickUpPoint);
            transform.position = PickUpPoint.position;
            this.transform.parent = GameObject.Find("PickupPoint").transform;

            itemIsPicked = true;
        }
        
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Opening door!");
        return true;
    }

    public bool Interact(ThirdPersonController thirdPersonController)
    {
        PickUpLamp();
        return true;
    }
}
