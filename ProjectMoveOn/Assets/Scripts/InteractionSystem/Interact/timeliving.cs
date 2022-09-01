using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeliving : MonoBehaviour, IInteractable
{
    public GameObject[] gameObjects;
    public GameObject UIpast;
    public GameObject UIpresent;
    public GameObject UIpost;
    //public GameObject Showinmap;
    public bool past = false;
    public bool present = true;
    public bool post = false;
    public bool open = false;

    public string InteractionPrompt => throw new System.NotImplementedException();

    public bool Interact(Interactor interactor)
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(ThirdPersonController thirdPersonController)
    {
        Debug.Log("openoption");
        open = !open;
        if (open)
        {
            if (present == true)
            {
                UIpast.SetActive(open);
                UIpost.SetActive(open);
            }
            if (past == true)
            {
                UIpresent.SetActive(open);
                UIpost.SetActive(open);
            }
            if (post == true)
            {
                UIpresent.SetActive(open);
                UIpast.SetActive(open);
            }
        }
        return true;
    }

    private void Start()
    {
        //Showinmap = gameObjects[1];
        UIpast.SetActive(false);
        UIpresent.SetActive(false);
        UIpost.SetActive(false);
    }

    public void Changetime(string timeyouwant)
    {
        switch (timeyouwant)
        {
            case "past":
                if(past == false)
                {
                    gameObjects[1].SetActive(false);
                    gameObjects[2].SetActive(false);
                    gameObjects[0].SetActive(true);
                    //Showinmap = gameObjects[0].SetActive(true;
                }
                past = true;
                present = false;
                post = false;
                UIpast.SetActive(false);
                UIpost.SetActive(false);
                break;
            case "present":
                if (present == false)
                {
                    gameObjects[0].SetActive(false);
                    gameObjects[2].SetActive(false);
                    gameObjects[1].SetActive(true);
                    //Showinmap = gameObjects[1];
                }
                past = false;
                present = true;
                post = false;
                UIpresent.SetActive(false);
                UIpost.SetActive(false);
                break;
            case "post":
                if (post == false)
                {
                    gameObjects[0].SetActive(false);
                    gameObjects[1].SetActive(false);
                    gameObjects[2].SetActive(true);
                   // Showinmap = gameObjects[2];
                }
                past = false;
                present = false;
                post = true;
                UIpresent.SetActive(false);
                UIpast.SetActive(false);
                break;
            default: Debug.Log("invalid sting");
                break;
        }
    }



}
