using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StarterAssets
{
    public class CheckTop : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "checkhang")
            {
                //ThirdPersonController.Instance._ladderYZ = false;
                Debug.Log(ThirdPersonController.Instance._ladderYZ);
                ThirdPersonController.Instance._animator.SetFloat(ThirdPersonController.Instance._animIDisClimbup, 0.111f);//playanimation climb to stand
                ThirdPersonController.Instance._verticalVelocity = -2f;
                //ThirdPersonController.Instance.transform.Translate(Vector3.forward * 2);
                //ThirdPersonController.Instance.ClimFromledge();
            }
        }
    }
}
