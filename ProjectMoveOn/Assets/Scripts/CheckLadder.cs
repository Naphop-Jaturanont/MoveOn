using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class CheckLadder : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "checkhead")
            {
                ThirdPersonController.Instance._ladderYZ = true;
                Debug.Log(ThirdPersonController.Instance._ladderYZ);
                ThirdPersonController.Instance._animator.SetBool(ThirdPersonController.Instance._animIDIshang, true);
                //StarterAssets.ThirdPersonController.Instance._animator.SetBool("isHang", true);//playanimationclimb
                ThirdPersonController.Instance._verticalVelocity = 0f;
            }
        }
               
    }
}
