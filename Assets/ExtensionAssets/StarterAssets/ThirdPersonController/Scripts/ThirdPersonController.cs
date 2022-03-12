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
        [Header("Player")]
        [Tooltip("Prone speed of the character in m/s")]
        public float ProneSpeed = 2.0f;
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;
        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

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
        private float _animationBlendAction;
        private float _animationBlandMove_x;
        private float _animationBlandMove_y;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        [SerializeField] private float Sensitivity = 1f;
        [SerializeField] private bool _rotateOnMove = true;

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
        private int _animIDProne;
        private int _animIDmoove_x;
        private int _animIDmoove_y;
        private int _animIDFirstWeapon;
        private int _animIDSecondWeapon;
        private int _animIDThirdWeapon;
        private int _animIDFourthWeapon;
        private int _animIDAction;
        private int _animIDActionShoot;

        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;
        private bool isProne;
        private Weapons currentWeapon;

        private void Awake()
        {
            currentWeapon = Weapons.FirstWeapon;
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            _input = GetComponent<StarterAssetsInputs>();
            _input.OnProneCustom += Prone;
            _input.OnFirstWeaponCustom += SetWeapon;
            _input.OnSecondWeaponCustom += SetWeapon;
            _input.OnThirdWeaponCustom += SetWeapon;
            _input.OnFourthWeaponCustom += SetWeapon;
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
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
            CrouchLie();
            Move();
            Shoot();
            //print("_input.move.x " + _input.move.x);
            //print("_input.move.y " + _input.move.y);
            //print("_input.move.magnitude " + _input.move.magnitude);
            //print("_controller.velocity.x " + _controller.velocity.x);
            //print("_controller.velocity.y " + _controller.velocity.y);
            //print("_controller.velocity.z " + _controller.velocity.z);
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
            _animIDProne = Animator.StringToHash("Prone");
            _animIDmoove_x = Animator.StringToHash("move_x");
            _animIDmoove_y = Animator.StringToHash("move_y");
            _animIDFirstWeapon = Animator.StringToHash("FirstWeapon");
            _animIDSecondWeapon = Animator.StringToHash("SecondWeapon");
            _animIDThirdWeapon = Animator.StringToHash("ThirdWeapon");
            _animIDFourthWeapon = Animator.StringToHash("FourthWeapon");
            _animIDAction = Animator.StringToHash("Action");
            _animIDActionShoot = Animator.StringToHash("ActionShoot");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

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
                _cinemachineTargetYaw += _input.look.x * Time.deltaTime;
                _cinemachineTargetPitch += _input.look.y * Time.deltaTime;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed;
            if (_input.sprint) targetSpeed = SprintSpeed;
            else targetSpeed = MoveSpeed;
            if (isProne) targetSpeed = ProneSpeed;
            //targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            float target_x = 0;
            float target_y = 0;
            if (_input.move != Vector2.zero)
            {
                target_x = _input.move.x;
                target_y = _input.move.y;
            }
            else
            {
                target_x = 0;
                target_y = 0;
            }
            // update animator if using character
            _animationBlandMove_x = Mathf.Lerp(_animationBlandMove_x, target_x, Time.deltaTime * SpeedChangeRate);
            _animationBlandMove_y = Mathf.Lerp(_animationBlandMove_y, target_y, Time.deltaTime * SpeedChangeRate);
            //print("_animationBlend " +_animationBlend);
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                _animator.SetFloat(_animIDmoove_x, _animationBlandMove_x);
                _animator.SetFloat(_animIDmoove_y, _animationBlandMove_y);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                        _animator.SetBool(_animIDCrouch, false);
                        _animator.SetBool(_animIDProne, false);
                        isProne = false;
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
                    if (_hasAnimator)
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
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }

        public void SetSensitivity(float newSensitivity)
        {
            Sensitivity = newSensitivity;
        }
        public void SetRotateOnMove(bool newRotateOnMove)
        {
            _rotateOnMove = newRotateOnMove;
        }

        private void CrouchLie()
        {
            if (_input.crouch)
            {
                _animator.SetBool(_animIDCrouch, true);
            }
            else
            {
                _animator.SetBool(_animIDCrouch, false);
            }
        }

        private void Prone()
        {
            if (isProne)
            {
                _animator.SetBool(_animIDProne, false);
                isProne = false;
            }
            else
            {
                _animator.SetBool(_animIDProne, true);
                isProne = true;
            }
        }

        private void Shoot()
        {
            if (_input.shoot)
            {
                _animationBlendAction = Mathf.Lerp(_animationBlendAction, 1, Time.deltaTime * SpeedChangeRate);
                _animator.SetBool(_animIDActionShoot, true);
            }
            else
            {
                _animationBlendAction = Mathf.Lerp(_animationBlendAction, 0, Time.deltaTime * SpeedChangeRate);
                _animator.SetBool(_animIDActionShoot, false);
            }

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDAction, _animationBlendAction);
            }
        }
        private void SetWeapon(Weapons weapons)
        {
            currentWeapon = weapons;
            _animator.SetBool(_animIDFirstWeapon, false);
            _animator.SetBool(_animIDSecondWeapon, false);
            _animator.SetBool(_animIDThirdWeapon, false);
            _animator.SetBool(_animIDFourthWeapon, false);
            switch (currentWeapon)
            {
                case Weapons.FirstWeapon:
                    _animator.SetBool(_animIDFirstWeapon, true);
                    break;
                case Weapons.SecondWeapon:
                    _animator.SetBool(_animIDSecondWeapon, true);
                    break;
                case Weapons.ThirdWeapon:
                    _animator.SetBool(_animIDThirdWeapon, true);
                    break;
                case Weapons.FourthWeapon:
                    _animator.SetBool(_animIDFourthWeapon, true);
                    break;
            }
        }

        private void OnDestroy()
        {
            _input.OnProneCustom -= Prone;
            _input.OnFirstWeaponCustom -= SetWeapon;
            _input.OnSecondWeaponCustom -= SetWeapon;
            _input.OnThirdWeaponCustom -= SetWeapon;
            _input.OnFourthWeaponCustom -= SetWeapon;
        }
    }
}

public enum Weapons
{
    FirstWeapon,
    SecondWeapon,
    ThirdWeapon,
    FourthWeapon,
}