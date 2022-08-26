using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkhang : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "checkhead")
        {
            Debug.Log(StarterAssets.ThirdPersonController.Instance._animator);
            StarterAssets.ThirdPersonController.Instance._climbing = true;
            StarterAssets.ThirdPersonController.Instance._animator.SetBool("isHang", true);
        }
    }
}
