using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StarterAssets
{
    public class Checkhang : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "checkhang")
            {
                //ThirdPersonController.Instance._climbing = true;
                ThirdPersonController.Instance._climbing = true;
                ThirdPersonController.Instance._animator.SetBool(ThirdPersonController.Instance._animIDIshang, true);
                Debug.Log(ThirdPersonController.Instance._climbing);
                ThirdPersonController.Instance._verticalVelocity = 0f;
            }
        }
    }
}
