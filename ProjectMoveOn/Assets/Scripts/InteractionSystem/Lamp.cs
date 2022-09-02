using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lamp : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform pickUpPointR;
    [SerializeField] private Transform pickUpPointL;
    [SerializeField] private string prompt;

    public string InteractionPrompt => prompt;

    private Transform _pickUpPoint;
    private Transform _player;
    //[SerializeField]private GameObject lamb;

    public float pickUpDistance;

    public bool itemIsPicked;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _player = GameObject.Find("playerArmature").transform;
        pickUpPointR = GameObject.Find("pickupPointR").transform;
        pickUpPointL = GameObject.Find("pickupPointL").transform;
        //_pickUpPoint = GameObject.FindGameObjectWithTag("Lamp").transform;
        //lamb = GameObject.FindGameObjectWithTag("Lamp");
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame && itemIsPicked == true)
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
            Debug.Log(_pickUpPoint);
            transform.position = _pickUpPoint.position;
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
