using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            starterAssetsInputs.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            starterAssetsInputs.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            starterAssetsInputs.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            starterAssetsInputs.SprintInput(virtualSprintState);
        }

        public void VirtualCrouchInput(bool virtualSprintState)
        {
            starterAssetsInputs.CrouchInput(virtualSprintState);
        }

        public void VirtualClimbInput(bool virtualSprintState)
        {
            starterAssetsInputs.ClimbInput(virtualSprintState);
        }

        public void VirtualhandupInput(bool virtualSprintState)
        {
            starterAssetsInputs.handupInput(virtualSprintState);
        }
        public void VirtualinteractRInput(bool virtualSprintState)
        {
            starterAssetsInputs.interactRInput(virtualSprintState);
        }
        public void VirtualinteractLInput(bool virtualSprintState)
        {
            starterAssetsInputs.interactLInput(virtualSprintState);
        }

    }

}
