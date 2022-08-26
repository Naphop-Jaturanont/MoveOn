using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        public static ThirdPersonController Instance;

         

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("Move speed crouch of the character in m/s")]
        public float CrouchMoveSpeed = 1.0f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        //Climb Setting
        [Header("Climb Setting")]
        [SerializeField] private float _walkAngleMax;
        [SerializeField] private float _groundAngleMax;
        [SerializeField] private LayerMask _layermaskClimb;

        //Heights
        [Header("Heights")]
        [SerializeField] private float _overpassHeights;
        [SerializeField] private float _hangHeights;
        [SerializeField] private float _climbUpHeights;
        [SerializeField] private float _vaultHeights;
        [SerializeField] private float stepHeights;

        [Header("Offsets")]
        [SerializeField] private Vector3 endoffset;
        [SerializeField] private Vector3 climbOriginDown;

        [Header("Animation setting")]
        public crossfangSetting standtoFreeHandSetting;
        public crossfangSetting climbUpSetting;
        public crossfangSetting vaultSetting;
        public crossfangSetting stepUpSetting;
        public crossfangSetting dropSetting;
        public crossfangSetting dropToAirSetting;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDCrouch;
        public int _animIDIshang;
        private int _animIDisClimbup;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        public Animator _animator;
        private Rigidbody rigidbody;
        private CapsuleCollider capsule;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        public bool _climbing = false;
        public bool crouch = false;
        public bool freefall = false;
        private Vector3 endPosition;
        private RaycastHit downRaycastHit;
        private RaycastHit forwardRaycastHit;

        public bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }else if(Instance != null)
            {
                Destroy(this);
            }
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            rigidbody = GetComponent<Rigidbody>();
            capsule = GetComponent<CapsuleCollider>();
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
            Crouch();
            ClimbbUP();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDCrouch = Animator.StringToHash("Crouch");
            _animIDIshang = Animator.StringToHash("isHang");
            _animIDisClimbup = Animator.StringToHash("isClimbup");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void Crouch()
        {
            if (Grounded && freefall == false)
            {

                // set target speed based on move speed, sprint speed and if sprint is pressed
                float targetSpeed = _input.Crouch ? CrouchMoveSpeed : MoveSpeed;

                //if (_input.Crouch == false) targetSpeed = 0.0f;



                if (_hasAnimator )
                {
                    _animator.SetBool(_animIDCrouch, _input.Crouch);
                    crouch = true;
                    if (_input.Crouch == true && _input.move == Vector2.zero)
                    {
                        _controller.height = 0.9f;
                        _controller.center = new Vector3(0, 0.49f, 0);
                    }
                    if (_input.Crouch == true && _input.move != Vector2.zero)
                    {

                        _controller.height = 1.2f;
                        _controller.center = new Vector3(0, 0.65f, 0);
                    }

                }

                if (_input.Crouch == false)
                {
                    crouch = false;
                    _controller.height = 1.8f;
                    _controller.center = new Vector3(0, 0.93f, 0);
                }

            }
        }

        private void ClimbbUP()
        {
            if (_climbing == true)
            {
                if (_input.climb == true)
                {
                    Debug.Log("climbup");
                }
            }
            _input.climb = false;
        }

        private bool canClimb(out RaycastHit _downRaycastHit, out RaycastHit _forwardRaycastHit, out Vector3 _endPosition)
        {
            _endPosition = Vector3.zero;
            _downRaycastHit = new RaycastHit();
            _forwardRaycastHit = new RaycastHit();

            bool downHit;
            bool forwardHit;
            bool overpassHit;
            float climbHeight;
            float groundAngle;
            float wallAngle;

            //RaycastHit downRaycastHit;
           // RaycastHit forwardRaycastHit;
            RaycastHit overpassRaycastHit;

            Vector3 endPosition;
            Vector3 forwardDirectionXZ;
            Vector3 forwardNormalXZ;

            Vector3 downDirection = Vector3.down;
            Vector3 downOrigin = transform.TransformPoint(climbOriginDown);

            downHit = Physics.Raycast(downOrigin, downDirection, out downRaycastHit, climbOriginDown.y, _layermaskClimb);
            if (downHit)
            {
                //forward + over pass
                float forwardDistance = climbOriginDown.z;
                Vector3 forwardOrigin = new Vector3(transform.position.x, downRaycastHit.point.y - 0.1f, transform.position.z);
                Vector3 overpassOrigin = new Vector3(transform.position.x, _overpassHeights, transform.position.z);

                forwardDirectionXZ = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
                forwardHit = Physics.Raycast(forwardOrigin, forwardDirectionXZ, out forwardRaycastHit,forwardDistance , _layermaskClimb);
                overpassHit = Physics.Raycast(overpassOrigin, forwardDirectionXZ, out overpassRaycastHit,forwardDistance , _layermaskClimb);
                climbHeight = downRaycastHit.point.y - transform.position.y;

                if (forwardHit)
                {
                    if(overpassHit || climbHeight < _overpassHeights)
                    {
                        //Angle
                        forwardNormalXZ = Vector3.ProjectOnPlane(forwardRaycastHit.normal, Vector3.up);
                        groundAngle = Vector3.Angle(downRaycastHit.normal, Vector3.up);
                        wallAngle = Vector3.Angle(forwardNormalXZ, forwardDirectionXZ);

                        if(wallAngle <= _walkAngleMax)
                        {
                            if (groundAngle <= _groundAngleMax)
                            {
                                //End offset
                                Vector3 vectSurface = Vector3.ProjectOnPlane(forwardDirectionXZ, downRaycastHit.normal);
                                endPosition = downRaycastHit.point + Quaternion.LookRotation(vectSurface, Vector3.up) * endoffset;

                                //De-penetration
                                Collider colliderB = downRaycastHit.collider;
                                bool penetrationOverlap = Physics.ComputePenetration(
                                    colliderA: capsule,
                                    positionA: endPosition,
                                    rotationA: transform.rotation,
                                    colliderB: colliderB,
                                    positionB: colliderB.transform.position,
                                    rotationB: colliderB.transform.rotation,
                                    direction: out Vector3 penetrationDirection,
                                    distance: out float penetrationDistance);
                                if (penetrationOverlap)
                                    endPosition += penetrationDirection * penetrationDistance;

                                //up Sweep
                                float inflate = -0.05f;
                                float upsweepDistance = downRaycastHit.point.y - transform.position.y;
                                Vector3 upSweepDirection = transform.up;
                                Vector3 upSweepOrigin = transform.position;
                                bool upSweepHit = CharacterSweep(
                                    position: upSweepOrigin,
                                    rotation: transform.rotation,
                                    direction: upSweepDirection,
                                    distance: upsweepDistance,
                                    layerMask: _layermaskClimb,
                                    inflate: inflate);

                                //forwardSweep
                                Vector3 forwardSweepOrigin = transform.position + upSweepDirection * upsweepDistance;
                                Vector3 forwardSweepVector = endPosition - forwardSweepOrigin;
                                bool forwardSweepHit = CharacterSweep(
                                    position: forwardSweepOrigin,
                                    rotation: transform.rotation,
                                    direction: forwardSweepVector.normalized,
                                    distance: forwardSweepVector.magnitude,
                                    layerMask: _layermaskClimb,
                                    inflate: inflate);

                                if(!upSweepHit && forwardSweepHit)
                                {
                                    _endPosition = endPosition;
                                    _downRaycastHit = downRaycastHit;
                                    _forwardRaycastHit = forwardRaycastHit;
                                    return true;
                                }
                            }
                        }
                    }
                }

            }
            return false;
        }

        private bool CharacterSweep(Vector3 position, Quaternion rotation,Vector3 direction, float distance, LayerMask layerMask, float inflate)
        {
            float heightScale = Mathf.Abs(transform.lossyScale.y);
            float radiusScale = Mathf.Max(Mathf.Abs(transform.lossyScale.x), Mathf.Abs(transform.lossyScale.z));

            float radius = capsule.radius * radiusScale;
            float totalheights = Mathf.Max(capsule.height * heightScale, radius * 2);

            Vector3 capsuleup = rotation * Vector3.up;
            Vector3 center = position + rotation * capsule.center;
            Vector3 top = center + capsuleup * (totalheights / 2 - radius);
            Vector3 bottom = center - capsuleup * (totalheights / 2 - radius);

            bool sweepHit = Physics.CapsuleCast(
                point1: bottom,
                point2: top,
                radius: radius,
                direction: direction,
                maxDistance: distance,
                layerMask: _layermaskClimb);

            return sweepHit;
        }

        private void InitiateClimb()
        {
            _climbing = true;
            _speed = 0;
            _animator.SetFloat("Speed", 0);//Forward
            capsule.enabled = false;
            rigidbody.isKinematic = true;

            float climheight = downRaycastHit.point.y - transform.position.y;

            if(climheight > _hangHeights)
            {
                _animator.CrossFadeInFixedTime(standtoFreeHandSetting);
            }
            else if(climheight > _climbUpHeights)
            {
                _animator.CrossFadeInFixedTime(climbUpSetting);
            }
            else if(climheight > _vaultHeights)
            {
                _animator.CrossFadeInFixedTime(vaultSetting);
            }
            else if(climheight > stepHeights)
            {
                _animator.CrossFadeInFixedTime(stepUpSetting);
            }
            else
            {
                _climbing = false;
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded && crouch == false)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                    freefall = false;
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f  )
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator )
                    {
                        _animator.SetBool(_animIDFreeFall, true);                        
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
                
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        

    }
}