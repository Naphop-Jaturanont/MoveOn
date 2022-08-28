using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLadder : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "checkhead")
        {
            StarterAssets.ThirdPersonController.Instance._ladderYZ = true;
            Debug.Log(StarterAssets.ThirdPersonController.Instance._ladderYZ);
            //StarterAssets.ThirdPersonController.Instance._animator.SetBool("isHang", true);//playanimationclimb
            StarterAssets.ThirdPersonController.Instance._verticalVelocity = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "checkhead")
        {
            StarterAssets.ThirdPersonController.Instance._ladderYZ = false;
            Debug.Log(StarterAssets.ThirdPersonController.Instance._ladderYZ);
            //StarterAssets.ThirdPersonController.Instance._animator.SetBool("isHang", true);//playanimation climb to stand
            StarterAssets.ThirdPersonController.Instance._verticalVelocity = -2f;
            StarterAssets.ThirdPersonController.Instance.transform.Translate(Vector3.forward * 2);
        }
    }
}
