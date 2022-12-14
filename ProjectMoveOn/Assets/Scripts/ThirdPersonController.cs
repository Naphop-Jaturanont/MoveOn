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
        [SerializeField]private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        public float _verticalVelocity;
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
        public int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDCrouch;
        public int _animIDIshang;
        public int _animIDisClimbup;
        public int _animIDDown;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        public Animator _animator;
        private Rigidbody rigidbody;
        private CapsuleCollider capsule;
        private CharacterController _controller;
        public StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        public bool _climbing = false;
        public bool _ladderYZ = false;
        public bool crouch = false;
        public bool freefall = false;
        public bool openlamb = false;
        public bool keepLamb =false;
        private Vector3 endPosition;
        private RaycastHit downRaycastHit;
        private RaycastHit forwardRaycastHit;

        public bool _hasAnimator;
        

        public bool climbing = false;

        public LayerMask ladderMask;
        public Transform chestT;
        public Interactor interactor;
        RaycastHit hitinfo;
        
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
            interactor = GetComponent<Interactor>();
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
            Handup();
            
        }

       
        private void Handup()
        {
            if (_input.handup)
            {
                Debug.Log("chu lamp");
                openlamb = !openlamb;
                Debug.Log(openlamb);
                //chu lamb
                //increase area light
            }
            _input.handup = false;
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
            _animIDDown = Animator.StringToHash("Down");
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
            if (_input.move != Vector2.zero && _climbing == false)
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
            if (_input.move != Vector2.zero)
            {
                if (_ladderYZ == false && _climbing == false)
                {
                    _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
                }
                else if (_ladderYZ == true)
                {
                    transform.Translate(Vector3.up * 1.5f * Time.deltaTime);
                }
            }
            if(Grounded == false &&_ladderYZ == false)
            {                
                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            }

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
            if (interactor.handRight == false && interactor.handLeft == false)
            {
                Ray ray = new Ray(chestT.position, chestT.TransformDirection(Vector3.forward));
                if (Physics.Raycast(ray, out hitinfo, 5f, ladderMask, QueryTriggerInteraction.Ignore))
                {

                    if (_input.climb == true)
                    {
                        _ladderYZ = true;
                        _animator.SetBool(_animIDIshang, true);
                        //transform.Translate(Vector3.forward * 2);
                        _verticalVelocity = 0f;
                    }
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        _ladderYZ = false;
                        _animator.SetBool(_animIDIshang, false);
                        _animator.SetTrigger(_animIDDown);
                        _verticalVelocity = -2f;
                    }
                }
                Debug.DrawRay(chestT.position, chestT.TransformDirection(Vector3.forward) * hitinfo.distance, Color.red);
                _input.climb = false;
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
                    //freefall = false;
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f && _climbing ==false)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f &&_climbing == false )
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    _controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

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
                    if (_hasAnimator && _climbing == false)
                    {
                        _animator.SetBool(_animIDFreeFall, true);                        
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
                
            }
            if (Grounded == false)
            {

            }
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity && _climbing == false)
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
        public void ClimFromledge()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.8f, transform.position.z + 1.5f);
            //transform.position = checkhang.Getstanduppos(0);
            _animator.SetFloat(_animIDisClimbup, 0.0f);
            _animator.SetBool(_animIDIshang, false);
            
            Invoke("changebooleanclimb", 0.15f);
        }
        
        public void changebooleanclimb()
        {
            _ladderYZ = false;
            _climbing = false;
        }

    }
}